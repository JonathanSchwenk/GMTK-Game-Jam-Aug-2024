using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class ServiceManager : MonoBehaviour
{
    
    public SaveManager saveManager;
    public AudioManager audioManager;
    public GameManager gameManager;
    


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
    }
    

}

