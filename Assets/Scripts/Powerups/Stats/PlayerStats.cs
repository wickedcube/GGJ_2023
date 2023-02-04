using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    
    
    [SerializeField]
    private int health;
    
    [Header("Measured in Count")]
    [SerializeField] private int maxGrenades = 4;
    [SerializeField] private int grenadesLeft;
    
    [Header("Measured in seconds")]
    [SerializeField] private int maxChronosTime = 5;
    [SerializeField] private float chronoStatisLeft;

    public System.Action HealthDepleted;

    public int GrenadesLeft => grenadesLeft;
    public float ChronoStatisLeft => chronoStatisLeft;

    private void Start()
    {
        chronoStatisLeft = 0;
        grenadesLeft = 0;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
            HealthDepleted?.Invoke();
    }

    public void GrantGrenade(int qty)
    {
        grenadesLeft = Mathf.Clamp(grenadesLeft + qty, 0, maxGrenades);
    }
    
    public void ConsumeGrenade()
    {
        grenadesLeft = Mathf.Clamp(grenadesLeft - 1, 0, maxGrenades);
    }

    public void TopupChronoMeter(float topUpValue)
    {
        chronoStatisLeft = Mathf.Clamp(chronoStatisLeft + topUpValue, 0, maxChronosTime);
    }
    
    public void ConsumeChronoMeter(float amt)
    {
        chronoStatisLeft = Mathf.Clamp(chronoStatisLeft - amt, 0, maxChronosTime);
    }
}
