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

    private List<GameObject> collidingPieces = new List<GameObject>();
    private Color originalColor;

    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        originalColor = gameObject.transform.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {
        if (!isPlaced) {
            if (collidingPieces.Count != 0) {
                gameObject.transform.GetComponent<Renderer>().material.color = new Color(1, 0, 99/255, 1);
            } else {
                gameObject.transform.GetComponent<Renderer>().material.color = originalColor;
            }
        }
    }

    // For testing purposes
    // private void OnMouseDown() {
    //     gamePieceManager.activeGamePiece = this;
    // }

    // Move
    private void OnMouseDrag() {
        if (!isPlaced) {
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

        // Set the z-coordinate of the mouse position to a fixed distance from the camera
        // This distance is how far away you want the object to be from the camera in world space
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

        // Convert screen space to world space
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // Change color if the object is colliding with another object
    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.tag == "GamePiece") {
    //         collidingPieces.Add(other.gameObject);
    //     }
    // }

    // Change color back to original if the object is no longer colliding with another object
    // private void OnCollisionExit(Collision other) {
    //     if (other.gameObject.tag == "GamePiece") {
    //         collidingPieces.Remove(other.gameObject);
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "GamePiece") {
            collidingPieces.Add(other.gameObject);
        }
        if (other.gameObject.tag == "Border") {
            canPlace = false;
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
            canPlace = true;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
