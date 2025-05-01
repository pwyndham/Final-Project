using UnityEngine;

public class Shopkeeper : Interactable
{
    [SerializeField] private GameObject shopkeeperInventoryPanel; // Assign in Inspector
    [SerializeField] private GameObject playerInventoryPanel; // Assign in Inspector

    protected override void Interact()
    {
        base.Interact();
        Debug.Log("Interacted with shopkeeper!");

        // Toggle inventories on interaction
        bool isActive = shopkeeperInventoryPanel.activeSelf; 
        shopkeeperInventoryPanel.SetActive(!isActive);
        playerInventoryPanel.SetActive(!isActive);
    }
}
