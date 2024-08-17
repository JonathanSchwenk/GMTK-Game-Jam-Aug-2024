using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using Unity.VisualScripting;
using System;

public class GamePieceObject : MonoBehaviour
{

    public float weight = 1;
    public bool isPlaced = false;
    public string category;
    
    private List<GameObject> collidingPieces = new List<GameObject>();
    private Color originalColor;

    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start()
    {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        originalColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced) {
            if (collidingPieces.Count != 0) {
                GetComponent<Renderer>().material.color = Color.red;
            } else {
                GetComponent<Renderer>().material.color = originalColor;
            }
        }
    }

    // For testing purposes
    private void OnMouseDown() {
        gamePieceManager.activeGamePiece = this;
    }

    // Move
    private void OnMouseDrag() {
        print("Dragging");
        // if (gamePieceManager.activeGamePiece != null) {
            Vector3 pos = GetMouseWorldPosition(); //- new Vector3(0, Camera.main.transform.position.y, 0);
            transform.position = new Vector3(pos.x, 0, pos.z);
        // }
    }

    private Vector3 GetMouseWorldPosition() 
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;
        
        // Since you're using an orthographic camera, you don't need to consider depth; 
        // just convert the screen position to world position
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // Change color if the object is colliding with another object
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "GamePiece") {
            collidingPieces.Add(other.gameObject);
        }
    }

    // Change color back to original if the object is no longer colliding with another object
    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "GamePiece") {
            collidingPieces.Remove(other.gameObject);
        }
    }
}
