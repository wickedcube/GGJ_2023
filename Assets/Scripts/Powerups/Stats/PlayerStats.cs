using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    public const int MAX_COMBO_METER = 4000;
    public const int COMBO_STEP = 1000;
    
    [SerializeField] private int maxHealth = 100;
    
    [SerializeField] private int health;
    [SerializeField] private float comboMeter;
    
    public float ComboMeterValue => comboMeter;
    
    /// <summary>
    /// returns true is Locked by some entity.
    /// </summary>
    public bool ComboMeterLocked = false;
    
    /// <summary>
    /// Subscribe to get death events
    /// </summary>
    public System.Action HealthDepleted;

    private void Start()
    {
        health = maxHealth;
        comboMeter = 0;
        
        PlayerHealthUI.Instance.SetHealthPerc((float)health/maxHealth);
        PlayerHealthUI.Instance.SetComboMeterF((float)comboMeter/MAX_COMBO_METER);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
            AddToCombo(100f);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
            HealthDepleted?.Invoke();
        
        PlayerHealthUI.Instance.SetHealthPerc((float)health/maxHealth);
    }

    public void ConsumeCombo(float amt)
    {
        comboMeter = Mathf.Clamp(comboMeter - amt, 0, MAX_COMBO_METER);
        PlayerHealthUI.Instance.SetComboMeterF(comboMeter/MAX_COMBO_METER);
    }
    
    public void AddToCombo(float amt)
    {
        if (ComboMeterLocked)
            return;
        
        comboMeter = Mathf.Clamp(comboMeter + amt, 0, MAX_COMBO_METER);
        PlayerHealthUI.Instance.SetComboMeterF(comboMeter/MAX_COMBO_METER);
    }
}