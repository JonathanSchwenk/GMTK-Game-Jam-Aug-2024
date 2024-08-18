using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ObjectsDatabase", menuName = "GMTK-Game-Jam-Aug-2024/ObjectsDatabase", order = 1)]
public class ObjectsDatabase : ScriptableObject
{
    public GameObject[] cityObjects;
    public GameObject[] egyptObjects;
    public GameObject[] japanObjects;
    public GameObject[] medievalObjects;
    public GameObject[] neighborhoodObjects;
    public GameObject[] pirateObjects;
    public GameObject[] scifiObjects;
    public GameObject[] westernObjects;
}
