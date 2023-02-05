using Enemy;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumberCreationTest : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField input;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private EnemyBehavior behavior = default;

    public void OnButtonClick()
    {
        if(behavior != null)
        {
            behavior.ReturnToPool();
            behavior = default;
        }

        int result = 1;
        int.TryParse(input.text, out result);

        enemySpawner.CreateEnemyAt(Vector3.zero, result);

        behavior = FindObjectOfType<EnemyBehavior>();
    }
}
