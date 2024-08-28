using UnityEngine;
using System;
using Dorkbots.ServiceLocatorTools;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IGameManager
{
    public GameState gameState {get; set;}
    public Action<GameState> OnGameStateChanged {get; set;}


    private ISaveManager saveManager;
    private IAudioManager audioManager;
    private IPlayingCanvasManager playingCanvasManager;
    private IGamePieceManager gamePieceManager;
    private IInventoryManager inventoryManager;



    void OnDestroy() {
        OnGameStateChanged = null;
    }

    // Sets the state to ready when the game starts 
    void Start() {
        saveManager = ServiceLocator.Resolve<ISaveManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();
        playingCanvasManager = ServiceLocator.Resolve<IPlayingCanvasManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        inventoryManager = ServiceLocator.Resolve<IInventoryManager>();

        UpdateGameState(GameState.StartMenu);
    }

    void Update() {
        // To Exit the game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    // For next game, control more with this game managers state machine to keep everything in one spot
    // Update game state function
    public void UpdateGameState(GameState newState) {
        gameState = newState;

        // Switch statement that deals with each possible state 
        switch(newState) {
            case GameState.StartMenu:
                
                break;
            case GameState.Tutorial:

                break;
            case GameState.Idle:
                
                break;
            case GameState.Playing:
                
                break;
            case GameState.GameOver:
                playingCanvasManager.StopCountdown();
                // Score
                if (gamePieceManager.score > saveManager.saveData.highScore) {
                    saveManager.saveData.highScore = gamePieceManager.score;
                }

                // Chain
                if (gamePieceManager.curMaxChain > saveManager.saveData.highChain) {
                    saveManager.saveData.highChain = gamePieceManager.curMaxChain;
                }

                // Gems
                saveManager.saveData.gems = inventoryManager.gems;

                // Inventory

                saveManager.Save();

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        // Null checker then calls the action for anthing subscribed to it
        OnGameStateChanged?.Invoke(newState);
    } 


}




// GameState enum (basically a definition)
public enum GameState {
    StartMenu,
    Tutorial,
    Idle,
    Playing,
    GameOver
}


