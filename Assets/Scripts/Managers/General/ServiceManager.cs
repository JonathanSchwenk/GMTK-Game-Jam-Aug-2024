using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class ServiceManager : MonoBehaviour
{
    
    public SaveManager saveManager;
    public AudioManager audioManager;
    public GameManager gameManager;
    public GamePieceManager gamePieceManager;
    public SpawnManager spawnManager;
    public InventoryManager inventoryManager;
    public UsePowerupManager powerupManager;
    public MenuCanvasManager menuCanvasManager;
    public PlayingCanvasManager playingCanvasManager;
    public CanvasManager canvasManager;
    public MissionManager missionManager;

    


    private void Awake() {
        // If there is no SaveManager service registered, create one, else, do nothing
        if (ServiceLocator.IsRegistered<ISaveManager>()) {
            //Debug.Log("A SaveManager already exists");
            // Loading save here because this script gets executed early which is where I need to load so Im trying here.
            saveManager.Load();
        } else {
            //Debug.Log("SaveManager not found, creating one");
            ServiceLocator.Register<ISaveManager>(saveManager);

            // Loading save here because this script gets executed early which is where I need to load so Im trying here.
            saveManager.Load();
        }

        // If there is no AudioManager service registered, create one, else, do nothing
        if (ServiceLocator.IsRegistered<IAudioManager>()) {
            //Debug.Log("An AudioManager already exists");
        } else {
            //Debug.Log("AudioManager not found, creating one");
            ServiceLocator.Register<IAudioManager>(audioManager);
        }

        if (!ServiceLocator.IsRegistered<IGameManager>()) {
            ServiceLocator.Register<IGameManager>(gameManager);
        }
        if (!ServiceLocator.IsRegistered<IGamePieceManager>()) {
            ServiceLocator.Register<IGamePieceManager>(gamePieceManager);
        }
        if (!ServiceLocator.IsRegistered<ISpawnManager>()) {
            ServiceLocator.Register<ISpawnManager>(spawnManager);
        }
        if (!ServiceLocator.IsRegistered<IPlayingCanvasManager>()) {
            ServiceLocator.Register<IPlayingCanvasManager>(playingCanvasManager);
        }
        if (!ServiceLocator.IsRegistered<IInventoryManager>()) {
            ServiceLocator.Register<IInventoryManager>(inventoryManager);
        }
        if (!ServiceLocator.IsRegistered<IMenuCanvasManager>()) {
            ServiceLocator.Register<IMenuCanvasManager>(menuCanvasManager);
        }
        if (!ServiceLocator.IsRegistered<IPowerupManager>()) {
            ServiceLocator.Register<IPowerupManager>(powerupManager);
        }
        if (!ServiceLocator.IsRegistered<ICanvasManager>()) {
            ServiceLocator.Register<ICanvasManager>(canvasManager);
        }
        if (!ServiceLocator.IsRegistered<IMissionManager>()) {
            ServiceLocator.Register<IMissionManager>(missionManager);
        }

    }


    private void OnDestroy() {

    }

    private void OnApplicationQuit() {
        if (ServiceLocator.IsRegistered<ISaveManager>()) {
            ServiceLocator.Unregister<ISaveManager>();
        }
        if (ServiceLocator.IsRegistered<IAudioManager>()) {
            ServiceLocator.Unregister<IAudioManager>();
        }
        if (ServiceLocator.IsRegistered<IGameManager>()) {
            ServiceLocator.Unregister<IGameManager>();
        }
        if (ServiceLocator.IsRegistered<IGamePieceManager>()) {
            ServiceLocator.Unregister<IGamePieceManager>();
        }
        if (ServiceLocator.IsRegistered<ISpawnManager>()) {
            ServiceLocator.Unregister<ISpawnManager>();
        }
        if (ServiceLocator.IsRegistered<IPlayingCanvasManager>()) {
            ServiceLocator.Unregister<IPlayingCanvasManager>();
        }
        if (ServiceLocator.IsRegistered<IInventoryManager>()) {
            ServiceLocator.Unregister<IInventoryManager>();
        }
        if (ServiceLocator.IsRegistered<IMenuCanvasManager>()) {
            ServiceLocator.Unregister<IMenuCanvasManager>();
        }
        if (ServiceLocator.IsRegistered<IPowerupManager>()) {
            ServiceLocator.Unregister<IPowerupManager>();
        }
        if (ServiceLocator.IsRegistered<ICanvasManager>()) {
            ServiceLocator.Unregister<ICanvasManager>();
        }
        if (ServiceLocator.IsRegistered<IMissionManager>()) {
            ServiceLocator.Unregister<IMissionManager>();
        }
    }
    

}

