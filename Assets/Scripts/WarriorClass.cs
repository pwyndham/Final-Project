using UnityEngine;

public class WarriorClass : BaseClass
{
    
    public override string classType => "Warrior Class";
    public int _strength  = 5;
    public int _intelligence  = 0;
    public int _dexterity  = 3;
    public int _constitution  = 5;
    public int _agility  = 3;
    public int _endurance  = 7;
    public int _perception  = 1;
    public int _armorResistance  = 6;
    public int _magicResistance  = 6;
    public int _criticalStrike  = 0;
    
    float highPriorityMultiplier = 1.3f; // multiplier for most correct stat
    float mediumPriorityMultiplier = 1.1f; // multiplier for second most correct stat
    float lowPriorityMultiplier = .9f; // multiplier for last most correct stat
    float baseMultiplierConstitution = 20f;
    float baseMultiplierStrength = 10f; 
    float baseMultiplierDexterity = 10f; 
    float baseMultiplierIntelligence = 10f; 
    float baseMultiplierAgility = 2f; 
    float baseMultiplierEndurance = 2f; 
    float baseMultiplierPerception = 5f; 
    float baseMultiplierArmorResistance = 4f; 
    float baseMultiplierMagicResistance = 4f; 
    float baseMultiplierCriticalStrike = 5f;  
    

    public override float constitutionMultiplier => baseMultiplierConstitution * highPriorityMultiplier;
    public override float strengthMultiplier => baseMultiplierStrength * highPriorityMultiplier;
    public override float dexterityMultiplier => baseMultiplierDexterity * lowPriorityMultiplier;
    public override float intelligenceMultiplier => baseMultiplierIntelligence * lowPriorityMultiplier;
    public override float agilityMultiplier => baseMultiplierAgility * lowPriorityMultiplier;
    public override float enduranceMultiplier => baseMultiplierEndurance * lowPriorityMultiplier;
    public override float perceptionMultiplier => baseMultiplierPerception * lowPriorityMultiplier;
    public override float armorResistanceMultiplier => baseMultiplierArmorResistance * highPriorityMultiplier;
    public override float magicResistanceMultiplier => baseMultiplierMagicResistance * mediumPriorityMultiplier;
    public override float criticalStrikeMultiplier => baseMultiplierCriticalStrike * lowPriorityMultiplier;
    
    public override int strength
    {
        get => _strength;
        protected set => _strength = value;
    }
    public override int intelligence
    {
        get => _intelligence;
        protected set => _intelligence = value;
    }
    public override int dexterity
    {
        get => _dexterity;
        protected set => _dexterity = value;
    }
    public override int constitution
    {
        get => _constitution;
        protected set => _constitution = value;
    }
    public override int agility
    {
        get => _agility;
        protected set => _agility = value;
    }
    public override int endurance
    {
        get => _endurance;
        protected set => _endurance = value;
    }
    public override int perception
    {
        get => _perception;
        protected set => _perception = value;
    }
    public override int armorResistance
    {
        get => _armorResistance;
        protected set => _armorResistance = value;
    }
    public override int magicResistance
    {
        get => _magicResistance;
        protected set => _magicResistance = value;
    }
    public override int criticalStrike
    {
        get => _criticalStrike;
        protected set => _criticalStrike = value;
    }

    public override void ApplyLevelUp(int level)
    {
        

        strength += 1;
        constitution += 1;
        armorResistance += 1;
        magicResistance += 1;
        endurance += 1;
  
        if (level % 4 == 0)
        {
        intelligence += 1;
        agility += 1;
        dexterity += 1;
        perception += 1;
        criticalStrike += 1;
            
        }

    }
}
