using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTimeMissionFile
{
    // Chains
    public List<AllTimeMissionSpawnData> chainAllTimeSpawnData = new List<AllTimeMissionSpawnData>
    {
        new AllTimeMissionSpawnData(new string[] { "City" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "City" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "City" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Egypt" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Japan" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Japan" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Medieval" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Neighborhood" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Pirate" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Scifi" }, 10000, 15, 1000),

        new AllTimeMissionSpawnData(new string[] { "Western" }, 5000, 25, 500),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 7500, 20, 750),
        new AllTimeMissionSpawnData(new string[] { "Western" }, 10000, 15, 1000),
    };

    // Categories
    public List<AllTimeMissionSpawnData> categoryAllTimeSpawnData = new List<AllTimeMissionSpawnData>
    {
        new AllTimeMissionSpawnData(new string[] { "Category1" }, 500, 2, 200),
        // Add more AllTimeMissionSpawnData objects if needed
    };

    // Game Piece
    public List<AllTimeMissionSpawnData> gamePieceAllTimeSpawnData = new List<AllTimeMissionSpawnData>
    {
        new AllTimeMissionSpawnData(new string[] { "GamePiece1" }, 300, 3, 300),
        // Add more AllTimeMissionSpawnData objects if needed
    };
}