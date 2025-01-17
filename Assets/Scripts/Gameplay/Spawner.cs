using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using Unity.VisualScripting;

public class Spawner : MonoBehaviour
{

    private ISpawnManager spawnManager;
    private IGamePieceManager gamePieceManager;
    private IPlayingCanvasManager playingCanvasManager;
    private IGameManager gameManager;

    private float targetWidthHeight = 30f;
    private float timerCountdown = 15f;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = ServiceLocator.Resolve<ISpawnManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        playingCanvasManager = ServiceLocator.Resolve<IPlayingCanvasManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call from SpawnManager
    public void Spawn() {
        // For properly displaying the remaining enlarge and shrink values
        gamePieceManager.curEnlargeValue = gamePieceManager.enlargeRemaining;
        gamePieceManager.curShrinkValue = gamePieceManager.shrinkRemaining;

        // Starts the countdown for the game piece
        if (gameManager.gameState == GameState.Playing) {
            playingCanvasManager.StartCountdown(timerCountdown);
        }
        

        int randomCategoryInt = Random.Range(0, 8);
        // SpawnObject(spawnManager.objectsDatabase.scifiObjects);
        
        switch(randomCategoryInt) {
            case 0:
                SpawnObject(spawnManager.objectsDatabase.cityObjects);
                break;
            case 1:
                SpawnObject(spawnManager.objectsDatabase.egyptObjects);
                break;
            case 2:
                SpawnObject(spawnManager.objectsDatabase.japanObjects);
                break;
            case 3:
                SpawnObject(spawnManager.objectsDatabase.medievalObjects);
                break;
            case 4:
                SpawnObject(spawnManager.objectsDatabase.neighborhoodObjects);
                break;
            case 5:
                SpawnObject(spawnManager.objectsDatabase.pirateObjects);
                break;
            case 6:
                SpawnObject(spawnManager.objectsDatabase.scifiObjects);
                break;
            case 7:
                SpawnObject(spawnManager.objectsDatabase.westernObjects);
                break;
            default:
                break;
        }
    }

    private void SpawnObject(GameObject[] objectsInCategory) {
        int randomObjectInt = Random.Range(0, objectsInCategory.Length);
        // int randomObjectInt = 3;
        GameObject objectToSpawn = objectsInCategory[randomObjectInt];
        GameObject instatiatedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        string[] splitName = objectToSpawn.name.Split(' ');
        string category = splitName[0];
        string name = string.Join(" ", splitName[1..]);

        // Init the object with proper components and values
        instatiatedObject.AddComponent<GamePieceObject>();
        instatiatedObject.GetComponent<GamePieceObject>().category = category;
        instatiatedObject.GetComponent<GamePieceObject>().gamePieceName = name;
        instatiatedObject.GetComponent<GamePieceObject>().weight = 1;
        instatiatedObject.GetComponent<GamePieceObject>().isPlaced = false;

        instatiatedObject.tag = "GamePiece";

        instatiatedObject.AddComponent<Rigidbody>();
        instatiatedObject.GetComponent<Rigidbody>().useGravity = false;
        instatiatedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

        instatiatedObject.GetComponent<MeshCollider>().convex = true;
        instatiatedObject.GetComponent<MeshCollider>().isTrigger = true;

        // Sets the scale so all objects are around same size, do this based off the weight of the object
        Vector3 size = instatiatedObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        float scaleMultiplier = targetWidthHeight / ((size.x + size.z) / 2);

        instatiatedObject.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

        // Set the active game piece
       gamePieceManager.activeGamePiece = instatiatedObject.GetComponent<GamePieceObject>();
    }
}
