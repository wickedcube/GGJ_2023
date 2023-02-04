using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] private Image healthBar;
    [SerializeField] private Image comboBar;
    
    private Coroutine healthbarRoutine;
    private Coroutine combobarRoutine;
    
    private static PlayerHealthUI instance;
    public static PlayerHealthUI Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerHealthUI>();

            return instance;
        }
    }

    public void SetHealthPerc(float perc)
    {
        if(healthbarRoutine != null)
            StopCoroutine(healthbarRoutine);
        
        AnimateFill(healthBar, perc, healthbarRoutine);
    }

    public void SetComboMeterF(float perc)
    {
        if(combobarRoutine != null)
            StopCoroutine(combobarRoutine);
        
        AnimateFill(comboBar, perc, combobarRoutine);
    }
    
    private void AnimateFill(Image img, float number, Coroutine routine)
    {
        if(routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(AnimateFillRoutine(img, number));
    }
    
    IEnumerator AnimateFillRoutine(Image img, float number)
    {
        float timeStep = 0;
        float currValue = img.fillAmount;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.15f;
            img.fillAmount = Mathf.Lerp(currValue, number, timeStep);
            yield return new WaitForEndOfFrame();
        }
    }
}
