using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameManager {
    GameState State {get; set;}
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
