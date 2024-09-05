using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllTimeMissionFile {
    // Chains
    public static List<AllTimeMissionSpawnData> chainAllTimeSpawnData = new List<AllTimeMissionSpawnData>
    {
        new AllTimeMissionSpawnData(new string[] { "City" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "City" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "City" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Japan" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 10000, 10, 1000),

        new AllTimeMissionSpawnData(new string[] { "Western" }, 3000, 30, 500),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 6000, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 10000, 10, 1000),
    };

    // Area
    public static List<AllTimeMissionSpawnData> areaAllTimeSpawnData = new List<AllTimeMissionSpawnData>
    {
        new AllTimeMissionSpawnData(new string[] { "City" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "City" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "City" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Japan" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 5000, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 700, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 1000, 20, 750),

        new AllTimeMissionSpawnData(new string[] { "Western" }, 500, 80, 250),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 750, 40, 500),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 1000, 20, 750),
    };

    // Game Piece
    public static List<AllTimeMissionSpawnData> gamePieceAllTimeSpawnData = new List<AllTimeMissionSpawnData>
    {
        new AllTimeMissionSpawnData(new string[] { "City" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "City" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "City" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Japan" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 1000, 1000, 1000),

        new AllTimeMissionSpawnData(new string[] { "Western" }, 250, 250, 250),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 500, 500, 500),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 1000, 1000, 1000),
    };
}