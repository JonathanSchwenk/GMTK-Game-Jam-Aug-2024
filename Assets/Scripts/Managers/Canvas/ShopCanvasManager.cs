using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class ShopCanvasManager : MonoBehaviour
{

    [SerializeField] private ShopAbility extraTime;
    [SerializeField] private ShopAbility skips;
    [SerializeField] private ShopAbility destroys;
    [SerializeField] private ShopAbility extraEnlarges;
    [SerializeField] private ShopAbility extraShrinks;

    private IInventoryManager inventoryManager;
    private IMenuCanvasManager menuCanvasManager;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = ServiceLocator.Resolve<IInventoryManager>();
        menuCanvasManager = ServiceLocator.Resolve<IMenuCanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        extraTime.numInInventoryText.text = inventoryManager.extraTime.ToString();
        skips.numInInventoryText.text = inventoryManager.skips.ToString();
        destroys.numInInventoryText.text = inventoryManager.destroys.ToString();
        extraEnlarges.numInInventoryText.text = inventoryManager.extraEnlarges.ToString();
        extraShrinks.numInInventoryText.text = inventoryManager.extraShrinks.ToString();
    }

    public void OnBackButtonPressed() {
        // Close the shop
        menuCanvasManager.UpdateCanvasState(MenuCanvasState.Start);
    }

    // TODO: Need a not enough gems popup or something

    public void OnExtraTimeButtonPressed() {
        // Buy extra time
        if (inventoryManager.gems >= int.Parse(extraTime.costText.text)) {
            inventoryManager.gems -= int.Parse(extraTime.costText.text);
            inventoryManager.extraTime++;
            inventoryManager.SaveInventory();
        }
    }

    public void OnSkipsButtonPressed() {
        // Buy skips
        if (inventoryManager.gems >= int.Parse(skips.costText.text)) {
            inventoryManager.gems -= int.Parse(skips.costText.text);
            inventoryManager.skips++;
            inventoryManager.SaveInventory();
        }
    }

    public void OnDestroysButtonPressed() {
        // Buy destroys
        if (inventoryManager.gems >= int.Parse(destroys.costText.text)) {
            inventoryManager.gems -= int.Parse(destroys.costText.text);
            inventoryManager.destroys++;
            inventoryManager.SaveInventory();
        }
    }

    public void OnExtraEnlargesButtonPressed() {
        // Buy extra enlarges
        if (inventoryManager.gems >= int.Parse(extraEnlarges.costText.text)) {
            inventoryManager.gems -= int.Parse(extraEnlarges.costText.text);
            inventoryManager.extraEnlarges++;
            inventoryManager.SaveInventory();
        }
    }

    public void OnExtraShrinksButtonPressed() {
        // Buy extra shrinks
        if (inventoryManager.gems >= int.Parse(extraShrinks.costText.text)) {
            inventoryManager.gems -= int.Parse(extraShrinks.costText.text);
            inventoryManager.extraShrinks++;
            inventoryManager.SaveInventory();
        }
    }
}
