using UnityEngine;

public class CharacterExperienceManager : MonoBehaviour
{
    public static CharacterExperienceManager Instance { get; private set; }

    /// <summary>
    /// public CharacterExperience characterExperience;
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject); // Optional: enforce singleton
    }

    
}