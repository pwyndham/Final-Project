using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    Character character;
    CharacterAbilities characterAbilities;
    public AbilitiesUI abilitiesUI;
    public string classTypeCached;
    public BaseClass characterClass;
    public CharacterClassHandler characterClassHandler;
    CharacterExperience characterExperience;
    /// <summary>
    /// NOTE: ALL VALUES ARE FOR BASIS. CHANGE DUE TO STAT SYSTEM. NO NULLS
    /// </summary>
    /// 
    
    public int characterMoney = 100;

    public float levelUpPoints;
    public float energyCost = 5f; // character input energy
    public float projectileCost = 15f; // character input projectiles

    public float healthPoints = 100f; // constitution
    public float maxHealthPoints = 100f; // constitution
    public float healthRestoration = 10f; // constitution

    public float manaPoints = 100f; // intelligence
    public float maxManaPoints = 100f; // intelligence
    public float manaRestoration = 10f; // intelligence

    public float energyPoints = 100f; //endurance
    public float maxEnergyPoints = 100f; //endurance
    public float energyRestoration = 10f; //endurance

    public event Action<float, float> onManaChanged;
    public event Action<float, float> onHealthChanged;
    public event Action<float, float> onEnergyChanged;

    public float meleeDamage; // strength
    public float meleeShortDamage; // dexterity
    public float magicDamage; // intelligence
    public float projectileSpeed = 20f;
    float limiter;
    [SerializeField] float characterSpeed = 10f; // agility
    [SerializeField] float defaultSpeed = 10f; // agility
    [SerializeField] float sprintSpeed = 15f; // agility
    [SerializeField] float defaultJumpSpeed = 10f; // agility
    [SerializeField] float jumpSpeed = 10f; // agility
    [SerializeField] float enemyDetection = 1f; // perception
    [SerializeField] float armorResistance = 10f; // armor resistance
    [SerializeField] float magicResistance = 10f; // magic resistance
    [SerializeField] float criticalStrike = 10f; // critical strike

    public float DefaultSpeed 
    {
        get {return defaultSpeed; }
        set {defaultSpeed = value; }
    }
    public float SprintSpeed 
    {
        get {return sprintSpeed; }
        set {sprintSpeed = value; }
    }
    public float CharacterSpeed 
    {
        get {return characterSpeed; }
        set {characterSpeed = value; }
    }

    public float JumpSpeed 
    {
        get {return jumpSpeed; }
        set {jumpSpeed = value; }
    }
    public float DefaultJumpSpeed 
    {
        get {return defaultJumpSpeed; }
        set {defaultJumpSpeed = value; }
    }
    
    public float ProjectileCost 
    {
        get {return projectileCost; }
        set {projectileCost = value; }
    }
    
    public float EnergyCost 
    {
        get {return energyCost; }
        set {energyCost = value; }
    }
    
    public void SetCharacterClass(string classType)
    {
        PlayerMeleeWeapon playerMeleeWeapon;
        ///
        /// logic to actually pick the class here.
        /// 
        switch(classType)
        {
            

            case "Mage Class":
            
            classTypeCached = classType;
            characterClass = new MageClass();
            characterClassHandler.ClassDefaultInstantiation("Mage Class");
            //Debug.Log("Projectile damage" + projectile.damage);
            //magicDamage = projectile.damage;
            characterAbilities.AssignAbilities(classType);
            abilitiesUI.UpdateAbilitiesUI();
            Debug.Log(magicDamage);

            ApplyStatCalculation();
            // TODO: ASSIGN WEAPON
            Debug.Log("Strength: " + characterClass.strength);
            Debug.Log("Intelligence: " + characterClass.intelligence);
            // healthMultiplier;
            // speedMultiplier;

            break;
            
            case "Rogue Class":

            classTypeCached = classType;
            Debug.Log("You are a Rogue now");
            characterClass = new RogueClass();
            characterClassHandler.ClassDefaultInstantiation("Rogue Class");

            playerMeleeWeapon = GetComponentInChildren<PlayerMeleeWeapon>();
            meleeShortDamage = playerMeleeWeapon.damage;

            characterAbilities.AssignAbilities(classType);
            abilitiesUI.UpdateAbilitiesUI();
            Debug.Log(meleeShortDamage);

            ApplyStatCalculation();
            // TODO: ASSIGN WEAPON
            Debug.Log("Strength: " + characterClass.strength);
            Debug.Log("Intelligence: " + characterClass.intelligence);
            break;

            case "Warrior Class":

            classTypeCached = classType;
            Debug.Log("You are a Warrior now");
            characterClass = new WarriorClass();
            characterClassHandler.ClassDefaultInstantiation("Warrior Class");

            playerMeleeWeapon = GetComponentInChildren<PlayerMeleeWeapon>();
            meleeDamage = playerMeleeWeapon.damage;

            characterAbilities.AssignAbilities(classType);
            abilitiesUI.UpdateAbilitiesUI();
            Debug.Log(meleeDamage);

            ApplyStatCalculation();
            // TODO: ASSIGN WEAPON
            Debug.Log("Strength: " + characterClass.strength);
            Debug.Log("Intelligence: " + characterClass.intelligence);
            break;

            default:
            characterClass = new BaseClass();
            break;
        }

    }

    void HandleLevelUp()
    {
        if (characterClass != null)
        {
            characterClass.ApplyLevelUp(characterExperience.characterLevel);
            StatUpgrade();
            ApplyStatCalculation();
            UpdateBars();
            Debug.Log("Strength: " + characterClass.strength);
            Debug.Log("Const: " + characterClass.constitution);
        }
    }

    public void LevelUpStat(int stat)
    {
        stat+=1;
    }
    void StatUpgrade()
    {
        levelUpPoints += 1f;

        //give level up point to player
        //they can use whenever
        //in UI reference character stat and send point back to stat class
    }
   public ManaBar manaBar;
   public void ApplyStatCalculation()
   {    
        manaBar.enabled = true;
        //FindObjectsByType("ManaBar");
        limiter = .1f;
        
        //TODO:
        criticalStrike = characterClass.criticalStrike * characterClass.criticalStrikeMultiplier;

        healthPoints = characterClass.constitution * characterClass.constitutionMultiplier;
        maxHealthPoints = characterClass.constitution * characterClass.constitutionMultiplier;
        healthRestoration = characterClass.constitution * (characterClass.constitutionMultiplier * limiter);

        //TODO:
        manaPoints = characterClass.intelligence * characterClass.intelligenceMultiplier; 
        maxManaPoints = characterClass.intelligence * characterClass.intelligenceMultiplier;
        manaRestoration = characterClass.intelligence * (characterClass.intelligenceMultiplier * limiter);
        magicDamage = characterClass.intelligence * (characterClass.intelligenceMultiplier * limiter);

        energyPoints = characterClass.endurance * characterClass.enduranceMultiplier;
        maxEnergyPoints = characterClass.endurance * characterClass.enduranceMultiplier;
        energyRestoration = characterClass.endurance * (characterClass.enduranceMultiplier * limiter);

        //TODO: 
        meleeDamage = characterClass.strength * characterClass.strengthMultiplier;

        //TODO: 
        meleeShortDamage = characterClass.dexterity * characterClass.dexterityMultiplier;
    
        float speedMult = 5f;
        float jumpAndSpringMult = 1.5f;
        //TODO: 
        characterSpeed = speedMult * (float)Math.Log((double)characterClass.agility, (double)characterClass.agilityMultiplier);
        defaultSpeed = speedMult * (float)Math.Log((double)characterClass.agility, (double)characterClass.agilityMultiplier);
        sprintSpeed = jumpAndSpringMult * speedMult * (float)Math.Log((double)characterClass.agility, (double)characterClass.agilityMultiplier);
        defaultJumpSpeed = speedMult * (float)Math.Log((double)characterClass.agility, (double)characterClass.agilityMultiplier);
        jumpSpeed = jumpAndSpringMult * speedMult * (float)Math.Log((double)characterClass.agility, (double)characterClass.agilityMultiplier);
        
        //TODO: create enemy detection system, seeing enemy through walls within a certain radius. red outline mesh.
        enemyDetection = characterClass.perception * characterClass.perceptionMultiplier;

        //TODO: 
        armorResistance = characterClass.armorResistance * characterClass.armorResistanceMultiplier;

        //TODO: 
        magicResistance = characterClass.magicResistance * characterClass.magicResistanceMultiplier;

        UpdateBars();
   }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyDetection);
    }
    // void DetectEnemies()
    // {
    //     float radius = 2f; // thickness of the "ray"
    //     float maxDistance = enemyDetection;
    //     LayerMask mask = LayerMask.GetMask("Enemy");
    
    // Ray ray = new Ray(transform.position, transform.forward); // from player forward
    // Creature hitCreature;
    // if (Physics.SphereCast(ray, radius, out RaycastHit hit, maxDistance, mask))
    //     {
    //         hitCreature = hit.collider.GetComponent<Creature>();
    //         if (hitCreature != null)
    //         {
    //             hitCreature.EnableOutline();
    //             StartCoroutine(StopOutlinePeriod(hitCreature));
    //         }
    //         Debug.Log("Enemy hit with SphereCast: " + hit.collider.name);
    //     }
    // }

    // IEnumerator StopOutlinePeriod(Creature creature)
    // {
    //     yield return new WaitForSeconds(3);
    //     if (creature != null)
    //     {
    //     creature.DisableOutline();
    //     }
    // }
    public float ApplyCriticalStrikeChance(float baseDamage)
    {
        float critDamage = 2f;

        if (Random.Range(0f, 100f) <= criticalStrike)
        {
       
        Debug.Log("Crit " + baseDamage * critDamage);
        return baseDamage * critDamage;

        }
        return baseDamage;
    }

    public void UpdateBars()
    {
        onHealthChanged?.Invoke(healthPoints,maxHealthPoints);
        onManaChanged?.Invoke(manaPoints,maxManaPoints);
        onEnergyChanged?.Invoke(energyPoints,maxEnergyPoints);
    }
    void Awake()
    {
        
        //SetCharacterClass(ui string passed in); // pass in string from ui

        

        characterClassHandler = GetComponentInChildren<CharacterClassHandler>();
        character = GetComponent<Character>();
        characterExperience = GetComponent<CharacterExperience>();
        characterAbilities = GetComponent<CharacterAbilities>();

        characterExperience.OnLevelUp += HandleLevelUp;

        manaPoints = maxManaPoints;
        energyPoints = maxEnergyPoints;
        healthPoints = maxHealthPoints;

        //UpdateBars();
    }

    public void TakeDamage(float damage, DamageType damageType)
    {
        float magicDamageReductionPercentage = 1 - magicResistance/100f;
        float armorDamageReductionPercentage = 1 - armorResistance/100f;

        switch(damageType)
        {
            case DamageType.Physical:
            damage = damage * armorDamageReductionPercentage;
            break;
            case DamageType.Magical:
            damage = damage * magicDamageReductionPercentage;
            break;
            default:
            Debug.Log("No type found..");
            break;
        }
        Debug.Log("Armor " + armorDamageReductionPercentage);
        Debug.Log("MR " + magicDamageReductionPercentage);
        Debug.Log(damage);

        healthPoints -= damage;
        healthPoints = Mathf.Clamp(healthPoints, 0, maxHealthPoints);
        onHealthChanged?.Invoke(healthPoints,maxHealthPoints);
    
        if(healthPoints <= 0) 
        {
            PlayerAnimationStateChanger playerAnimationStateChanger = GetComponent<PlayerAnimationStateChanger>();
            playerAnimationStateChanger.ChangeAnimation("CharacterDeath");

            StartCoroutine(WaitForDeath());
            IEnumerator WaitForDeath()
            {
                Debug.Log("Dead");
                yield return new WaitForSeconds(1f);
                StartCoroutine(TeleportToLossDungeon());
            }
            //wait death, game over screen, tp to shopkeeper
            //gameObject.SetActive(false);
            //Destroy(gameObject, 1f);
        }
    }
    IEnumerator TeleportToLossDungeon()
    {
        yield return new WaitForSeconds(2f);
        TeleportPlayerToLoserDungeon();
        
    }
    void TeleportPlayerToLoserDungeon()
    {
        if (PlayerController.Instance != null)
        {
            CharacterController controller = PlayerController.Instance.GetComponent<CharacterController>();
            //CharacterInput controller2 = PlayerController.Instance.GetComponent<CharacterInput>();
            GameObject lossTransform = new GameObject();
            Transform lossDungeonTransform = lossTransform.transform;
            lossDungeonTransform.position = new Vector3(-30.8f, -48.544f, 53.3913f);

            if (controller != null)
            {
                controller.enabled = false;
                //controller2.enabled = false;
                healthPoints = maxHealthPoints;
                UpdateBars();
                PlayerController.Instance.transform.position = lossDungeonTransform.position;
                controller.enabled = true;
            }
            else
            {
                PlayerController.Instance.transform.position = lossDungeonTransform.position;
            }

            Debug.Log("Player teleported to " + lossDungeonTransform.position);
        }
        else
        {
            Debug.LogWarning("PlayerController.Instance is null");
        }
    }
    void Update()
    {
        //OnDrawGizmosSelected();
        // DetectEnemies();
        RegenerateEnergy();
        RegenerateMana();
    }
    public void UseMana(float amount)
    {
        manaPoints -= amount;
        manaPoints = Mathf.Clamp(manaPoints, 0, maxManaPoints);
        onManaChanged?.Invoke(manaPoints,maxManaPoints);

        if (manaPoints <= 0)
        {
            Debug.Log("no more shoot");
        }
    }

    public void UseManaPotion(float amount)
    {
        if (manaPoints < maxManaPoints)
        {
            manaPoints += amount;
            if (manaPoints > maxManaPoints)
            {
                manaPoints = maxManaPoints;
            }
            Debug.Log("Mana potion used. Current Mana: " + manaPoints);
        }
        else
        {
            Debug.Log("Mana is already full!");
        }
    }
    public void UseEnergyPotion(float amount)
    {
        if (energyPoints < maxEnergyPoints)
        {
            energyPoints += amount;
            if (energyPoints > maxEnergyPoints)
            {
                energyPoints = maxEnergyPoints;
            }
            Debug.Log("energy potion used. Current energy: " + energyPoints);
        }
        else
        {
            Debug.Log("energy is already full!");
        }
    }
    public void UseHealthPotion(float amount)
    {
        if (healthPoints < maxHealthPoints)
        {
            healthPoints += amount;
            if (healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
            Debug.Log("Mana potion used. Current Mana: " + healthPoints);
        }
        else
        {
            Debug.Log("Mana is already full!");
        }
    }

    void RegenerateMana()
    {
        if (manaPoints < maxManaPoints)
        {
            manaPoints += manaRestoration * Time.deltaTime;
            manaPoints = Mathf.Clamp(manaPoints, 0, maxManaPoints);
            onManaChanged?.Invoke(manaPoints, maxManaPoints);
        }
        
    }

   
    public void UseEnergy(float amount)
    {
        energyPoints -= amount;
        energyPoints = Mathf.Clamp(energyPoints, 0, maxEnergyPoints);
        onEnergyChanged?.Invoke(energyPoints,maxEnergyPoints);

        if(energyPoints <= 0)
        {
            jumpSpeed = 5f;
            characterSpeed = 5f;
        }
    }

    void RegenerateEnergy()
    {
        if (energyPoints < maxEnergyPoints)
        {
            energyPoints += energyRestoration * Time.deltaTime;
            energyPoints = Mathf.Clamp(energyPoints, 0, maxEnergyPoints);
            onEnergyChanged?.Invoke(energyPoints, maxEnergyPoints);
        }
        if (energyPoints >= maxEnergyPoints)
        {
            jumpSpeed = defaultJumpSpeed; // or store original value in a separate variable if it can vary
            characterSpeed = defaultSpeed;    // same here
        }
    }

    void OnDestroy()
    {
        characterExperience.OnLevelUp -= HandleLevelUp;
    }

    public void GiveMoney(int creatureMoney)
    {
        characterMoney += creatureMoney;
    }
}