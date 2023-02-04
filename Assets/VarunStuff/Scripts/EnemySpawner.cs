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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var poolable = pool.GetObject();
            if(poolable is EnemyBehavior ebh)
            {
                ebh.SetParameters(walkableArea, PlayerTransform);
            }

            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var results = Physics.RaycastAll(ray, float.MaxValue, ArenaMask);
            foreach (var result in results)
            {
                poolable.transform.position = result.point + new Vector3(0, poolable.transform.localScale.y / 2, 0);
                return;
            }
        }
    }
}
