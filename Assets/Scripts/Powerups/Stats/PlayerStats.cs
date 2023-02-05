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

    [SerializeField] private List<int> comboKillThresholds;
    [SerializeField] private List<int> comboMulitplierVals;

    private int currentComboIndex;
    private int killCount;
    private int currentMeter;

    private void Start()
    {
        health = maxHealth;
        comboMeter = 0;

        PlayerHealthUI.Instance.SetHealthPerc((float)health / maxHealth);
        PlayerHealthUI.Instance.SetComboMeterF((float)comboMeter / MAX_COMBO_METER);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            AddToCombo(100f);

        AddToCombo(Time.deltaTime * MAX_COMBO_METER / 30f);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        ResetKillCounter();

        if (health <= 0)
            HealthDepleted?.Invoke();

        PlayerHealthUI.Instance.SetHealthPerc((float)health / maxHealth);
    }

    public void ConsumeCombo(float amt)
    {
        comboMeter = Mathf.Clamp(comboMeter - amt, 0, MAX_COMBO_METER);
        PlayerHealthUI.Instance.SetComboMeterF(comboMeter / MAX_COMBO_METER);
    }

    public void AddToCombo(float amt)
    {
        if (ComboMeterLocked)
            return;

        comboMeter = Mathf.Clamp(comboMeter + amt, 0, MAX_COMBO_METER);
        PlayerHealthUI.Instance.SetComboMeterF(comboMeter / MAX_COMBO_METER);
    }

    public void IncrementKillValue()
    {
        killCount++;
        if (currentComboIndex < comboKillThresholds.Count)
        {
            if (killCount > comboKillThresholds[currentComboIndex + 1])
            {
                currentComboIndex++;
            }
        }

        AddToCombo(comboMulitplierVals[currentComboIndex]);
    }

    public void ResetKillCounter()
    {

        killCount = 0;
        currentComboIndex = 0;
    }
}