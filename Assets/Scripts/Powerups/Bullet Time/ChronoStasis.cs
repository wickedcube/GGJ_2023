using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoStasis : MonoBehaviour
{
    [Range(0.1f,1f)]
    [SerializeField] private float slowdownTimePerc = 1;


    private PlayerStats stats;
    private bool activated = false;
    
    public System.Func<bool> CanActivate;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        //default
        CanActivate = () => stats.ChronoStatisLeft > 0;
    }

    private void Update()
    {
        if (KeyMappings.GetChronoKey() && CanActivate())
        {
            activated = true;
            ChronoHelper.OnChronoEffectStarted?.Invoke(slowdownTimePerc);
            stats.ConsumeChronoMeter(Time.deltaTime);
        }
        else if (activated)
        {
            activated = false;
            ChronoHelper.OnChronoEffectEnded?.Invoke();
        }
    }
}
