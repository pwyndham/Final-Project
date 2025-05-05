using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;


public class CharacterAbilities : MonoBehaviour
{

    public List<Ability> abilities = new List<Ability>();
    Ability ability;
    public void AssignAbilities(string classType)
    {
        switch(classType)
        {
            case "Warrior Class":
            
            Ability warriorBasic = gameObject.AddComponent<WarriorBasicAttack>();
            Ability ragingSlashes = gameObject.AddComponent<RagingSlashes>();
            Ability forcefield = gameObject.AddComponent<Forcefield>();

            abilities.Add(warriorBasic);
            abilities.Add(ragingSlashes);
            abilities.Add(forcefield);

            Debug.Log(abilities.ToString());
            Debug.Log("Warrior abilities are assigned");
            break;
            case "Mage Class":

            Ability mageBasicAttack = gameObject.AddComponent<MageBasicAttack>();
            Ability lightshot = gameObject.AddComponent<LightShot>();
            Ability lightbeam = gameObject.AddComponent<LightBeam>();

            abilities.Add(mageBasicAttack);
            abilities.Add(lightshot);
            abilities.Add(lightbeam);
            Debug.Log("Mage abilities are assigned");
            break;
            case "Rogue Class":

            Ability rogueBasicAttack = gameObject.AddComponent<RogueBasicAttack>();
            Ability dash = gameObject.AddComponent<Dash>();
            Ability frenzy = gameObject.AddComponent<Frenzy>();

            abilities.Add(rogueBasicAttack);
            abilities.Add(dash);
            abilities.Add(frenzy);
            Debug.Log("Rogue abilities are assigned");
            break;
            default:
            Debug.Log("No class found");
            break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && abilities.Count > 0)
        {
            abilities[0].Activate();
        }
        if (Input.GetKeyDown(KeyCode.Q) && abilities.Count > 1)
        {
            abilities[1].Activate();
        }
        if (Input.GetKeyDown(KeyCode.E) && abilities.Count > 2)
        {
            abilities[2].Activate();
        }
    }

    // private void UseAbility()
    // {
    //     foreach (Ability a in abilities)
    //     {
    //         a.Activate();
    //     }
    // }
}

