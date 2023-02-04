using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class ElTestEnemy : MonoBehaviour, INumberEnemy
{
    public int Value => 10;

    public bool CanTakeDamage(object obj)
    {
        throw new System.NotImplementedException();
    }

    public void SetValue(int val, EnemyNumberCreator creator)
    {
        throw new System.NotImplementedException();
    }

    public bool TakeDamage(object obj)
    {
        throw new System.NotImplementedException();
    }
}
