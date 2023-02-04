using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    public void Init(int num)
    { 
        Debug.LogError($"Spawning enemy for number {num}");
        var t = FindObjectOfType<EnemySpawner>();
        t.CreateEnemyAt(transform.position, num);
    }
}
