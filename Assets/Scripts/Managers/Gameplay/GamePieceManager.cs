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
    private float calculatedWeightMultiplier = 1f; // Multiplier to adjust weight scaling
    private float weightCap_Max = 7500f; // Maximum weight value
    private float weightCap_Min = 10f; // Minimum weight value

    private float detectionRadius = 25f;  // Radius of the sphere cast
    private Vector3 testOrigin;  // Origin of the sphere cast

    private float changeSizeValue = 0.1f; // Value to change the size of the game piece by
    private float tempEnlargeRemaining;
    private float tempShrinkRemaining;

    private List<GamePieceObject> curConnectedObjects = new List<GamePieceObject>();


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
            CalculateScore(activeGamePiece);

            activeGamePiece.isPlaced = true;
            activeGamePiece = null;
        }
    }

    // Calculate score
    private void CalculateScore(GamePieceObject gamePiece) {
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

            gamePiece.gameObject.transform.GetChild then get new collider object and check for collisions 
            if collision, check if it's the same category
        */

        // Need to add self to the list of connected objects because it starts the list
        curConnectedObjects.Add(gamePiece);

        DetectNearbyObjects(gamePiece);

        // Calculate the score based on the number of connected objects
        float tempRunningWeight = 0;
        for (int i = 0; i < curConnectedObjects.Count; i++) {
            print("Connected object: " + curConnectedObjects[i].name);
            print("Connected object weight: " + curConnectedObjects[i].weight);
            tempRunningWeight += curConnectedObjects[i].weight;
        }
        score += curConnectedObjects.Count * tempRunningWeight * calculatedWeightMultiplier;

        // Reset curConnectedObjects
        curConnectedObjects.Clear();
    }

    void DetectNearbyObjects(GamePieceObject gamePiece) {
        Mesh mesh = gamePiece.gameObject.GetComponent<MeshFilter>().sharedMesh;
        Vector3 size = mesh.bounds.size;
        Vector3 scaledSize = Vector3.Scale(size, activeGamePiece.gameObject.transform.localScale);

        detectionRadius = scaledSize.x * 1f;

        testOrigin = gamePiece.transform.position; // This is for my gizmo

        // Find all colliders within the detection radius centered on the object's position
        Collider[] hitColliders = Physics.OverlapSphere(gamePiece.transform.position, detectionRadius);

        print("Detected nearby objects called");

        // // Check if the object has already been checked and skip it if it has
        // // Don't need to check the first object because it's the active game piece
        // if (curConnectedObjects.Contains(gamePiece) && gamePiece != activeGamePiece) {
        //     print("Already checked this object");
        //     return;
        // }

        foreach (Collider collider in hitColliders) {
            GameObject nearbyObject = collider.gameObject;

            // Ignore self
            if (nearbyObject != gamePiece.gameObject && nearbyObject.tag == "GamePiece") {
                Debug.Log("Nearby object detected: " + nearbyObject.name);
                if (nearbyObject.GetComponent<GamePieceObject>().category == gamePiece.category) {
                    Debug.Log("Same category detected: " + nearbyObject.name);

                    if (!curConnectedObjects.Contains(nearbyObject.GetComponent<GamePieceObject>())) {
                        // Add the nearby object to the list of connected objects
                        curConnectedObjects.Add(nearbyObject.GetComponent<GamePieceObject>());

                        // Recurse on the nearby object
                        DetectNearbyObjects(nearbyObject.GetComponent<GamePieceObject>());
                    }
                }
            }
        }
    }

    // Draw Gizmos to visualize the SphereCast in the editor
    void OnDrawGizmos() {
        // Set the color of the gizmo
        Gizmos.color = Color.yellow;

        // Draw the sphere at the start point
        Gizmos.DrawWireSphere(testOrigin, detectionRadius);
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
            // print("scaledSize: " + scaledSize);

            // Calculate a simple volume (or use another method based on your requirements)
            float volume = scaledSize.x * scaledSize.y * scaledSize.z;
            // print("Volume: " + volume);

            // Assign weight based on the volume
            float weight = baseWeight + (volume * weightMultiplier);
            // print("Weight: " + weight);

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
