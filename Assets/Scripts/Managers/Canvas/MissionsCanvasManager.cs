using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class MissionsCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject dailyMissionsScrollContent;
    [SerializeField] private GameObject chainTitle_AllTimeMissions;
    [SerializeField] private GameObject chainContent_AllTimeMissions;
    [SerializeField] private GameObject areaTitle_AllTimeMissions;
    [SerializeField] private GameObject areaContent_AllTimeMissions;
    [SerializeField] private GameObject gamePieceTitle_AllTimeMissions;
    [SerializeField] private GameObject gamePieceContent_AllTimeMissions;
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private GameObject titlePrefab;
    [SerializeField] private GameObject daily;
    [SerializeField] private GameObject dailyOnFill;
    [SerializeField] private GameObject dailyOffFill;
    [SerializeField] private GameObject allTime;
    [SerializeField] private GameObject allTimeOnFill;
    [SerializeField] private GameObject allTimeOffFill;


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
        missionManager.SpawnAllTimeMissions(
            titlePrefab, 
            missionPrefab, 
            chainTitle_AllTimeMissions, 
            chainContent_AllTimeMissions, 
            areaTitle_AllTimeMissions, 
            areaContent_AllTimeMissions, 
            gamePieceTitle_AllTimeMissions, 
            gamePieceContent_AllTimeMissions
        );
    }

    // Button for submitting a mission, this is done in the Mission game object

    public void OnBackButtonPressed() {
        // Close the shop
        menuCanvasManager.UpdateCanvasState(MenuCanvasState.Start);
    }

    public void toDailyMissions() {
        daily.SetActive(true);
        allTime.SetActive(false);

        dailyOnFill.SetActive(true);
        dailyOffFill.SetActive(false);
        allTimeOnFill.SetActive(false);
        allTimeOffFill.SetActive(true);
    }

    public void toAllTimeMissions() {
        daily.SetActive(false);
        allTime.SetActive(true);

        dailyOnFill.SetActive(false);
        dailyOffFill.SetActive(true);
        allTimeOnFill.SetActive(true);
        allTimeOffFill.SetActive(false);
    }
}
