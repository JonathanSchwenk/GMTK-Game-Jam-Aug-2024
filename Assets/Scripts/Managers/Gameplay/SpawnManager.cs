using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class SpawnManager : MonoBehaviour, ISpawnManager
{

    public ObjectsDatabase objectsDatabase {get; set;}

    [SerializeField] private ObjectsDatabase objectsDatabaseLocal;
    [SerializeField] private Spawner spawner;

    private IGamePieceManager gamePieceManager;
    private IGameManager gameManager;

    // TODO: Get gameManager here and only start spawning if the game state is playing
    // But for now start right away

    // Start is called before the first frame update
    void Start()
    {
        objectsDatabase = objectsDatabaseLocal;

        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePieceManager.activeGamePiece == null && gameManager.gameState == GameState.Playing) {
            spawner.Spawn();
        }
    }
}
