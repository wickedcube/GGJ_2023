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
    
    private EnemyNumberCreator enemyNumberCreator;

    int counter = 0;

    void Awake() {
        enemyNumberCreator = GetComponent<EnemyNumberCreator>();
    }

    public void CreateEnemyAt(Vector3 position, int value)
    {
        var poolable = pool.GetObject();
        if (poolable is EnemyBehavior ebh)
        {
            ebh.SetParameters(walkableArea);

            ebh.SetValue(value, enemyNumberCreator);
        }

        poolable.transform.position = position;
    }
}
