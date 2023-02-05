using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] private Image healthBar;
    [SerializeField] private Image comboBar;
    [SerializeField] private List<TMPro.TMP_Text> comboText;
    [SerializeField] private List<TMPro.TMP_Text> scoreText;

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

    public void SetComboMeterF(float perc, bool animate = false)
    {
        if(combobarRoutine != null)
            StopCoroutine(combobarRoutine);
        
        if(animate)
            AnimateFill(comboBar, perc, combobarRoutine);
        else
        {
            comboBar.fillAmount = perc;
        }
    }

    public void SetComboCounter(int val)
    {
        comboText[0].gameObject.SetActive(val != 0);
        comboText[1].gameObject.SetActive(val != 0);
        
        comboText[0].text = comboText[1].text = $"x{val}";
    }
    
    public void SetScoreCounter(int val)
    {
        scoreText[0].text = scoreText[1].text = $"Score: {val}";
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
