using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    [Range(0.1f,1f)]
    [SerializeField] private float slowdownTimePerc = 1;
    [SerializeField] private bool defaultActivation = true;
    
    public System.Func<bool> CanActivate;

    private void Start()
    {
        //default
        CanActivate = () => defaultActivation;
    }

    private void Update()
    {
        if (KeyMappings.GetChronoKey() && CanActivate())
        {
            Time.timeScale = slowdownTimePerc;
        }
        else
            Time.timeScale = 1;
    }
}
