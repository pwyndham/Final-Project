using UnityEngine;

public class PlayerInventoryInput : MonoBehaviour
{
    public Transform playerInventoryGrid; 
    public Transform hotbarGrid;
    CharacterStats characterStats;

    public int energyPotionAmount = 10;
    public int healthPotionAmount = 10;
    public int manaPotionAmount = 10;


    void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //characterStats.energy
            characterStats.UseEnergyPotion(energyPotionAmount);
            characterStats.UpdateBars();
            UsePotion(PotionType.Energy);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            characterStats.UseHealthPotion(healthPotionAmount);
            characterStats.UpdateBars();
            UsePotion(PotionType.Health);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            characterStats.UseManaPotion(manaPotionAmount);
            characterStats.UpdateBars();
            UsePotion(PotionType.Mana);
        }
    }

    void UsePotion(PotionType type)
    {
        if (TryUsePotionInGrid(hotbarGrid, type)) return;
        if (TryUsePotionInGrid(playerInventoryGrid, type)) return;

        Debug.Log("No " + type + " potions available!");
    }

    bool TryUsePotionInGrid(Transform grid, PotionType type)
    {
        foreach (Transform slot in grid)
        {
            if (slot.childCount > 0)
            {
                InventoryItem item = slot.GetChild(0).GetComponent<InventoryItem>();
                if (item != null && item.potionType == type)
                {
                    Debug.Log("Used " + type + " potion!");
                    Destroy(item.gameObject); // Remove item from slot
                    return true;
                }
            }
        }
        return false;
    }
}