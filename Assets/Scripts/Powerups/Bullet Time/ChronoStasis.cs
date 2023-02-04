using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoStasis : MonoBehaviour
{
    [Range(1, PlayerStats.MAX_COMBO_METER)] 
    [SerializeField] private int minComboToActivate = 4;
    
    [Range(0.1f,1f)]
    [SerializeField] private float slowdownTimePerc = 1;
    
    private PlayerStats stats;
    private bool activated = false;
    
    public System.Func<bool> CanActivate;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        //default
        CanActivate = () => stats.ComboMeterValue >= minComboToActivate;
    }

    private void Update()
    {
        if (KeyMappings.GetChronoKeyDown() && CanActivate())
        {
            Debug.Log("Here we go");
            ChronoHelper.OnChronoEffectStarted?.Invoke(slowdownTimePerc);
            RunChronoStasis();
        }
    }

    private void RunChronoStasis()
    {
        StartCoroutine(ChronoStatis());
    }

    IEnumerator ChronoStatis()
    {
        stats.ComboMeterLocked = true;
        float timeStep = 0;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / minComboToActivate;
            stats.ConsumeCombo(Time.deltaTime * PlayerStats.COMBO_STEP);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        stats.ComboMeterLocked = false;
    }
}
