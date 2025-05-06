using UnityEngine;

public class Loot : MonoBehaviour
{
    public int lootAmount = 50;
    CharacterStats characterStats;

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            characterStats = player.GetComponent<CharacterStats>();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        characterStats.GiveMoney(lootAmount);
        Destroy(gameObject);
    }
    
}