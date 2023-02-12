using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button back;
    [SerializeField] private GameObject playerUI;


    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text playerScoreText;

    private static GameHUD instance;
    public static GameHUD Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameHUD>();

            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PauseGame(bool pause)
    {
        pauseMenu.SetActive(pause);
        playerUI.SetActive(!pause);
        back.gameObject.SetActive(!pause);
        if (pause)
        {
            //Time.timeScale = 0f;
        }
        else
        {
            //Time.timeScale = 1.0f;
        }
    }

    public void LoadMainMenu()
    {
        //Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);

        var player = GetComponentInParent<PlayerController>();
        if (player != null && player.MonoBridge != null)
        {
            player.MonoBridge.Disconnect();
        }
    }


    public void ShowGameOverMenu(int score)
    {
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        playerUI.SetActive(false);

        playerScoreText.text = $"{score}";
        highScoreText.text = $"{PlayerPrefs.GetInt(LeaderboardHandler.SCORE_PREF)}";
    }
}
