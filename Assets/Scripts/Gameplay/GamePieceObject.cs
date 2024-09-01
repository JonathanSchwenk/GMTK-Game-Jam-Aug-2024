using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using Unity.VisualScripting;
using System;

public class GamePieceObject : MonoBehaviour {

    public float weight = 1;
    public bool isPlaced = false;
    public bool canPlace = true;
    public string category;
    public string gamePieceName;

    public List<GameObject> collidingPieces = new List<GameObject>();
    private Color originalColor;
    private bool justSpawned = true;
    private float targetWidthHeight = 60f;
    private float largePieceSize = 0.9f;


    private IGamePieceManager gamePieceManager;
    private IPlayingCanvasManager playingCanvasManager;
    private IPowerupManager powerupManager;
    private ICanvasManager canvasManager;
    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        playingCanvasManager = ServiceLocator.Resolve<IPlayingCanvasManager>();
        powerupManager = ServiceLocator.Resolve<IPowerupManager>();
        canvasManager = ServiceLocator.Resolve<ICanvasManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();

        originalColor = gameObject.transform.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {
        if (!isPlaced) {
            if (collidingPieces.Count != 0) {
                canPlace = false;
                gameObject.transform.GetComponent<Renderer>().material.color = new Color(1, 0, 99 / 255, 1);
            } else {
                canPlace = true;
                gameObject.transform.GetComponent<Renderer>().material.color = originalColor;
            }
        }
    }

    private void OnMouseDown() {
        // gamePieceManager.activeGamePiece = this; // For testing
        if (isPlaced) {
            if (gamePieceManager.activelyDestroying) {
                // Destroy and deal with the subtraction of points
                // Do this by acting as if you were going to place the piece to calculate the points and then subtract instead of add
                gamePieceManager.subtractPoints(this);

                // Destroy the object
                Destroy(gameObject);
    
                // Continue timer
                playingCanvasManager.countdownTimerIncrement = 0.1f;

                // change the icon back to the original
                powerupManager.globalOgPowerupButton.transform.GetChild(1).gameObject.SetActive(true);
                powerupManager.globalOgPowerupButton.transform.GetChild(2).gameObject.SetActive(false);
            } else {
                if (Utilities.IsClickOverUI(canvasManager.graphicRaycaster, gameManager.eventSystem)) {
                    return;
                }
                // Show the description of the piece
                playingCanvasManager.ShowTappedPieceDescription(this.gameObject, category, gamePieceName);
            }
        }
    }

    // Move
    private void OnMouseDrag() {
        if (!isPlaced && Time.timeScale != 0 && playingCanvasManager.countdownTimerIncrement > 0) {
            Vector3 pos = GetMouseWorldPosition();
            transform.position = new Vector3(pos.x, 0, pos.z);
        }
    }

    // private Vector3 GetMouseWorldPosition() 
    // {
    //     // Get the mouse position in screen space
    //     Vector3 mousePosition = Input.mousePosition;

    //     return Camera.main.ScreenToWorldPoint(mousePosition);
    // }

    private Vector3 GetMouseWorldPosition() {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

        // Convert screen space to world space
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "GamePiece") {
            collidingPieces.Add(other.gameObject);
        }
        if (other.gameObject.tag == "Border") {
            collidingPieces.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Border") {
            canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "GamePiece") {
            collidingPieces.Remove(other.gameObject);
        }
        if (other.gameObject.tag == "Border") {
            if (justSpawned) {
                Vector3 size = gameObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;
                float averageSize = (size.x + size.z) / 2;
                float scaleMultiplier = Mathf.Pow(targetWidthHeight / averageSize, 0.5f);
                print(scaleMultiplier);

                if (scaleMultiplier < 1) {
                    scaleMultiplier = largePieceSize;
                }

                gameObject.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
                justSpawned = false;
            }
            collidingPieces.Remove(other.gameObject);
        }
    }
}
