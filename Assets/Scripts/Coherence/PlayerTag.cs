using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    private PlayerStats parentStats;
    private static readonly int HealthHeight = Shader.PropertyToID("_HealthHeight");

    private void Update()
    {
        if (parentStats == null)
        {
            parentStats = GetComponentInParent<PlayerTag>().GetComponent<PlayerStats>();
        }

        var health = parentStats.health / 100f;
        var actualModel = transform.root.Find("characterMedium");
        if (actualModel == null)
        {
            Debug.LogError($"Model not found!");
        }
        else
        {
            actualModel.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat(HealthHeight, health);
        }

    }
}
