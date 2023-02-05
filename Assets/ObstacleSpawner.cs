using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs;
    public Transform playerRef;
    private int currentCellX = -1;
    private int currentCellY = -1;
    private int chunkSize = 45;
    private HashSet<Vector2> spawnedChunkedCoOrds = new HashSet<Vector2>();
    int xCoOrd, zCoOrd;

    void Update()
    {
        xCoOrd = (int)(playerRef.position.x + (chunkSize / 2)) / chunkSize;
        zCoOrd = (int)(playerRef.position.z + (chunkSize / 2)) / chunkSize;
        if (currentCellX != xCoOrd && currentCellY != zCoOrd)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (spawnedChunkedCoOrds.Add(new Vector2(xCoOrd + i, zCoOrd + j)))
                    {
                        Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], new Vector3((xCoOrd + i)* chunkSize - chunkSize / 2, 0, (zCoOrd + j) * chunkSize - chunkSize / 2), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
                    }
                }
            }
        }
    }
}
