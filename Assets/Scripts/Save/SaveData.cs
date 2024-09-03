using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData 
{
    public float highScore = 0;
    public float highChain = 0;
    public int gems = 0;
    public bool SFXOn = true;
    public bool musicOn = true;
    public int extraTime = 0;
    public int skips = 0;
    public int destroys = 0;
    public int extraEnlarges = 0;
    public int extraShrinks = 0;

    public long lastActive;
    public List<MissionData> dailyMissions;
    public List<MissionData> allTimeMissions;
}

