using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class ElTestEnemy : MonoBehaviour, INumberEnemy
{

    public int Value => 10;
    public void SetValue(int val)
    {
        Debug.Log("Oh Crap!!");
    }
}
