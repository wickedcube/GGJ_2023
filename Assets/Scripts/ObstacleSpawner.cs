using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs;
    // public Transform playerRef;
    private int currentCellX = -1;
    private int currentCellY = -1;
    private int chunkSize = 45;
    private HashSet<Vector2> spawnedChunkedCoOrds = new HashSet<Vector2>();
    int xCoOrd, zCoOrd;
    WaitForSeconds waitTime = new WaitForSeconds(0.05f);

    void Start()
    {
        Random.InitState(42);
        StartCoroutine(FillUpWorld());
    }

    IEnumerator FillUpWorld()
    {
        for (int i = -5; i < 6; i++)
        {
            for (int j = -5; j < 6; j++)
            {
                Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], new Vector3((xCoOrd + i) * chunkSize - chunkSize / 2, 0, (zCoOrd + j) * chunkSize - chunkSize / 2), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
                yield return waitTime;
            }
        }
    }
}
