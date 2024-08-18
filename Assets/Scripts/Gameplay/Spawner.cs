using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using Unity.VisualScripting;

public class Spawner : MonoBehaviour
{

    private ISpawnManager spawnManager;
    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = ServiceLocator.Resolve<ISpawnManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call from SpawnManager
    public void Spawn() {
        int randomCategoryInt = Random.Range(0, 8);
        
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
        GameObject objectToSpawn = objectsInCategory[randomObjectInt];
        GameObject instatiatedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        // Init the object with proper components and values
        instatiatedObject.AddComponent<GamePieceObject>();
        instatiatedObject.GetComponent<GamePieceObject>().category = objectToSpawn.tag;
        instatiatedObject.GetComponent<GamePieceObject>().weight = 1;
        instatiatedObject.GetComponent<GamePieceObject>().isPlaced = false;

        // Set the active game piece
       gamePieceManager.activeGamePiece = instatiatedObject.GetComponent<GamePieceObject>();
    }
}
