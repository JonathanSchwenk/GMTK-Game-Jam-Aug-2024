using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour, ICanvasManager {

    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject playingCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject worldSpaceActivePieceCanvas;

    public GraphicRaycaster graphicRaycaster { get; set; }

    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        gameManager = ServiceLocator.Resolve<IGameManager>();

        if (gameManager != null) {
            gameManager.OnGameStateChanged += HandleGameStateChanged;
        }

        DeactivateAllCanvases();
        startMenuCanvas.SetActive(true);
    }

    private void HandleGameStateChanged(GameState newState) {
        // Handle the game state change here
        Debug.Log("Game state changed to: " + newState);

        switch (newState) {
            case GameState.StartMenu:
            graphicRaycaster = startMenuCanvas.GetComponent<GraphicRaycaster>();
                DeactivateAllCanvases();
                startMenuCanvas.SetActive(true);
                break;
            case GameState.Tutorial:
                graphicRaycaster = tutorialCanvas.GetComponent<GraphicRaycaster>();
                DeactivateAllCanvases();
                tutorialCanvas.SetActive(true);
                playingCanvas.SetActive(true);
                worldSpaceActivePieceCanvas.SetActive(true);
                break;
            case GameState.Playing:
                graphicRaycaster = playingCanvas.GetComponent<GraphicRaycaster>();
                DeactivateAllCanvases();
                playingCanvas.SetActive(true);
                worldSpaceActivePieceCanvas.SetActive(true);
                break;
            case GameState.GameOver:
                graphicRaycaster = gameOverCanvas.GetComponent<GraphicRaycaster>();
                DeactivateAllCanvases();
                gameOverCanvas.SetActive(true);
                break;
        }
    }

    private void OnDestroy() {
        if (gameManager != null) {
            gameManager.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void DeactivateAllCanvases() {
        startMenuCanvas.SetActive(false);
        tutorialCanvas.SetActive(false);
        playingCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        worldSpaceActivePieceCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }
}
