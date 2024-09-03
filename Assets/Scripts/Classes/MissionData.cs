using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MissionData {
    public int missionID { get; set; }
    public string missionType { get; set; }
    public string[] categories { get; set; }
    public string status { get; set; }
    public string missionDesc { get; set; }
    public int progressValue { get; set; }
    public float targetValue { get; set; }
    public int goalValue { get; set; }
    public int reward { get; set; }
}
