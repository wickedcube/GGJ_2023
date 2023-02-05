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


    [SerializeField] private TMP_Text playerRankText;
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
        SceneManager.LoadScene(0);
    }


    public void ShowGameOverMenu(int score)
    {
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        playerUI.SetActive(false);

        LeaderboardHandler.Instance?.UpdateLeaderboardUI(() =>
        {
            playerScoreText.text = $"{score}";
            playerRankText.text = "-";
            
            for (int i = 0; i < LeaderboardHandler.Instance.Leaderboard.Count; i++)
            {
                LeaderboardData item = LeaderboardHandler.Instance.Leaderboard[i];
                if (item.username == LeaderboardHandler.Instance.PlayerData.username && item.highscore > 0)
                {
                    playerScoreText.text = item.highscore.ToString();
                    playerRankText.text = $"{i + 1}";
                }
            }
        });
    }
}
