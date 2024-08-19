using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class GameOverCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI yourScoreValue;
    [SerializeField] private TextMeshProUGUI bestScoreValue;
    [SerializeField] private TextMeshProUGUI roundsMaxChainValue;
    [SerializeField] private TextMeshProUGUI bestMaxChainValue;


    private IGamePieceManager gamePieceManager;
    private IGameManager gameManager;
    private IAudioManager audioManager;
    private ISaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();
        saveManager = ServiceLocator.Resolve<ISaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        yourScoreValue.text = gamePieceManager.score.ToString();
        bestScoreValue.text = saveManager.saveData.highScore.ToString();
        roundsMaxChainValue.text = gamePieceManager.curMaxChain.ToString();
        bestMaxChainValue.text = saveManager.saveData.highChain.ToString();
    }

    public void OnReplayPressed() {
        // Resets the game state and starts a new round
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.Playing);
        
        audioManager.PlaySFX("UIClick_General");
        audioManager.PlaySFX("StartGame");
        audioManager.StopMusic("MenuMusic");
        audioManager.PlayMusic("GameplayMusic");
    }

    public void OnQuitPressed() {
        audioManager.PlaySFX("UIClick_General");
        // Quits the game
        Application.Quit();
    }

    public void OnMainMenuPressed() {
        audioManager.PlaySFX("UIClick_General");
        // Returns to the main menu after resetting
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.StartMenu);
    }
}
