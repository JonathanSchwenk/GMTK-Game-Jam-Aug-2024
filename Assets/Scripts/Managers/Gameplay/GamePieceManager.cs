using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceManager : MonoBehaviour, IGamePieceManager {
    // public List<GameObject> gamePieces = new List<GameObject>(); // Not sure if I even need this

    public GamePieceObject activeGamePiece { get; set; }
    public float score { get; set; }
    public float enlargeRemaining { get; set; }
    public float shrinkRemaining { get; set; }

    private float baseWeight = 1f; // Base weight value
    private float weightMultiplier = 0.05f; // Multiplier to adjust weight scaling
    private float weightCap_Max = 7500f; // Maximum weight value
    private float weightCap_Min = 10f; // Minimum weight value
    
    private float changeSizeValue = 0.1f; // Value to change the size of the game piece by
    private float tempEnlargeRemaining;
    private float tempShrinkRemaining;


    // Start is called before the first frame update
    void Start() {
        InitRoundStats();
    }

    // Update is called once per frame
    void Update() {

    }

    // Place
    public void Place() {
        if (activeGamePiece != null) {
            // Get the weight of the active game piece
            AssignWeight();

            // Reduce the enlarge and shrink values by the temporary values
            enlargeRemaining -= tempEnlargeRemaining;
            shrinkRemaining -= tempShrinkRemaining;

            // Reset the temporary enlarge and shrink values
            tempEnlargeRemaining = 0;
            tempShrinkRemaining = 0;

            // Add the weight of the active game piece to the score
            // TODO: use CalculateScore() function later. I need to somehow see if the object is close to other objects of the same 
            // category
            score += activeGamePiece.weight;

            activeGamePiece.isPlaced = true;
            activeGamePiece = null;
        }
    }

    // Calculate score
    private void CalculateScore() {
        /*
            Get the objects near the active game piece
            See if they are of the same category
            If they are, add some value to a multiplier and then check that object for other objects of the same category near it
            Make sure that you skip objects that have already been checked
                Can do this by adding them to a list of checked objects

            Use recursion
            Start with the active game piece that you just placed
            Check for objects of the same category near it
            If there are objects of the same category near it, add some value to a multiplier
            recurse on the objects of the same category near it
        */
    }

    // Enlarge
    public void Enlarge() {
        if (activeGamePiece != null) {
            if (enlargeRemaining - tempEnlargeRemaining > 0 && GetWeight() < weightCap_Max) {
                // Enlarge the active game piece
                activeGamePiece.transform.localScale += new Vector3(changeSizeValue, changeSizeValue, changeSizeValue);
                tempEnlargeRemaining += 1;
            }
        }
    }

    // Shrink
    public void Shrink() {
        if (activeGamePiece != null) {
            if (shrinkRemaining - tempShrinkRemaining > 0 && GetWeight() > weightCap_Min) {
                // Shrink the active game piece
                activeGamePiece.transform.localScale -= new Vector3(changeSizeValue, changeSizeValue, changeSizeValue);
                tempShrinkRemaining += 1;
            }
        }
    }

    private void InitRoundStats() {
        score = 0;
        enlargeRemaining = 500;
        shrinkRemaining = 500;
    }

    // Get Weight
    private float GetWeight() {
        Mesh mesh = activeGamePiece.gameObject.GetComponent<MeshFilter>().sharedMesh;

        if (mesh != null) {
            // Calculate the size of the mesh using its bounds
            Vector3 size = mesh.bounds.size;
            Vector3 scaledSize = Vector3.Scale(size, activeGamePiece.gameObject.transform.localScale);
            print("scaledSize: " + scaledSize);

            // Calculate a simple volume (or use another method based on your requirements)
            float volume = scaledSize.x * scaledSize.y * scaledSize.z;
            print("Volume: " + volume);

            // Assign weight based on the volume
            float weight = baseWeight + (volume * weightMultiplier);
            print("Weight: " + weight);

            return weight;
        } else {
            print($"MeshFilter on {activeGamePiece.gameObject.name} does not have a mesh assigned.");
            return 0f;
        }
    }

    // Call this when you, create the game piece, shrink or enlarge, or place the game piece
    private void AssignWeight() {
        activeGamePiece.weight = GetWeight();
    }
}
