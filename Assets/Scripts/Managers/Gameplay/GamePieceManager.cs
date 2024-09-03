using System;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Dorkbots.ServiceLocatorTools;
using UnityEngine;

public class GamePieceManager : MonoBehaviour, IGamePieceManager {
    // public List<GameObject> gamePieces = new List<GameObject>(); // Not sure if I even need this

    [SerializeField] private GameObject[] placeParticleEffectList;
    [SerializeField] private GameObject[] gemParticleEffectList;
    [SerializeField] private Canvas playingCanvas;

    public GamePieceObject activeGamePiece { get; set; }
    public float score { get; set; }
    public int curGems { get; set; }
    public float enlargeRemaining { get; set; }
    public float shrinkRemaining { get; set; }
    public float curEnlargeValue { get; set; }
    public float curShrinkValue { get; set; }
    public float pointsEarned { get; set; }
    public float curMaxChain { get; set; }
    public float tempEnlargeShrinkValue { get; set; }
    public bool activelyDestroying { get; set; }
    public float gamePieceArea { get; set; }

    private float weightMultiplier = 0.25f; // Multiplier to adjust weight scaling
    private float calculatedWeightMultiplier = 2.5f; // Multiplier to adjust weight scaling
    private float weightCap_Max = 750f; // Maximum weight value
    private float weightCap_Min = 50f; // Minimum weight value

    private float detectionRadius = 25f;  // Radius of the sphere cast
    private Vector3 testOrigin;  // Origin of the sphere cast

    private float changeSizeValue = 0.25f; // Value to change the size of the game piece by

    private List<GamePieceObject> curConnectedObjects = new List<GamePieceObject>();
    private List<GameObject> totGamePiecesOnBoard = new List<GameObject>();

    private bool startAddingPoints = false;
    private float placeParticleEffectDuration = 2.0f;

    private IPlayingCanvasManager playingCanvasManager;
    private IAudioManager audioManager;
    private IInventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start() {
        playingCanvasManager = ServiceLocator.Resolve<IPlayingCanvasManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();
        inventoryManager = ServiceLocator.Resolve<IInventoryManager>();
        InitRoundStats();
    }

    public void InitRoundStats() {
        // Reset score
        score = 0;
        pointsEarned = 0;
        curGems = 0;

        // Largest chain
        curMaxChain = 0;

        // Reset enlarge and shrink values
        enlargeRemaining = 50;
        shrinkRemaining = 50;
        tempEnlargeShrinkValue = 0;

        // Reset the connected objects
        curConnectedObjects.Clear();

        // Reset the active game piece
        if (activeGamePiece != null) {
            Destroy(activeGamePiece.gameObject);
        }
        activeGamePiece = null;

        // Reset the total game pieces on board
        foreach (GameObject gamePiece in totGamePiecesOnBoard) {
            Destroy(gamePiece);
        }
        totGamePiecesOnBoard.Clear();
    }

    // Update is called once per frame
    void Update() {
        if (activeGamePiece != null) {
            gamePieceArea = GetWeight(activeGamePiece.gameObject) / weightMultiplier;
        }
    }

    # region Place

    // Place
    public void Place() {
        if (activeGamePiece != null && activeGamePiece.canPlace && playingCanvasManager.countdownTimerIncrement > 0) {
            // Stop the timers coroutine
            playingCanvasManager.StopCountdown();

            HandlePlaceParticleEffect();
            AddGems();

            // Add to total game pieces on board
            totGamePiecesOnBoard.Add(activeGamePiece.gameObject);

            // Get the weight of the active game piece
            AssignWeight();

            // Reset the temporary enlarge and shrink values
            tempEnlargeShrinkValue = 0;

            // Add the weight of the active game piece to the score
            CalculateScore(activeGamePiece);

            activeGamePiece.isPlaced = true;

            // Reset the active game piece
            activeGamePiece.gameObject.GetComponent<MeshCollider>().isTrigger = false;
            Destroy(activeGamePiece.gameObject.GetComponent<Rigidbody>());

            activeGamePiece = null;
        }
    }

    # endregion

    # region gems

    private void AddGems() {
        int randomInt = UnityEngine.Random.Range(0, 100);

        if (randomInt < 100) {
            int randomGems = UnityEngine.Random.Range(1, 5);
            HandleGemsParticleEffect(randomGems);
            StartCoroutine(WaitToAddGems(randomGems, placeParticleEffectDuration / 2));
        }
    }

    private IEnumerator WaitToAddGems(int gemsEarned, float delay) {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        StartCoroutine(AddGemsOverTime(gemsEarned, placeParticleEffectDuration / 2));
    }

