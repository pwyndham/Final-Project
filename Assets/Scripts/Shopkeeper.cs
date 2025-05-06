using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shopkeeper : Interactable
{
    [SerializeField] private GameObject shopkeeperInventoryPanel; // Assign in Inspector
    [SerializeField] private GameObject playerInventoryPanel; // Assign in Inspector
    public CharacterStats characterStats;
    //public int CharacterMoney;
    public TMP_Text characterMoneyText;

    public int ShopKeeperMoney;
    public TMP_Text ShopKeeperMoneyText;

    public Button HealthPotionButton;
    public Button ManaPotionButton;
    public Button EnergyPotionButton;

    public int ManaPotionCount;
    public int HealthPotionCount;
    public int EnergyPotionCount;

    public TMP_Text ManaPotionCountText;
    public TMP_Text HealthPotionCountText;
    public TMP_Text EnergyPotionCountText;

    [SerializeField] private GameObject manaPotionPrefab;
    [SerializeField] private GameObject healthPotionPrefab;
    [SerializeField] private GameObject energyPotionPrefab;
    [SerializeField] private Transform playerInventorySlotParent;
    [SerializeField] private Transform playerInventoryGrid;
    public int ManaPotionPrice;
    public int HealthPotionPrice;
    public int EnergyPotionPrice;

    public TMP_Text ManaPriceText;
    public TMP_Text EnergyPriceText;
    public TMP_Text HealthPriceText;

    protected override void Interact()
    {
        base.Interact();
        Debug.Log("Interacted with shopkeeper!");

        // Toggle inventories on interaction
        bool isActive = shopkeeperInventoryPanel.activeSelf; 
        shopkeeperInventoryPanel.SetActive(!isActive);
        playerInventoryPanel.SetActive(!isActive);
    }
    void Awake()
    {
        //characterStats = GetComponent<CharacterStats>();
        //CharacterMoney = characterStats.characterMoney;
    }
    void Start()
    {
        
        ManaPriceText.text = ManaPotionPrice.ToString();
        EnergyPriceText.text = EnergyPotionPrice.ToString();
        HealthPriceText.text = HealthPotionPrice.ToString();
    
        ManaPotionCountText.text = ManaPotionCount.ToString();
        EnergyPotionCountText.text = EnergyPotionCount.ToString();
        HealthPotionCountText.text = HealthPotionCount.ToString();

        ShopKeeperMoneyText.text = ShopKeeperMoney.ToString();

        ManaPotionButton.onClick.AddListener(BuyManaPotion);
        HealthPotionButton.onClick.AddListener(BuyHealthPotion);
        EnergyPotionButton.onClick.AddListener(BuyEnergyPotion);
    
        characterMoneyText.text = characterStats.characterMoney.ToString();
    }
    
        public void BuyManaPotion()
    {
        if (characterStats.characterMoney >= ManaPotionPrice)
        {
            characterStats.characterMoney -= ManaPotionPrice;
            ManaPotionCount--;
            ManaPotionCountText.text = ManaPotionCount.ToString();
            ShopKeeperMoney+=ManaPotionPrice;
            ShopKeeperMoneyText.text = ShopKeeperMoney.ToString();
            characterMoneyText.text = characterStats.characterMoney.ToString();
            Debug.Log("Transfer mana potion");

            foreach (Transform slot in playerInventoryGrid)
        {
            if (slot.childCount == 0)
            {
                GameObject newItem = Instantiate(manaPotionPrefab, slot);
                newItem.transform.localPosition = Vector3.zero;

                // Set parent reference 
                InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
                inventoryItem.potionType = PotionType.Mana; 
                inventoryItem.originalParentReference = slot;

                break;
            }
        }

        Debug.Log("Transferred mana potion to player");
        }
        else
        {
            Debug.Log("Not enough money or out of potions");
        }
    }
    void Update()
    {
        characterMoneyText.text = characterStats.characterMoney.ToString();
    }
    public void BuyHealthPotion()
    {
        if (characterStats.characterMoney >= HealthPotionPrice)
        {
            characterStats.characterMoney -= HealthPotionPrice;
            HealthPotionCount--;
            HealthPotionCountText.text = HealthPotionCount.ToString();
            ShopKeeperMoney+=HealthPotionPrice;
            ShopKeeperMoneyText.text = ShopKeeperMoney.ToString();
            characterMoneyText.text = characterStats.characterMoney.ToString();
            Debug.Log("Transfer health potion");
        foreach (Transform slot in playerInventoryGrid)
        {
            if (slot.childCount == 0)
            {
                GameObject newItem = Instantiate(healthPotionPrefab, slot);
                newItem.transform.localPosition = Vector3.zero;

                // Set parent reference 
                InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
                inventoryItem.potionType = PotionType.Health; 
                inventoryItem.originalParentReference = slot;

                break;
            }
        }

        Debug.Log("Transferred mana potion to player");
        }
        else
        {
            Debug.Log("Not enough money or out of potions");
        }
    }

    public void BuyEnergyPotion()
    {
       if (characterStats.characterMoney >= EnergyPotionPrice)
        {
            characterStats.characterMoney -= EnergyPotionPrice;
            EnergyPotionCount--;
            EnergyPotionCountText.text = EnergyPotionCount.ToString();
            ShopKeeperMoney+=EnergyPotionPrice;
            ShopKeeperMoneyText.text = ShopKeeperMoney.ToString();
            characterMoneyText.text = characterStats.characterMoney.ToString();
            Debug.Log("Transfer energy potion");

            
        foreach (Transform slot in playerInventoryGrid)
        {
            if (slot.childCount == 0)
            {
                GameObject newItem = Instantiate(energyPotionPrefab, slot);
                newItem.transform.localPosition = Vector3.zero;

                // Set parent reference 
                InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
                inventoryItem.potionType = PotionType.Energy; 
                inventoryItem.originalParentReference = slot;

                break;
            }
        }

        Debug.Log("Transferred mana potion to player");
        }
        else
        {
            Debug.Log("Not enough money or out of potions");
        }
    }
}
