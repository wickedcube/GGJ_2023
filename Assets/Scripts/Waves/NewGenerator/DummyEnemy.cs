using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    public void Init(int num)
    {
        
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);
        
    }
}
