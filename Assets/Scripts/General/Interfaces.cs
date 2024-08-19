using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameManager {
    GameState gameState {get; set;}
    Action<GameState> OnGameStateChanged {get; set;}
    void UpdateGameState(GameState state);
}

public interface IObjectPooler {
    List<Pool> Pools {get; set;}
    Dictionary<string, Queue<GameObject>> poolDictionary {get; set;}

    GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation);
}

public interface ISaveManager {
    SaveData saveData {get; set;}
    Action<int>OnSave {get; set;}

    void Save();
    void Load();
    void DeleteSavedData();
}

public interface IAudioManager {
    void PlaySFX(string name);
    void StopSFX(string name);
    void PlayMusic(string name);
    void StopMusic(string name);
}


public interface IAdManager {
    void LoadRewardedAd();
}

public interface IGamePieceManager {
    GamePieceObject activeGamePiece {get; set;}
    float score {get; set;}
    float enlargeRemaining {get; set;}
    float shrinkRemaining {get; set;}
    float curEnlargeValue {get; set;}
    float curShrinkValue {get; set;}
    float pointsEarned { get; set; }
    float curMaxChain { get; set; }
    float tempEnlargeShrinkValue { get; set; }

    void InitRoundStats();
    void Place();
    void Enlarge();
    void Shrink();
    float GetWeight(GameObject localGameObject);
}

public interface ISpawnManager {
    ObjectsDatabase objectsDatabase {get; set;}
}

public interface IPlayingCanvasManager {
    void StartCountdown(float duration);
    void StopCountdown();
    float countdownTimer {get; set;}
}
