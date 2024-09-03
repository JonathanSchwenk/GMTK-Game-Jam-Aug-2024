using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTimeMissionSpawnData {
    public string[] categories { get; set; }
    public float targetValue { get; set; }
    public int goalValue { get; set; }
    public int reward { get; set; }

    public AllTimeMissionSpawnData(string[] categories, float targetValue, int goalValue, int reward) {
        this.categories = categories;
        this.targetValue = targetValue;
        this.goalValue = goalValue;
        this.reward = reward;
    }
}
