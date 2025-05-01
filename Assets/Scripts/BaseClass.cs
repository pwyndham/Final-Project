using UnityEngine;

public class BaseClass
{
    public virtual string classType => "Base Class";
    public virtual int strength { get; protected set; } = 0;
    public virtual int intelligence { get; protected set; } = 0;
    public virtual int dexterity { get; protected set; } = 0;
    public virtual int constitution { get; protected set; } = 0;
    public virtual int agility { get; protected set; } = 0;
    public virtual int endurance { get; protected set; } = 0;
    public virtual int perception { get; protected set; } = 0;
    public virtual int armorResistance { get; protected set; } = 0;
    public virtual int magicResistance { get; protected set; } = 0;
    public virtual int criticalStrike { get; protected set; } = 0;

    public virtual float constitutionMultiplier { get; protected set; } = 0;
     public virtual float strengthMultiplier { get; protected set; } = 0;
    public virtual float dexterityMultiplier { get; protected set; } = 0;
    public virtual float intelligenceMultiplier { get; protected set; } = 0;
    public virtual float agilityMultiplier { get; protected set; } = 0;
    public virtual float enduranceMultiplier { get; protected set; } = 0;
    public virtual float perceptionMultiplier { get; protected set; } = 0;
    public virtual float armorResistanceMultiplier { get; protected set; } = 0;
    public virtual float magicResistanceMultiplier { get; protected set; } = 0;
    public virtual float criticalStrikeMultiplier { get; protected set; } = 0;


    public virtual void IncreaseStrength(int amount)
    {
        strength += amount;
    }
    public virtual void IncreaseDexterity(int amount)
    {
        dexterity += amount;
    }
    public virtual void IncreaseIntelligence(int amount)
    {
        intelligence += amount;
    }
    public virtual void IncreaseConstitution(int amount)
    {
        constitution += amount;
    }
    public virtual void IncreaseAgility(int amount)
    {
        agility += amount;
    }
    public virtual void IncreaseEndurance(int amount)
    {
        endurance += amount;
    }
    public virtual void IncreasePerception(int amount)
    {
        perception += amount;
    }
    public virtual void IncreaseArmorResistance(int amount)
    {
        armorResistance += amount;
    }
    public virtual void IncreaseMagicResistance(int amount)
    {
        magicResistance += amount;
    }
    public virtual void IncreaseCriticalStrike(int amount)
    {
        criticalStrike += amount;
    }

    public virtual void ApplyLevelUp(int level){}
}
