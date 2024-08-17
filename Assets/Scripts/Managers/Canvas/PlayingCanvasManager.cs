using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class PlayingCanvasManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI enlargeRemainingValue;
    [SerializeField] private TextMeshProUGUI shrinkRemainingValue;

    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
    }

    // Update is called once per frame
    void Update() {
        scoreValue.text = gamePieceManager.score.ToString();
        enlargeRemainingValue.text = gamePieceManager.enlargeRemaining.ToString();
        shrinkRemainingValue.text = gamePieceManager.shrinkRemaining.ToString();
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

}
