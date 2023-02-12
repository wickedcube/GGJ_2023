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
    [SerializeField] private List<TMPro.TMP_Text> waveText;

    [SerializeField] private GameObject grenadeKeyMap;
    [SerializeField] private GameObject chronoStasis;

    [SerializeField] private GameObject waitingForOtherPlayer;
    
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

    public void ShowChronoStatisKey(bool show)
    {
        chronoStasis.gameObject.SetActive(show);
    }
    
    public void ShowGrenadeKey(bool show)
    {
        grenadeKeyMap.gameObject.SetActive(show);
    }

    public void ShowWaveIncomingText() 
    {
        StartCoroutine(WaveIncomingText());
    }

    public void SetWaitingTextStatus(bool toSet)
    {
        if (waitingForOtherPlayer != null)
        {
            waitingForOtherPlayer.SetActive(toSet);
        }
    }

    IEnumerator WaveIncomingText() 
    {
        waveText[0].gameObject.SetActive(true);
        waveText[1].gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            int k = i;
            waveText[0].text = waveText[1].text = " Wave incoming \n in  <size=120>"+ (3 - k).ToString() +"</size>...";
            yield return new WaitForSeconds(1f);
        }

        waveText[0].gameObject.SetActive(false);
        waveText[1].gameObject.SetActive(false);
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
