using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;
using UnityEngine.UI;

public class PlayingCanvasManager : MonoBehaviour, IPlayingCanvasManager {
    [SerializeField, Header("UI Elements"), Space(10)]
    private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI enlargeRemainingValue;
    [SerializeField] private TextMeshProUGUI shrinkRemainingValue;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timerText;
    // [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private GameObject pauseIcon;
    [SerializeField] private GameObject playIcon;
    [SerializeField] private TextMeshProUGUI pointsEarnedText;
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private TextMeshProUGUI areaText;

    [SerializeField, Header("Cameras"), Space(10)]
    private GameObject orthographicCamera;
    [SerializeField] private GameObject perspectiveCamera;

    [SerializeField, Header("Buttons"), Space(10)]
    private GameObject placeButtonFill;
    [SerializeField] private GameObject enlargeButtonFill;
    [SerializeField] private GameObject shrinkButtonFill;

    [SerializeField, Header("Other GameObjects"), Space(10)]
    private GameObject spawner;
    [SerializeField] private Canvas playingCanvas;
    [SerializeField] private TappedGamePieceDesc[] tappedPieceDescList;

    public float countdownTimer { get; set; }
    public float countdownTimerIncrement { get; set; }

    private IGamePieceManager gamePieceManager;
    private IGameManager gameManager;
    private IAudioManager audioManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();

        countdownTimerIncrement = 0.1f;
    }

    // Update is called once per frame
    void Update() {
        scoreValue.text = Mathf.RoundToInt(gamePieceManager.score).ToString();
        gemsText.text = gamePieceManager.curGems.ToString();
        enlargeRemainingValue.text = gamePieceManager.enlargeRemaining.ToString();
        shrinkRemainingValue.text = gamePieceManager.shrinkRemaining.ToString();
        if (gamePieceManager.activeGamePiece != null) {
            categoryText.text = gamePieceManager.activeGamePiece.category;
            nameText.text = gamePieceManager.activeGamePiece.gamePieceName;
        } else {
            categoryText.text = "Category";
            nameText.text = "Name";
        }
        timerText.text = countdownTimer.ToString("F1");
        pointsEarnedText.text = Mathf.RoundToInt(gamePieceManager.pointsEarned).ToString();
        areaText.text = gamePieceManager.gamePieceArea.ToString("F2");

        UpdateButtonsFill();
    }

    // Update the fill color of the buttons
    private void UpdateButtonsFill() {
        // Place button
        if (gamePieceManager.activeGamePiece != null) {
            if (gamePieceManager.activeGamePiece.canPlace && countdownTimerIncrement > 0 && Time.timeScale != 0) {
                placeButtonFill.GetComponent<Image>().color = Constants.buttonOnColor;
            } else {
                placeButtonFill.GetComponent<Image>().color = Constants.buttonOffColor;
            }
        }

        // Enlarge button
        if (gamePieceManager.enlargeRemaining > 0 && countdownTimerIncrement > 0 && Time.timeScale != 0) {
            enlargeButtonFill.GetComponent<Image>().color = Constants.buttonOnColor;
        } else {
            enlargeButtonFill.GetComponent<Image>().color = Constants.buttonOffColor;
        }

        // Shrink button
        if (gamePieceManager.shrinkRemaining > 0 && countdownTimerIncrement > 0 && Time.timeScale != 0) {
            shrinkButtonFill.GetComponent<Image>().color = Constants.buttonOnColor;
        } else {
            shrinkButtonFill.GetComponent<Image>().color = Constants.buttonOffColor;
        }
    }


    # region Button Functions

    public void OnPlaceObjectPressed() {
        if (Time.timeScale != 0) {
            if (gamePieceManager.activeGamePiece.canPlace) {
                gamePieceManager.Place();
                audioManager.PlaySFX("UIClick_General");
                audioManager.PlaySFX("PlaceGamePiece");
            } else {
                audioManager.PlaySFX("UIClick_Error");
            }
        }
    }

    public void OnEnlargeObjectPressed() {
        if (Time.timeScale != 0 && countdownTimerIncrement > 0) {
            gamePieceManager.Enlarge();
        }
    }

    public void OnShrinkObjectPressed() {
        if (Time.timeScale != 0 && countdownTimerIncrement > 0) {
            gamePieceManager.Shrink();
        }
    }

    public void ToggleCamera() {
        orthographicCamera.SetActive(!orthographicCamera.activeSelf);
        perspectiveCamera.SetActive(!perspectiveCamera.activeSelf);

        audioManager.PlaySFX("UIClick_General");
    }

    public void PauseButton() {
        if (gameManager.gameState != GameState.Tutorial) {
            audioManager.PlaySFX("UIClick_General");
            if (Time.timeScale == 0) {
                // pauseText.text = "Pause";
                pauseIcon.SetActive(true);
                playIcon.SetActive(false);
                Time.timeScale = 1;
            } else {
                // pauseText.text = "Play";
                pauseIcon.SetActive(false);
                playIcon.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void GiveUpButton() {
        if (gameManager.gameState != GameState.Tutorial) {
            audioManager.PlaySFX("UIClick_General");
            audioManager.PlaySFX("GameOver");
            audioManager.PlayMusic("MenuMusic");
            audioManager.StopMusic("GameplayMusic");

            gameManager.UpdateGameState(GameState.GameOver);
        }
    }

    public void ResetButton() {
        // Scale values
        gamePieceManager.enlargeRemaining = gamePieceManager.curEnlargeValue;
        gamePieceManager.shrinkRemaining = gamePieceManager.curShrinkValue;
        gamePieceManager.tempEnlargeShrinkValue = 0;

        // move and scale back
        Vector3 size = gamePieceManager.activeGamePiece.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        float scaleMultiplier = 30f / ((size.x + size.z) / 2);
        gamePieceManager.activeGamePiece.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
        gamePieceManager.activeGamePiece.transform.position = spawner.transform.position;
    }

    #endregion

    # region Coroutine Functions

    public void StartCountdown(float duration) {
        StartCoroutine(CountdownCoroutine(duration));
    }

    public void StopCountdown() {
        StopAllCoroutines();
    }

    private IEnumerator CountdownCoroutine(float duration) {
        countdownTimer = duration;

        while (countdownTimer > 0) {
            // Print the remaining time to the tenth of a second
            // Debug.Log("Time remaining: " + countdownTimer.ToString("F1") + " seconds");

            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Decrease the remaining time
            countdownTimer -= countdownTimerIncrement;
        }

        // Ensure remainingTime does not go negative
        countdownTimer = 0;

        // Game over
        audioManager.PlaySFX("GameOver");
        audioManager.PlayMusic("MenuMusic");
        audioManager.StopMusic("GameplayMusic");
        gameManager.UpdateGameState(GameState.GameOver);
    }

    #endregion


    public void ShowTappedPieceDescription(GameObject obj, string category, string name) {
        for (int i = 0; i < tappedPieceDescList.Length; i++) {
            if (tappedPieceDescList[i].tappedPiece.activeSelf == false) {
                tappedPieceDescList[i].tappedPiece.SetActive(true);
                tappedPieceDescList[i].tappedPiece.transform.localPosition = Utilities.GetCanvasPosition(
                    obj.transform.position,
                    Camera.main,
                    playingCanvas
                );
                tappedPieceDescList[i].categoryText.text = category;
                tappedPieceDescList[i].nameText.text = name;

                StartCoroutine(Utilities.DisableObjectAfterTime(tappedPieceDescList[i].tappedPiece, 2.5f));

                break;
            }
        }
    }

}
