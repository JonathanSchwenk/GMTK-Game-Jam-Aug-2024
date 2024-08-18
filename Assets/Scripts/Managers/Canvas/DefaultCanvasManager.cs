using System.Collections;
using System.Collections.Generic;
using Dorkbots.ServiceLocatorTools;
using TMPro;
using UnityEngine;

public class DefaultCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreValue;

    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
    }

    // Update is called once per frame
    void Update() {
        scoreValue.text = gamePieceManager.score.ToString();
    }
}
