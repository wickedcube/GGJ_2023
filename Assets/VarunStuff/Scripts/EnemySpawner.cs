using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using ObjectPooling;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private ObjectPool pool;

    [SerializeField]
    private LayerMask ArenaMask;

    [SerializeField]
    private MeshRenderer walkableArea;
    


    int counter = 0;

    void Start() {
    }

    public void CreateEnemyAt(Vector3 position, int value)
    {
        Debug.LogError($"Sid :: Step 1 : Creating Enemy");
        var poolable = pool.GetObject();
        if (poolable is EnemyBehavior ebh)
        {
            ebh.SetParameters(walkableArea);

            ebh.SetValue(value, true);
        }

        poolable.transform.position = position;
    }
}
