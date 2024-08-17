using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "City_ObjectDatabase", menuName = "GMTK-Game-Jam-Aug-2024/City_ObjectDatabase", order = 3)]
public class City_ObjectDatabase : ScriptableObject
{
    public GameObject[] objects; // Array to hold your building prefabs
}
