using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class PlayingCanvasManager : MonoBehaviour, IPlayingCanvasManager {
    [SerializeField] private TextMeshProUGUI enlargeRemainingValue;
    [SerializeField] private TextMeshProUGUI shrinkRemainingValue;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject orthographicCamera;
    [SerializeField] private GameObject perspectiveCamera;

    public float countdownTimer {get; set;}

    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
    }

    // Update is called once per frame
    void Update() {
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
        Debug.Log("Time remaining: 0.0 seconds");

        // Print out when the time has run out
        Debug.Log("Time's up! You lost!");
        // TODO: End the game
    }

}
