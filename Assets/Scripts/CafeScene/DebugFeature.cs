using UnityEngine;
using UnityEngine.UI;

public class DebugFeature : MonoBehaviour
{
    public GameObject npcPrefab;
    public GameObject mikaPrefab;


    public void SpawnNpc()
    {
        SpawnPrefab(npcPrefab);
    }

    public void SpawnMika()
    {
        SpawnPrefab(mikaPrefab);
    }

    private void SpawnPrefab(GameObject targetPrefab)
    {
        if (targetPrefab != null && CafeSceneManager.Instance.npcSpawnPosition != null)
        {
            GameObject spawnedNpc = Instantiate(targetPrefab, CafeSceneManager.Instance.npcSpawnPosition.position, Quaternion.identity);

            // NpcMover 컴포넌트에 필요한 값 설정
            NpcMover mover = spawnedNpc.GetComponent<NpcMover>();
            if (mover != null)
            {
                mover.SpawnPosition = CafeSceneManager.Instance.npcSpawnPosition;
            }

            Debug.Log("Spawned new NPC at " + CafeSceneManager.Instance.npcSpawnPosition.position);
        }
        else
        {
            Debug.LogError("NPC Prefab or Spawn Position not assigned!");
        }
    }
    

}