    private IEnumerator AddGemsOverTime(int gemsEarned, float duration) {
        float startGems = curGems;
        int targetGems = curGems + gemsEarned;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            curGems = (int)Mathf.Lerp(startGems, targetGems, elapsedTime / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set exactly to the target
        inventoryManager.gems = inventoryManager.gems + gemsEarned;
    }

    # endregion

    # region Particle Effects

    private void HandlePlaceParticleEffect() {
        for (int i = 0; i < placeParticleEffectList.Length; i++) {
            if (placeParticleEffectList[i].activeSelf == false) {
                placeParticleEffectList[i].SetActive(true);

                // Get particle image
                ParticleImage particleImage = placeParticleEffectList[i].GetComponent<ParticleImage>();
                Mesh mesh = activeGamePiece.GetComponent<MeshFilter>().sharedMesh;
                Vector3 size = mesh.bounds.size;
                Vector3 scaledSize = Vector3.Scale(size, activeGamePiece.transform.localScale);
                float area = scaledSize.x * scaledSize.z;
                particleImage.circleRadius = area / 10;
                particleImage.rateOverTime = GetWeight(activeGamePiece.gameObject) / 3;

                // Position the particle system at the object's position
                placeParticleEffectList[i].transform.localPosition = Utilities.GetCanvasPosition(activeGamePiece.transform.position, Camera.main, playingCanvas);

                StartCoroutine(WaitToAddPoints(placeParticleEffectDuration / 2));

                StartCoroutine(Utilities.DisableObjectAfterTime(placeParticleEffectList[i], placeParticleEffectDuration));

                break;
            }
        }
    }

    private void HandleGemsParticleEffect(int gemCount) {
        for (int i = 0; i < gemParticleEffectList.Length; i++) {
            if (gemParticleEffectList[i].activeSelf == false) {
                gemParticleEffectList[i].SetActive(true);

                // Get particle image
                ParticleImage particleImage = gemParticleEffectList[i].GetComponent<ParticleImage>();
                Mesh mesh = activeGamePiece.GetComponent<MeshFilter>().sharedMesh;
                Vector3 size = mesh.bounds.size;
                Vector3 scaledSize = Vector3.Scale(size, activeGamePiece.transform.localScale);
                float area = scaledSize.x * scaledSize.z;
                particleImage.circleRadius = area / 10;
                particleImage.rateOverTime = gemCount;

                // Position the particle system at the object's position
                gemParticleEffectList[i].transform.localPosition = Utilities.GetCanvasPosition(activeGamePiece.transform.position, Camera.main, playingCanvas);

                StartCoroutine(Utilities.DisableObjectAfterTime(gemParticleEffectList[i], placeParticleEffectDuration));

                break;
            }
        }
    }

    # endregion

    # region points / score

    private IEnumerator WaitToAddPoints(float delay) {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        startAddingPoints = true;
    }

    // Calculate score
    private void CalculateScore(GamePieceObject gamePiece) {
        // Add new
        pointsEarned = GetPointsEarned(gamePiece);
        if (pointsEarned > curMaxChain) {
            curMaxChain = pointsEarned;
        }

        StartCoroutine(ChangePointsOverTime(pointsEarned, placeParticleEffectDuration / 2));
    }

    public void subtractPoints(GamePieceObject gamePiece) {
        pointsEarned = GetPointsEarned(gamePiece);
        pointsEarned = pointsEarned * -1;
        print("Pints subtracted: " + pointsEarned);

        startAddingPoints = true;

        StartCoroutine(ChangePointsOverTime(pointsEarned, placeParticleEffectDuration / 2));
    }

    private float GetPointsEarned(GamePieceObject gamePiece) {
        // Need to add self to the list of connected objects because it starts the list
        curConnectedObjects.Add(gamePiece);

        DetectNearbyObjects(gamePiece);

        // Calculate the score based on the number of connected objects
        float tempRunningWeight = 0;
        float tempSubtraction = 0;
        for (int i = 0; i < curConnectedObjects.Count; i++) {
            // print("Connected object: " + curConnectedObjects[i].name);
            // print("Connected object weight: " + curConnectedObjects[i].weight);
            tempRunningWeight += curConnectedObjects[i].weight;
        }

        // Subtract previous chain or weights/objects from the score to not double count
        // Do this by repeating but leave out the active game piece
        for (int i = 1; i < curConnectedObjects.Count; i++) {
            // print("Connected object: " + curConnectedObjects[i].name);
            // print("Connected object weight: " + curConnectedObjects[i].weight);
            tempSubtraction += curConnectedObjects[i].weight;
        }

        float addedWeight = curConnectedObjects.Count * tempRunningWeight * calculatedWeightMultiplier;
        float subtractedWeight = (curConnectedObjects.Count - 1) * tempSubtraction * calculatedWeightMultiplier;

        // Add new
        pointsEarned = addedWeight - subtractedWeight;

        // Reset curConnectedObjects
        curConnectedObjects.Clear();

        return pointsEarned;
    }

    private IEnumerator ChangePointsOverTime(float pointsEarned, float duration) {
        float startPoints = score;
        float targetPoints = score + pointsEarned;
        print("StartPoints: " + startPoints);
        print("Target Points: " + targetPoints);
        float elapsedTime = 0f;

        while (startAddingPoints != true) {
            yield return null;
        }

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            score = Mathf.Lerp(startPoints, targetPoints, elapsedTime / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set exactly to the target
        score = targetPoints;
        startAddingPoints = false;
    }

    # endregion

    # region Detect Nearby Objects

    void DetectNearbyObjects(GamePieceObject gamePiece) {
        Mesh mesh = gamePiece.gameObject.GetComponent<MeshFilter>().sharedMesh;
        Vector3 size = mesh.bounds.size;
        Vector3 scaledSize = Vector3.Scale(size, activeGamePiece.gameObject.transform.localScale);

        detectionRadius = scaledSize.x * 1f;

        testOrigin = gamePiece.transform.position; // This is for my gizmo

        // Find all colliders within the detection radius centered on the object's position
        Collider[] hitColliders = Physics.OverlapSphere(gamePiece.transform.position, detectionRadius);

        foreach (Collider collider in hitColliders) {
            GameObject nearbyObject = collider.gameObject;

            // Ignore self
            if (nearbyObject != gamePiece.gameObject && nearbyObject.tag == "GamePiece") {
                // Debug.Log("Nearby object detected: " + nearbyObject.name);
                if (nearbyObject.GetComponent<GamePieceObject>().category == gamePiece.category) {
                    // Debug.Log("Same category detected: " + nearbyObject.name);

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

    # endregion

    # region Enlarge/Shrink

    // Enlarge
    public void Enlarge() {
        if (activeGamePiece != null) {
            if (enlargeRemaining - 1 >= 0 && GetWeight(activeGamePiece.gameObject) < weightCap_Max) {
                audioManager.PlaySFX("UIClick_Scale");
                // Enlarge the active game piece
                activeGamePiece.transform.localScale += new Vector3(changeSizeValue, changeSizeValue, changeSizeValue);

                tempEnlargeShrinkValue += 1;

                if (tempEnlargeShrinkValue > 0) {
                    enlargeRemaining = curEnlargeValue - Math.Abs(tempEnlargeShrinkValue);
                } else {
                    // if (curShrinkValue - Math.Abs(tempEnlargeShrinkValue))
                    shrinkRemaining = curShrinkValue - Math.Abs(tempEnlargeShrinkValue);
                }
            } else {
                audioManager.PlaySFX("UIClick_Error");
            }
        }
    }

    // Shrink
    public void Shrink() {
        if (activeGamePiece != null) {
            if (shrinkRemaining - 1 >= 0 && GetWeight(activeGamePiece.gameObject) > weightCap_Min) {
                audioManager.PlaySFX("UIClick_Scale");
                // Shrink the active game piece
                activeGamePiece.transform.localScale -= new Vector3(changeSizeValue, changeSizeValue, changeSizeValue);

                tempEnlargeShrinkValue -= 1;

                if (tempEnlargeShrinkValue < 0) {
                    shrinkRemaining = curShrinkValue - Math.Abs(tempEnlargeShrinkValue);
                } else {
                    enlargeRemaining = curEnlargeValue - Math.Abs(tempEnlargeShrinkValue);
                }
            } else {
                audioManager.PlaySFX("UIClick_Error");
            }
        }
    }

    # endregion

    # region Weight

    // Get Weight
    public float GetWeight(GameObject localGameObject) {
        Mesh mesh = localGameObject.GetComponent<MeshFilter>().sharedMesh;

        if (mesh != null) {
            // Calculate the size of the mesh using its bounds
            Vector3 size = mesh.bounds.size;
            Vector3 scaledSize = Vector3.Scale(size, localGameObject.transform.localScale);
            // print("scaledSize: " + scaledSize);

            // Calculate a simple volume (or use another method based on your requirements)
            // float volume = scaledSize.x * scaledSize.y * scaledSize.z;
            float area = scaledSize.x * scaledSize.z;
            // print("Volume: " + volume);

            // Assign weight based on the volume
            float weight = area * weightMultiplier;
            // print("Weight: " + weight);

            return weight;
        } else {
            print($"MeshFilter on {localGameObject.name} does not have a mesh assigned.");
            return 0f;
        }
    }

    // Call this when you, create the game piece, shrink or enlarge, or place the game piece
    private void AssignWeight() {
        activeGamePiece.weight = GetWeight(activeGamePiece.gameObject);
    }

    # endregion
}
