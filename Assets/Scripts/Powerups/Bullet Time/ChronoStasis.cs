using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoStasis : MonoBehaviour
{
    [Range(0.1f,1f)]
    [SerializeField] private float slowdownTimePerc = 1;
    [SerializeField] private bool defaultActivation = true;
    
    private bool activated = false;
    
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
            activated = true;
            ChronoHelper.OnChronoEffectStarted?.Invoke(slowdownTimePerc);
        }
        else if (activated)
        {
            activated = false;
            ChronoHelper.OnChronoEffectEnded?.Invoke();
        }
    }
}
