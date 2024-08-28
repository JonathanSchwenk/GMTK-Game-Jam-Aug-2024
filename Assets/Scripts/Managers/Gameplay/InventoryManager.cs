using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class InventoryManager : MonoBehaviour, IInventoryManager
{
    public int gems {get; set;}


    private ISaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = ServiceLocator.Resolve<ISaveManager>();

        gems = saveManager.saveData.gems;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
