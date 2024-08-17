using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour, ISpawnManager
{

    public Egypt_ObjectDatabase egyptObjects {get; set;}

    [SerializeField] private Egypt_ObjectDatabase egyptObjectDatabase;
    // Start is called before the first frame update
    void Start()
    {
        egyptObjects = egyptObjectDatabase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
