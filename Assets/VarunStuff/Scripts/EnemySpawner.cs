using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using ObjectPooling;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private Camera camera;


    [SerializeField]
    private ObjectPool pool;

    [SerializeField]
    private LayerMask ArenaMask;

    [SerializeField]
    private Transform PlayerTransform;

    [SerializeField]
    private MeshRenderer walkableArea;

    [SerializeField]
    private EnemyNumberCreator enemyNumberCreator;

    int counter = 0;

    // Update is called once per frame
    void Update()
    {
        return;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var results = Physics.RaycastAll(ray, float.MaxValue, ArenaMask);
            foreach(var result in results)
            {
                CreateEnemyAt(result.point, counter);
            }
            
        }
    }

    public void CreateEnemyAt(Vector3 position, int value)
    {
        var poolable = pool.GetObject();
        if (poolable is EnemyBehavior ebh)
        {
            ebh.SetParameters(walkableArea, PlayerTransform);

            ebh.SetValue(value, enemyNumberCreator);
        }
        poolable.transform.position = position + new Vector3(0, poolable.transform.localScale.y / 2, 0);
    }
}
