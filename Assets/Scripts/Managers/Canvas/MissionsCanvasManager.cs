using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class MissionsCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject dailyMissionsScrollContent;
    [SerializeField] private GameObject allTimeMissionsScrollContent;
    [SerializeField] private GameObject missionPrefab;


    private IMenuCanvasManager menuCanvasManager;
    private IMissionManager missionManager;

    // Start is called before the first frame update
    void Start()
    {
        menuCanvasManager = ServiceLocator.Resolve<IMenuCanvasManager>();
        missionManager = ServiceLocator.Resolve<IMissionManager>();

        SpawnDailyMissions();
        SpawnAllTimeMissions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn from MissionManager
    private void SpawnDailyMissions() {
        missionManager.SpawnDailyMissions(missionPrefab, dailyMissionsScrollContent);
    }
    private void SpawnAllTimeMissions() {
        missionManager.SpawnAllTimeMissions(missionPrefab, dailyMissionsScrollContent);
    }

    // Button for submitting a mission

    public void OnBackButtonPressed() {
        // Close the shop
        menuCanvasManager.UpdateCanvasState(MenuCanvasState.Start);
    }
}
