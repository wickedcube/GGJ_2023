using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject firstFlowMenu;
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button controlsButton;


    [SerializeField] private TMP_Text playerRankText;
    [SerializeField] private TMP_Text playerScoreText;

    [Space(20)]

    [SerializeField] private TMP_Text usernameLog;
    [SerializeField] private Button updateUsername;
    [SerializeField] private TMP_InputField usernameField;

    [Space(20)]

    [SerializeField] private Transform leaderboardHolder;
    [SerializeField] private GameObject leaderboardEntryPrefab;

    [Space(20)]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject controlsPanel;


    void Start()
    {
        DOTween.Init();
        if (!PlayerPrefs.HasKey(LeaderboardHandler.USERNAME_PREF))
        {
            // first time flow
            SetupFirstFlow();
        }
        else
        {
            SetupGame();
        }
    }


    public void UpdateUsername()
    {
        LeaderboardHandler.Instance.UpdateUsername(usernameField.text, () =>
        {
            usernameLog.text = "Username added";
            SetupGame();
        },
        () =>
        {
            usernameLog.text = "Username unavailable :(\r\nPick Another one...!";
        });
    }

    public void SetupFirstFlow()
    {
        firstFlowMenu.SetActive(true);
        playButton.gameObject.SetActive(false);
        creditButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
    }

    public void SetupGame()
    {
        firstFlowMenu.SetActive(false);
        menu.SetActive(true);
        playButton.gameObject.SetActive(true);
        creditButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey(LeaderboardHandler.USERNAME_PREF))
        {
            LeaderboardHandler.Instance.PlayerData.username = PlayerPrefs.GetString(LeaderboardHandler.USERNAME_PREF);
            LeaderboardHandler.Instance.PlayerData.highscore = PlayerPrefs.GetInt(LeaderboardHandler.SCORE_PREF);
            //playerUsernameText.text = LeaderboardHandler.Instance.PlayerData.username;
            if(LeaderboardHandler.Instance.PlayerData.highscore == 0)
            {
                playerScoreText.text = "-";
                playerRankText.text = "-";
            }
            else
            {
                playerScoreText.text = LeaderboardHandler.Instance.PlayerData.highscore.ToString();
                //playerScoreText.text = LeaderboardHandler.Instance.PlayerData.highscore.ToString();
            }
        }

        LeaderboardHandler.Instance.UpdateLeaderboardUI(() =>
        {
            GameObject lbObj = null;

            for (int i = 0; i < LeaderboardHandler.Instance.Leaderboard.Count; i++)
            {
                LeaderboardData item = LeaderboardHandler.Instance.Leaderboard[i];
                if (item.username == LeaderboardHandler.Instance.PlayerData.username && item.highscore > 0)
                {
                    LeaderboardHandler.Instance.PlayerData.highscore = item.highscore;
                    playerScoreText.text = item.highscore.ToString();
                    playerRankText.text = $"{i + 1}";
                }
                if (item.highscore <= 0) continue;
                lbObj = Instantiate(leaderboardEntryPrefab, leaderboardHolder);
                if (item.username == LeaderboardHandler.Instance.PlayerData.username)
                {
                    lbObj.transform.Find("myScore").gameObject.SetActive(true);
                }
                lbObj.transform.Find("rank").GetComponent<TMP_Text>().text = $"{i + 1}";
                lbObj.transform.Find("username").GetComponent<TMP_Text>().text = item.username;
                lbObj.transform.Find("highscore").GetComponent<TMP_Text>().text = item.highscore.ToString();
            }
        });
    }


    public void ShowControls(bool show)
    {
        if (show)
        {
            controlsPanel.transform.DOShakePosition(1f, 3);
        }
        else
        {
            leaderboardPanel.transform.DOShakePosition(1f, 3);
        }
        leaderboardPanel.SetActive(!show);
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(show);
    }

    public void ShowCredits(bool show)
    {
        if (show)
        {
            creditsPanel.transform.DOShakePosition(1f, 3);
        }
        else
        {
            leaderboardPanel.transform.DOShakePosition(1f, 3);
        }
        leaderboardPanel.SetActive(!show);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(show);
    }

    public void Shake(GameObject gameObject)
    {
        gameObject.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, -0.1f), 0.5f,5,0.2f);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
}
