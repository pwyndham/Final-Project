using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Ability : MonoBehaviour
{

    public string abilityName;
    public string abilityDescription;
    public int defaultCooldown;
    public int abilityCooldown;
    public Image abilityIcon;
    public abstract void Activate();

}
