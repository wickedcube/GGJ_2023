using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChronoStasis : MonoBehaviour
{
    [SerializeField] private Volume volume;
    
    [Range(1, PlayerStats.MAX_COMBO_METER)] 
    [SerializeField] private int minComboToActivate = 4;
    
    [Range(0.1f,1f)]
    [SerializeField] private float slowdownTimePerc = 1;
    
    private PlayerStats stats;
    private bool activated = false;
    
    public System.Func<bool> CanActivate;

    private AudioSource chronoSFX;
    
    private void Start()
    {
        chronoSFX = GetComponent<AudioSource>();
        stats = GetComponent<PlayerStats>();
        //default
        CanActivate = () => stats.ComboMeterValue >= minComboToActivate;
    }

    private void Update()
    {
        if (KeyMappings.GetChronoKeyDown())
        {
            Debug.Log("Here we go");
            RunChronoStasis();
        }
    }

    private void RunChronoStasis()
    {
        chronoSFX.Play();
        StartCoroutine(ChronoStatis());
    }

    IEnumerator ChronoStatis()
    {
        stats.ComboMeterLocked = true;
        float timeStep = 0;
        
        ChromaticAberration aberation;
        LensDistortion lensDistortion;

        volume.profile.TryGet(typeof(ChromaticAberration), out aberation);
        volume.profile.TryGet(typeof(LensDistortion),out lensDistortion);
        
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.5f;
            aberation.intensity.value = Mathf.Lerp(0, 1, timeStep);
            lensDistortion.intensity.value = Mathf.Lerp(0, -0.35f, timeStep);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        if (CanActivate())
        {
            ChronoHelper.OnChronoEffectStarted?.Invoke(slowdownTimePerc);
            
            timeStep = 0;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / (minComboToActivate / PlayerStats.COMBO_STEP);
                stats.ConsumeCombo(Time.deltaTime * PlayerStats.COMBO_STEP, true);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
            ChronoHelper.OnChronoEffectEnded?.Invoke();
        }
        
        timeStep = 0;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.5f;
            aberation.intensity.value = Mathf.Lerp(1, 0, timeStep);
            lensDistortion.intensity.value = Mathf.Lerp(-0.35f, 0, timeStep);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        
        stats.ComboMeterLocked = false;
    }
}
