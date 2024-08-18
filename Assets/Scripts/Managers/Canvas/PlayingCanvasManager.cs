using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class PlayingCanvasManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI enlargeRemainingValue;
    [SerializeField] private TextMeshProUGUI shrinkRemainingValue;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private GameObject orthographicCamera;
    [SerializeField] private GameObject perspectiveCamera;

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

}
