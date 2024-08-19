using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class GameOverCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI yourScoreValue;


    private IGamePieceManager gamePieceManager;
    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        yourScoreValue.text = gamePieceManager.score.ToString();
    }

    public void OnReplayPressed() {
        // Resets the game state and starts a new round
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.Playing);
    }

    public void OnQuitPressed() {
        // Quits the game
        Application.Quit();
    }

    public void OnMainMenuPressed() {
        // Returns to the main menu after resetting
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.StartMenu);
    }
}
