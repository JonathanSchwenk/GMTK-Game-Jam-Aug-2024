using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class PlayingCanvasManager : MonoBehaviour, IPlayingCanvasManager {
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI enlargeRemainingValue;
    [SerializeField] private TextMeshProUGUI shrinkRemainingValue;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private GameObject orthographicCamera;
    [SerializeField] private GameObject perspectiveCamera;

    public float countdownTimer {get; set;}

    private IGamePieceManager gamePieceManager;
    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
    }

    // Update is called once per frame
    void Update() {
        scoreValue.text = gamePieceManager.score.ToString();
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
    }

    public void OnPlaceObjectPressed() {
        gamePieceManager.Place();
    }

    public void OnEnlargeObjectPressed() {
        gamePieceManager.Enlarge();
    }

    public void OnShrinkObjectPressed() {
        gamePieceManager.Shrink();
    }

    public void ToggleCamera() {
        orthographicCamera.SetActive(!orthographicCamera.activeSelf);
        perspectiveCamera.SetActive(!perspectiveCamera.activeSelf);
    }

    public void PauseButton() {
        if (Time.timeScale == 0) {
            pauseText.text = "Pause";
            Time.timeScale = 1;
        } else {
            pauseText.text = "Play";
            Time.timeScale = 0;
        }
    }

    public void GiveUpButton() {
        gameManager.UpdateGameState(GameState.GameOver);
    }

    // Add this function to your existing PlayingCanvasManager script
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
            countdownTimer -= 0.1f;
        }

        // Ensure remainingTime does not go negative
        countdownTimer = 0;
        
        // Game over
        gameManager.UpdateGameState(GameState.GameOver);
    }

}
