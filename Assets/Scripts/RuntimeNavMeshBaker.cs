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

    IEnumerator BakeWhenReady()
    {
        yield return new WaitUntil(() => spawnPrefabGrid.IsGenerationComplete);
       // yield return new WaitForSeconds(5f);
        //yield return new WaitForEndOfFrame();

        surface.BuildNavMesh();
        BakeComplete();
    }
    public bool BakeComplete()
    {
        Debug.Log("Bake complete");
        return true;
    }

}