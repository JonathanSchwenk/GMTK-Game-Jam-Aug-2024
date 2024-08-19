using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class StartCanvasManager : MonoBehaviour
{

    // [SerializeField] private TextMeshProUGUI bestScoreValue;
    // [SerializeField] private TextMeshProUGUI bestChainValue;

    [SerializeField] private TutorialCanvasManager tutorialCanvasManager;

    private IGameManager gameManager;
    private IGamePieceManager gamePieceManager;
    private IAudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = ServiceLocator.Resolve<IGameManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();

        if (audioManager != null && gameManager.gameState != GameState.Playing) {
            audioManager.StopMusic("GameplayMusic");
            audioManager.PlayMusic("MenuMusic");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPressed() {
        // Sound and music
        audioManager.PlaySFX("UIClick_General");
        audioManager.PlaySFX("StartGame");
        audioManager.StopMusic("MenuMusic");
        audioManager.PlayMusic("GameplayMusic");

        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.Playing);
    }

    public void OnTutorialButtonPressed() {
        audioManager.PlaySFX("UIClick_General");
        tutorialCanvasManager.currentPage = 0;
        gameManager.UpdateGameState(GameState.Tutorial);
    }

    public void OnExitButtonPressed() {
        audioManager.PlaySFX("UIClick_General");
        Application.Quit();
    }
}
