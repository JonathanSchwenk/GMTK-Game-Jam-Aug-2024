using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using UnityEngine.PlayerLoop;
using System.Linq;
using TMPro;

public class MissionManager : MonoBehaviour, IMissionManager {

    private string[] categoriesList = new string[] { "City", "Egypt", "Japan", "Medieval", "Neighborhood", "Pirate", "Scifi", "Western" };

    // These have to be saved for when players come back to a mission
    public List<MissionData> dailyMissions { get; set; }
    public List<MissionData> allTimeMissions { get; set; }

    // In order to update properly without having to check every frame I am going to save public values that gets reset each game
    // Don't need score because that is already saved in GamePieceManager

    // In the game as you place objects and make chains, I will update these values

    // This will be a list of all the chains that have been made in the game, split by category
    public TotalChains totalChains { get; set; }
    // List of every objects area, separated by category
    public TotalAreas totalAreas { get; set; }
    // List of every object in the game, separated by category
    public TotalGamePieces totalGamePieces { get; set; }


    private ISaveManager saveManager;


    // Start is called before the first frame update
    void Start() {
        saveManager = ServiceLocator.Resolve<ISaveManager>();

        if (saveManager.saveData.dailyMissions != null) {
            dailyMissions = saveManager.saveData.dailyMissions;
        } else {
            dailyMissions = new List<MissionData>();
        }
        if (saveManager.saveData.allTimeMissions != null) {
            allTimeMissions = saveManager.saveData.allTimeMissions;
        } else {
            allTimeMissions = new List<MissionData>();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    // Take in prefab and location to spawn
    public void SpawnDailyMissions(GameObject missionPrefab, GameObject scrollContent) {
        // Check if its a new day, if it is or if its null, then make new missions, else load the old missions
        DateTime now = DateTime.Now;
        DateTime lastTime = new DateTime(saveManager.saveData.lastActive);
        // DateTime lastTime = new DateTime(2021, 1, 1);
        print("Spawn Daily Missions");

        if (now.Day != lastTime.Day || saveManager.saveData.dailyMissions == null) {
            // Make new missions
            for (int i = 0; i < 10; i++) {
                // Pick a random mission type
                int randomMissionInt = UnityEngine.Random.Range(0, 3);
                if (randomMissionInt == 0) {
                    SpawnNewDaily(missionPrefab, scrollContent, "Chain", i);
                } else if (randomMissionInt == 1) {
                    SpawnNewDaily(missionPrefab, scrollContent, "Area", i);
                } else {
                    SpawnNewDaily(missionPrefab, scrollContent, "GamePiece", i);
                }
            }
            // Save the new missions
            saveManager.saveData.dailyMissions = new List<MissionData>(dailyMissions);
            saveManager.Save();

        } else {
            // Load the old missions
            SpawnExistingMissions(saveManager.saveData.dailyMissions, missionPrefab, scrollContent);
        }
    }

    private void SpawnNewDaily(GameObject missionPrefab, GameObject scrollContent, string missionType, int index) {
        GameObject mission = Instantiate(missionPrefab, scrollContent.transform);
        mission.GetComponent<MissionGameObject>().missionData = new MissionData {
            // Set mission ID
            missionID = index,

            // Set mission type
            missionType = missionType
        };

        // Set categories
        int randomCategories = UnityEngine.Random.Range(1, 6);
        string[] categories = GenerateRandomCategories(randomCategories);
        if (randomCategories == 5) {
            mission.GetComponent<MissionGameObject>().missionData.categories = new string[] { "All" };
        } else {
            mission.GetComponent<MissionGameObject>().missionData.categories = categories;
        }

        // Sets status
        mission.GetComponent<MissionGameObject>().missionData.status = "Incomplete";

        // Sets mission target value
        float targetValue = 0;
        if (missionType == "Chain") {
            int randomChain = UnityEngine.Random.Range(5, 11);
            targetValue = randomChain * 1000;
            mission.GetComponent<MissionGameObject>().missionData.targetValue = targetValue;
        } else if (missionType == "Area") {
            int randomArea = UnityEngine.Random.Range(5, 11); // 1000 area might be too much
            targetValue = randomArea * 100;
            mission.GetComponent<MissionGameObject>().missionData.targetValue = targetValue;
        } else {
            int randomGamePiece = UnityEngine.Random.Range(10, 21);
            targetValue = randomGamePiece * 10;
            mission.GetComponent<MissionGameObject>().missionData.targetValue = targetValue;
        }

        // Set progress value
        mission.GetComponent<MissionGameObject>().missionData.progressValue = 0;

        // Set goal value
        int randomGoalInt = UnityEngine.Random.Range(1, 6);
        if (missionType == "GamePiece") {
            mission.GetComponent<MissionGameObject>().missionData.goalValue = (int)targetValue;
        } else if (missionType == "Area") {
            mission.GetComponent<MissionGameObject>().missionData.goalValue = randomGoalInt + 5;
        } else {
            mission.GetComponent<MissionGameObject>().missionData.goalValue = randomGoalInt;
        }

        // Set mission description
        if (missionType == "Chain") {
            mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Make " + randomGoalInt + " chains equal to " + targetValue + " points or more";
        } else if (missionType == "Area") {
            mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Place " + randomGoalInt + " pieces with and area of " + targetValue + " or more";
        } else {
            mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Place " + (int)targetValue + " game pieces";
        }

        // Set reward
        if (missionType == "Chain") {
            mission.GetComponent<MissionGameObject>().missionData.reward = (int)((targetValue / 100) + (10 * randomGoalInt));
        } else if (missionType == "Area") {
            mission.GetComponent<MissionGameObject>().missionData.reward = (int)((targetValue / 10) + (10 * randomGoalInt));
        } else {
            mission.GetComponent<MissionGameObject>().missionData.reward = (int)(targetValue / 10) + 25;
        }

        dailyMissions.Add(mission.GetComponent<MissionGameObject>().missionData);
    }

    private void SpawnExistingMissions(List<MissionData> missions, GameObject missionPrefab, GameObject scrollContent) {
        for (int i = 0; i < missions.Count; i++) {
            GameObject mission = Instantiate(missionPrefab, scrollContent.transform);
            mission.GetComponent<MissionGameObject>().missionData = missions[i];
        }
    }

    // Spawn All Time Missions
    public void SpawnAllTimeMissions(
            GameObject titlePrefab,
            GameObject missionPrefab,
            GameObject chainTitle,
            GameObject chainContent,
            GameObject areaTitle,
            GameObject areaContent,
            GameObject gamePieceTitle,
            GameObject gamePieceContent
        ) {
        if (saveManager.saveData.allTimeMissions.Count <= 0) {
            // Chain
            GameObject titleChain = Instantiate(titlePrefab, chainTitle.transform);
            titleChain.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chain Missions";
            SpawnNewAllTime(missionPrefab, chainContent, AllTimeMissionFile.chainAllTimeSpawnData, 100, "Chain");
            // Area
            GameObject titleArea = Instantiate(titlePrefab, areaTitle.transform);
            titleArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Area Missions";
            SpawnNewAllTime(missionPrefab, areaContent, AllTimeMissionFile.areaAllTimeSpawnData, 200, "Area");
            // Total Game Pieces
            GameObject titleGamePiece = Instantiate(titlePrefab, gamePieceTitle.transform);
            titleGamePiece.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Total Game Piece Missions";
            SpawnNewAllTime(missionPrefab, gamePieceContent, AllTimeMissionFile.gamePieceAllTimeSpawnData, 300, "GamePiece");
        }
    }

    private void SpawnNewAllTime(GameObject missionPrefab, GameObject scrollContent, List<AllTimeMissionSpawnData> allTimeMissionSpawnData, int startingIndex, string missionType) {
        for (int i = 0; i < 24; i++) { // 24 is the amount of missions there are for a missionType
            GameObject mission = Instantiate(missionPrefab, scrollContent.transform);
            mission.GetComponent<MissionGameObject>().missionData = new MissionData {
                // Set mission ID
                missionID = startingIndex + i,

                // Set mission type
                missionType = missionType,

                // Set categories
                categories = allTimeMissionSpawnData[i].categories,

                // Sets status
                status = "Incomplete",

                // Sets mission target value
                targetValue = allTimeMissionSpawnData[i].targetValue,

                // Set progress value
                progressValue = 0,

                // Set goal value
                goalValue = allTimeMissionSpawnData[i].goalValue,

                // Reward
                reward = allTimeMissionSpawnData[i].reward
            };
            // Set mission description
            if (missionType == "Chain") {
                mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Make " + allTimeMissionSpawnData[i].goalValue + " chains equal to " + allTimeMissionSpawnData[i].targetValue + " points or more";
            } else if (missionType == "Area") {
                mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Place " + allTimeMissionSpawnData[i].goalValue + " pieces with and area of " + allTimeMissionSpawnData[i].targetValue + " or more";
            } else {
                mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Place " + (int)allTimeMissionSpawnData[i].targetValue + " game pieces";
            }
        }
    }


    // For spawning old missions
    private void SpawnExistingMissions(bool isDaily) {

    }

    private string[] GenerateRandomCategories(int randomCategoriesNum) {
        string[] categories = new string[randomCategoriesNum];
        for (int i = 0; i < randomCategoriesNum; i++) {
            int randomCategory = UnityEngine.Random.Range(0, categoriesList.Length);
            categories[i] = categoriesList[randomCategory];
        }
        return categories;
    }

    // Function for updating missions. This will get called at the end of the game
    // This is going to be painful computationally but IDK how else to do it
    public void UpdateMissions() {
        // Look at each mission
        // Check if the mission is incomplete, if yes then continue, else ignore
        // Look at the mission type and thats how I know what to check, chain, area, or game piece
        // Update the mission based on the values in the totalChains, totalAreas, and totalGamePieces

        // Use array.Contains(target); for categories

        /*
            In GamePieceManager, there is a variable that I will make public that holds the previous chain before the new object is added.
            I can use this to check against a total running list of chains and remove the chain that matches this previous value.
            If the chain is not found, then just ignore for now.
            But then add the new chain to the list.
            This is so I don't just keep duplicating chains past a certain value. For example, if the mission was 5 chains
            of 1000 and I got 1200, then added to it and got 2500, then added to it and got 5000. I don't want this to count as 3
            times but just once. 
        */

    }
}
