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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = ServiceLocator.Resolve<IGameManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPressed() {
        Debug.Log("Start button pressed");
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.Playing);
    }

    public void OnTutorialButtonPressed() {
        Debug.Log("Tutorial button pressed");
        tutorialCanvasManager.currentPage = 0;
        gameManager.UpdateGameState(GameState.Tutorial);
    }

    public void OnExitButtonPressed() {
        Debug.Log("Exit button pressed");
        Application.Quit();
    }
}
