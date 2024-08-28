using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class InventoryManager : MonoBehaviour, IInventoryManager
{
    public int gems {get; set;}
    public int extraTime {get; set;}
    public int skips {get; set;}
    public int destroys {get; set;}
    public int extraEnlarges {get; set;}
    public int extraShrinks {get; set;}


    private ISaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = ServiceLocator.Resolve<ISaveManager>();

        gems = saveManager.saveData.gems;
        extraTime = saveManager.saveData.extraTime;
        skips = saveManager.saveData.skips;
        destroys = saveManager.saveData.destroys;
        extraEnlarges = saveManager.saveData.extraEnlarges;
        extraShrinks = saveManager.saveData.extraShrinks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveInventory() {
        saveManager.saveData.gems = gems;
        saveManager.saveData.extraTime = extraTime;
        saveManager.saveData.skips = skips;
        saveManager.saveData.destroys = destroys;
        saveManager.saveData.extraEnlarges = extraEnlarges;
        saveManager.saveData.extraShrinks = extraShrinks;

        saveManager.Save();
    }
}
