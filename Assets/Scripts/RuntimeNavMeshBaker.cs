using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RuntimeNavMeshBaker : MonoBehaviour
{

    public NavMeshSurface surface;
    public SpawnPrefabGrid spawnPrefabGrid;

    void Start()
    {
        StartCoroutine(BakeWhenReady());
    }

    public bool IsBaked { get; private set; } = false;

IEnumerator BakeWhenReady()
{
    yield return new WaitUntil(() => spawnPrefabGrid.IsGenerationComplete);  // Wait for generation
    Debug.Log("[RuntimeNavMeshBaker] NavMesh generation complete.");

    yield return new WaitForSeconds(2f);  // Give extra time for the NavMesh to finalize
    surface.BuildNavMesh();  // Ensure NavMesh is built at this point
    IsBaked = true;  // Mark as baked
    BakeComplete();
}

public bool BakeComplete()
{
    Debug.Log("[RuntimeNavMeshBaker] NavMesh baking complete.");
    return IsBaked;
}

}