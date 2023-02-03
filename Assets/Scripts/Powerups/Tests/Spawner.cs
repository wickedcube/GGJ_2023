using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float forceMagnitude = 1f;
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            var go = Instantiate(prefab, transform.position, quaternion.identity);
            var rb = go.GetComponentInChildren<Rigidbody>();
            rb.AddForce(new Vector3(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)) * forceMagnitude);
        }
    }
}
