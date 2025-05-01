using Unity.Collections;
using UnityEngine;

public class DungeonData : MonoBehaviour
{
    
    public string dungeonName;
    public DungeonType dungeon;
    public int maxLevel;
    public GameObject[] enemyPrefabs;
    CharacterLevelManager characterLevel;


public enum DungeonType
{
    Ice,
    Earth,
    Fire
}
}

