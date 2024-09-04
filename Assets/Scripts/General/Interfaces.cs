using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IGameManager {
    GameState gameState { get; set; }
    Action<GameState> OnGameStateChanged { get; set; }
    void UpdateGameState(GameState state);
    EventSystem eventSystem { get; set; }
}

public interface IObjectPooler {
    List<Pool> Pools { get; set; }
    Dictionary<string, Queue<GameObject>> poolDictionary { get; set; }

    GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation);
}

public interface ISaveManager {
    SaveData saveData { get; set; }
    Action<int> OnSave { get; set; }

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
    GamePieceObject activeGamePiece { get; set; }
    float score { get; set; }
    int curGems { get; set; }
    float enlargeRemaining { get; set; }
    float shrinkRemaining { get; set; }
    float curEnlargeValue { get; set; }
    float curShrinkValue { get; set; }
    float pointsEarned { get; set; }
    float curMaxChain { get; set; }
    float tempEnlargeShrinkValue { get; set; }
    bool activelyDestroying { get; set; }
    float gamePieceArea { get; set; }

    void InitRoundStats();
    void Place();
    void subtractPoints(GamePieceObject gamePiece);
    void Enlarge();
    void Shrink();
    float GetWeight(GameObject localGameObject);
}

public interface ISpawnManager {
    ObjectsDatabase objectsDatabase { get; set; }
}

public interface IPlayingCanvasManager {
    void StartCountdown(float duration);
    void StopCountdown();
    float countdownTimer { get; set; }
    float countdownTimerIncrement { get; set; }
    void ShowTappedPieceDescription(GameObject obj, string category, string name);
}

public interface IInventoryManager {
    int gems { get; set; }
    int extraTime { get; set; }
    int skips { get; set; }
    int destroys { get; set; }
    int extraEnlarges { get; set; }
    int extraShrinks { get; set; }
    void SaveInventory();
}

public interface ICanvasManager {
    GraphicRaycaster graphicRaycaster { get; set; }
}

public interface IShopCanvasManager {

}

public interface IMissionCanvasManager {

}

public interface IMenuCanvasManager {
    MenuCanvasState menuCanvasState { get; set; }
    Action<MenuCanvasState> OnMenuCanvasStateChanged { get; set; }
    void UpdateCanvasState(MenuCanvasState newState);
}

public interface IPowerupManager {
    GameObject globalOgPowerupButton { get; }
}

public interface IMissionManager {
    List<MissionData> dailyMissions { get; set; }
    List<MissionData> allTimeMissions { get; set; }
    void SpawnDailyMissions(GameObject missionPrefab, GameObject scrollContent);
    void SpawnAllTimeMissions(
        GameObject missionPrefab,
        GameObject titlePrefab,
        GameObject chainTitle,
        GameObject chainContent,
        GameObject areaTitle,
        GameObject areaContent,
        GameObject gamePieceTitle,
        GameObject gamePieceContent
    );
}