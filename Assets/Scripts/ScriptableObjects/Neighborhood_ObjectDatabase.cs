using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Neighborhood_ObjectDatabase", menuName = "GMTK-Game-Jam-Aug-2024/Neighborhood_ObjectDatabase", order = 6)]
public class Neighborhood_ObjectDatabase : ScriptableObject
{
    public GameObject[] objects; // Array to hold your building prefabs
}
