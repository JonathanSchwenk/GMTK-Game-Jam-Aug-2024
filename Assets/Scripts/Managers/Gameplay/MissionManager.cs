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
    private IGameManager gameManager;
    private IInventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start() {
        saveManager = ServiceLocator.Resolve<ISaveManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
        inventoryManager = ServiceLocator.Resolve<IInventoryManager>();

        if (gameManager != null) {
            gameManager.OnGameStateChanged += HandleGameStateChanged;
        }

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

        // Need to init these lists
        totalChains = new TotalChains() {
            cityChains = new List<float>(),
            egyptChains = new List<float>(),
            japanChains = new List<float>(),
            medievalChains = new List<float>(),
            neighborhoodChains = new List<float>(),
            pirateChains = new List<float>(),
            scifiChains = new List<float>(),
            westernChains = new List<float>()
        };

        totalAreas = new TotalAreas() {
            cityAreas = new List<float>(),
            egyptAreas = new List<float>(),
            japanAreas = new List<float>(),
            medievalAreas = new List<float>(),
            neighborhoodAreas = new List<float>(),
            pirateAreas = new List<float>(),
            scifiAreas = new List<float>(),
            westernAreas = new List<float>()
        };
        totalGamePieces = new TotalGamePieces() {
            cityGamePieces = 0,
            egyptGamePieces = 0,
            japanGamePieces = 0,
            medievalGamePieces = 0,
            neighborhoodGamePieces = 0,
            pirateGamePieces = 0,
            scifiGamePieces = 0,
            westernGamePieces = 0
        };
    }

    private void HandleGameStateChanged(GameState newState) {
        if (newState == GameState.GameOver) {
            // Update missions
            UpdateMissions(dailyMissions);
            UpdateMissions(allTimeMissions);

            // Save
            saveManager.saveData.dailyMissions = new List<MissionData>(dailyMissions);
            saveManager.saveData.allTimeMissions = new List<MissionData>(allTimeMissions);
            saveManager.Save();

            // Clear total lists
            totalChains = new TotalChains();
            totalAreas = new TotalAreas();
            totalGamePieces = new TotalGamePieces();
        }
    }

    private void OnDestroy() {
        if (gameManager != null) {
            gameManager.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    // Take in prefab and location to spawn
    public void SpawnDailyMissions(GameObject missionPrefab, GameObject scrollContent) {
        // Check if its a new day, if it is or if its null, then make new missions, else load the old missions
        DateTime now = DateTime.Now;
        // DateTime lastTime = new DateTime(saveManager.saveData.lastActive);
        DateTime lastTime = new DateTime(2021, 1, 1);

        if (now.Day != lastTime.Day || saveManager.saveData.dailyMissions.Count <= 0) {
            // Clear to reset
            dailyMissions.Clear();

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
            int randomChain = UnityEngine.Random.Range(3, 7);
            targetValue = randomChain * 1000;
            mission.GetComponent<MissionGameObject>().missionData.targetValue = targetValue;
        } else if (missionType == "Area") {
            int randomArea = UnityEngine.Random.Range(4, 8); // 1000 area might be too much
            targetValue = randomArea * 100;
            mission.GetComponent<MissionGameObject>().missionData.targetValue = targetValue;
        } else {
            int randomGamePiece = UnityEngine.Random.Range(5, 11);
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
            mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Make " + mission.GetComponent<MissionGameObject>().missionData.goalValue + " chains equal to " + targetValue + " points or more";
        } else if (missionType == "Area") {
            mission.GetComponent<MissionGameObject>().missionData.missionDesc = "Place " + mission.GetComponent<MissionGameObject>().missionData.goalValue + " pieces with and area of " + targetValue + " or more";
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
        if (saveManager.saveData.allTimeMissions == null || saveManager.saveData.allTimeMissions.Count <= 0) {
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

            // Save the new missions
            saveManager.saveData.allTimeMissions = new List<MissionData>(allTimeMissions);
            saveManager.Save();
        } else {
            // Chain
            GameObject titleChain = Instantiate(titlePrefab, chainTitle.transform);
            titleChain.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chain Missions";
            SpawnExistingMissions(saveManager.saveData.allTimeMissions.Take(24).ToList(), missionPrefab, chainContent);
            // Area
            GameObject titleArea = Instantiate(titlePrefab, areaTitle.transform);
            titleArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Area Missions";
            SpawnExistingMissions(saveManager.saveData.allTimeMissions.Skip(24).Take(24).ToList(), missionPrefab, areaContent);
            // Total Game Pieces
            GameObject titleGamePiece = Instantiate(titlePrefab, gamePieceTitle.transform);
            titleGamePiece.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Total Game Piece Missions";
            SpawnExistingMissions(saveManager.saveData.allTimeMissions.Skip(48).Take(24).ToList(), missionPrefab, gamePieceContent);

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

            allTimeMissions.Add(mission.GetComponent<MissionGameObject>().missionData);
        }
    }

    private string[] GenerateRandomCategories(int randomCategoriesNum) {
        string[] categories = new string[randomCategoriesNum];
        for (int i = 0; i < randomCategoriesNum; i++) {
            int randomCategory = UnityEngine.Random.Range(0, categoriesList.Length);
            while (categories.Contains(categoriesList[randomCategory])) {
                randomCategory = UnityEngine.Random.Range(0, categoriesList.Length);
            }
            categories[i] = categoriesList[randomCategory];
        }
        return categories;
    }

    public void SubmitMission(GameObject go) {
        MissionData missionData = go.transform.GetComponent<MissionGameObject>().missionData;
        if (missionData.status == "Complete") {
            // Give the reward
            inventoryManager.gems += missionData.reward;
            inventoryManager.SaveInventory();
            // Set the status to submitted
            missionData.status = "Submitted";
            // Display the reward particle effect
            go.transform.GetChild(0).gameObject.SetActive(true);
            // Disable the mission after 5 seconds
            StartCoroutine(submitMissionCoroutine(go.transform.GetChild(0).gameObject));
        }
    }

    private IEnumerator submitMissionCoroutine(GameObject go) {
        yield return new WaitForSeconds(5f);
        go.SetActive(false);
    }

    // Function for updating missions. This will get called at the end of the game
    // This is going to be painful computationally but IDK how else to do it
    # region Update Missions

    public void UpdateMissions(List<MissionData> missionListToUpdate) {
        PreformUpdate(missionListToUpdate);
    }

    private void PreformUpdate(List<MissionData> missionListToUpdate) {
        for (int i = 0; i < missionListToUpdate.Count; i++) {
            if (missionListToUpdate[i].status == "Incomplete") {
                if (missionListToUpdate[i].missionType == "Chain") {
                    CheckAndUpdateChainMissions(missionListToUpdate, i);
                } else if (missionListToUpdate[i].missionType == "Area") {
                    CheckAndUpdateAreaMissions(missionListToUpdate, i);
                } else {
                    CheckAndUpdateGamePieceMissions(missionListToUpdate, i);
                }
                ChangeStatusOfMission(missionListToUpdate, i);
            }
        }
    }

    private void ChangeStatusOfMission(List<MissionData> missionListToUpdate, int i) {
        if (missionListToUpdate[i].progressValue >= missionListToUpdate[i].goalValue) {
            missionListToUpdate[i].status = "Complete";
        }
    }

    # endregion

    # region Check and update Missions

    private void CheckAndUpdateChainMissions(List<MissionData> missionListToUpdate, int i) {
        // For each mission, check the categories and see if I should increase the progress of the mission
        for (int j = 0; j < missionListToUpdate[i].categories.Length; j++) {
            if (missionListToUpdate[i].categories[j] == "All") {
                for (int k = 0; k < totalChains.cityChains.Count; k++) {
                    if (totalChains.cityChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.egyptChains.Count; k++) {
                    if (totalChains.egyptChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.japanChains.Count; k++) {
                    if (totalChains.japanChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.medievalChains.Count; k++) {
                    if (totalChains.medievalChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.neighborhoodChains.Count; k++) {
                    if (totalChains.neighborhoodChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.pirateChains.Count; k++) {
                    if (totalChains.pirateChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.scifiChains.Count; k++) {
                    if (totalChains.scifiChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalChains.westernChains.Count; k++) {
                    if (totalChains.westernChains[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }

            } else {
                switch (missionListToUpdate[i].categories[j]) {
                    case "City":
                        for (int k = 0; k < totalChains.cityChains.Count; k++) {
                            if (totalChains.cityChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Egypt":
                        for (int k = 0; k < totalChains.egyptChains.Count; k++) {
                            if (totalChains.egyptChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Japan":
                        for (int k = 0; k < totalChains.japanChains.Count; k++) {
                            if (totalChains.japanChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Medieval":
                        for (int k = 0; k < totalChains.medievalChains.Count; k++) {
                            if (totalChains.medievalChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Neighborhood":
                        for (int k = 0; k < totalChains.neighborhoodChains.Count; k++) {
                            if (totalChains.neighborhoodChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Pirate":
                        for (int k = 0; k < totalChains.pirateChains.Count; k++) {
                            if (totalChains.pirateChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Scifi":
                        for (int k = 0; k < totalChains.scifiChains.Count; k++) {
                            if (totalChains.scifiChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Western":
                        for (int k = 0; k < totalChains.westernChains.Count; k++) {
                            if (totalChains.westernChains[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    default:
                        break;
                }
                // Check if the progress value is equal to the goal value, if so, then set the status to complete
                if (missionListToUpdate[i].progressValue >= missionListToUpdate[i].goalValue) {
                    missionListToUpdate[i].status = "Complete";
                }
            }
        }
    }

    private void CheckAndUpdateAreaMissions(List<MissionData> missionListToUpdate, int i) {
        // For each mission, check the categories and see if I should increase the progress of the mission
        for (int j = 0; j < missionListToUpdate[i].categories.Length; j++) {
            if (missionListToUpdate[i].categories[j] == "All") {
                for (int k = 0; k < totalAreas.cityAreas.Count; k++) {
                    if (totalAreas.cityAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.egyptAreas.Count; k++) {
                    if (totalAreas.egyptAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.japanAreas.Count; k++) {
                    if (totalAreas.japanAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.medievalAreas.Count; k++) {
                    if (totalAreas.medievalAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.neighborhoodAreas.Count; k++) {
                    if (totalAreas.neighborhoodAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.pirateAreas.Count; k++) {
                    if (totalAreas.pirateAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.scifiAreas.Count; k++) {
                    if (totalAreas.scifiAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
                for (int k = 0; k < totalAreas.westernAreas.Count; k++) {
                    if (totalAreas.westernAreas[k] >= missionListToUpdate[i].targetValue) {
                        missionListToUpdate[i].progressValue += 1;
                    }
                }
            } else {
                switch (missionListToUpdate[i].categories[j]) {
                    case "City":
                        for (int k = 0; k < totalAreas.cityAreas.Count; k++) {
                            if (totalAreas.cityAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Egypt":
                        for (int k = 0; k < totalAreas.egyptAreas.Count; k++) {
                            if (totalAreas.egyptAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Japan":
                        for (int k = 0; k < totalAreas.japanAreas.Count; k++) {
                            if (totalAreas.japanAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Medieval":
                        for (int k = 0; k < totalAreas.medievalAreas.Count; k++) {
                            if (totalAreas.medievalAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Neighborhood":
                        for (int k = 0; k < totalAreas.neighborhoodAreas.Count; k++) {
                            if (totalAreas.neighborhoodAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Pirate":
                        for (int k = 0; k < totalAreas.pirateAreas.Count; k++) {
                            if (totalAreas.pirateAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Scifi":
                        for (int k = 0; k < totalAreas.scifiAreas.Count; k++) {
                            if (totalAreas.scifiAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    case "Western":
                        for (int k = 0; k < totalAreas.westernAreas.Count; k++) {
                            if (totalAreas.westernAreas[k] >= missionListToUpdate[i].targetValue) {
                                missionListToUpdate[i].progressValue += 1;
                            }
                        }
                        break;
                    default:
                        break;
                }
                // Check if the progress value is equal to the goal value, if so, then set the status to complete
                if (missionListToUpdate[i].progressValue >= missionListToUpdate[i].goalValue) {
                    missionListToUpdate[i].status = "Complete";
                }
            }
        }
    }

    private void CheckAndUpdateGamePieceMissions(List<MissionData> missionListToUpdate, int i) {
        // For each mission, check the categories and see if I should increase the progress of the mission
        for (int j = 0; j < missionListToUpdate[i].categories.Length; j++) {
            if (missionListToUpdate[i].categories[j] == "All") {
                missionListToUpdate[i].progressValue += totalGamePieces.cityGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.egyptGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.japanGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.medievalGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.neighborhoodGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.pirateGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.scifiGamePieces;
                missionListToUpdate[i].progressValue += totalGamePieces.westernGamePieces;
            } else {
                switch (missionListToUpdate[i].categories[j]) {
                    case "City":
                        missionListToUpdate[i].progressValue += totalGamePieces.cityGamePieces;
                        break;
                    case "Egypt":
                        missionListToUpdate[i].progressValue += totalGamePieces.egyptGamePieces;
                        break;
                    case "Japan":
                        missionListToUpdate[i].progressValue += totalGamePieces.japanGamePieces;
                        break;
                    case "Medieval":
                        missionListToUpdate[i].progressValue += totalGamePieces.medievalGamePieces;
                        break;
                    case "Neighborhood":
                        missionListToUpdate[i].progressValue += totalGamePieces.neighborhoodGamePieces;
                        break;
                    case "Pirate":
                        missionListToUpdate[i].progressValue += totalGamePieces.pirateGamePieces;
                        break;
                    case "Scifi":
                        missionListToUpdate[i].progressValue += totalGamePieces.scifiGamePieces;
                        break;
                    case "Western":
                        missionListToUpdate[i].progressValue += totalGamePieces.westernGamePieces;
                        break;
                    default:
                        break;
                }
                // Check if the progress value is equal to the goal value, if so, then set the status to complete
                if (missionListToUpdate[i].progressValue >= missionListToUpdate[i].goalValue) {
                    missionListToUpdate[i].status = "Complete";
                }
            }
        }
    }

    # endregion

    # region add to total lists

    public void AddToChainList(float chainValueToAdd, float? chainValueToRemove, string category) {
        if (chainValueToRemove != null || chainValueToRemove != 0) {
            CheckAndRemoveChains((float)chainValueToRemove, category);
        }
        AddChain(chainValueToAdd, category);
    }

    private void AddChain(float chainValue, string category) {
        switch (category) {
            case "City":
                totalChains.cityChains.Add(chainValue);
                break;
            case "Egypt":
                totalChains.egyptChains.Add(chainValue);
                break;
            case "Japan":
                totalChains.japanChains.Add(chainValue);
                break;
            case "Medieval":
                totalChains.medievalChains.Add(chainValue);
                break;
            case "Neighborhood":
                totalChains.neighborhoodChains.Add(chainValue);
                break;
            case "Pirate":
                totalChains.pirateChains.Add(chainValue);
                break;
            case "Scifi":
                totalChains.scifiChains.Add(chainValue);
                break;
            case "Western":
                totalChains.westernChains.Add(chainValue);
                break;
            default:
                break;
        }
    }

    private void CheckAndRemoveChains(float chainValue, string category) {
        switch (category) {
            case "City":
                if (totalChains.cityChains.Contains(chainValue)) {
                    totalChains.cityChains.Remove(chainValue);
                }
                break;
            case "Egypt":
                if (totalChains.egyptChains.Contains(chainValue)) {
                    totalChains.egyptChains.Remove(chainValue);
                }
                break;
            case "Japan":
                if (totalChains.japanChains.Contains(chainValue)) {
                    totalChains.japanChains.Remove(chainValue);
                }
                break;
            case "Medieval":
                if (totalChains.medievalChains.Contains(chainValue)) {
                    totalChains.medievalChains.Remove(chainValue);
                }
                break;
            case "Neighborhood":
                if (totalChains.neighborhoodChains.Contains(chainValue)) {
                    totalChains.neighborhoodChains.Remove(chainValue);
                }
                break;
            case "Pirate":
                if (totalChains.pirateChains.Contains(chainValue)) {
                    totalChains.pirateChains.Remove(chainValue);
                }
                break;
            case "Scifi":
                if (totalChains.scifiChains.Contains(chainValue)) {
                    totalChains.scifiChains.Remove(chainValue);
                }
                break;
            case "Western":
                if (totalChains.westernChains.Contains(chainValue)) {
                    totalChains.westernChains.Remove(chainValue);
                }
                break;
            default:
                break;
        }
    }

    public void AddToAreaList(float areaValue, string category) {
        switch (category) {
            case "City":
                totalAreas.cityAreas.Add(areaValue);
                break;
            case "Egypt":
                totalAreas.egyptAreas.Add(areaValue);
                break;
            case "Japan":
                totalAreas.japanAreas.Add(areaValue);
                break;
            case "Medieval":
                totalAreas.medievalAreas.Add(areaValue);
                break;
            case "Neighborhood":
                totalAreas.neighborhoodAreas.Add(areaValue);
                break;
            case "Pirate":
                totalAreas.pirateAreas.Add(areaValue);
                break;
            case "Scifi":
                totalAreas.scifiAreas.Add(areaValue);
                break;
            case "Western":
                totalAreas.westernAreas.Add(areaValue);
                break;
            default:
                break;
        }
    }

    public void AddToGamePieceList(string category) {
        switch (category) {
            case "City":
                totalGamePieces.cityGamePieces++;
                break;
            case "Egypt":
                totalGamePieces.egyptGamePieces++;
                break;
            case "Japan":
                totalGamePieces.japanGamePieces++;
                break;
            case "Medieval":
                totalGamePieces.medievalGamePieces++;
                break;
            case "Neighborhood":
                totalGamePieces.neighborhoodGamePieces++;
                break;
            case "Pirate":
                totalGamePieces.pirateGamePieces++;
                break;
            case "Scifi":
                totalGamePieces.scifiGamePieces++;
                break;
            case "Western":
                totalGamePieces.westernGamePieces++;
                break;
            default:
                break;
        }
    }

    # endregion
}
