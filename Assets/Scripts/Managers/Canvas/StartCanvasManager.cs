using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class StartCanvasManager : MonoBehaviour
{

    // [SerializeField] private TextMeshProUGUI bestScoreValue;
    // [SerializeField] private TextMeshProUGUI bestChainValue;

    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = ServiceLocator.Resolve<IGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPressed() {
        Debug.Log("Start button pressed");
        gameManager.UpdateGameState(GameState.Playing);
    }

    public void OnTutorialButtonPressed() {
        Debug.Log("Tutorial button pressed");
    }

    public void OnExitButtonPressed() {
        Debug.Log("Exit button pressed");
        Application.Quit();
    }

    public void OnSettingsButtonPressed() {
        Debug.Log("Settings button pressed");
    }
}
