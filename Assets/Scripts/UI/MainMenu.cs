using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LeaderboardHandler leaderboardHandler;


    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject firstFlowMenu;
    [SerializeField] private Button playButton;


    [SerializeField] private TMP_Text playerRankText;
    [SerializeField] private TMP_Text playerScoreText;

    [Space(20)]

    [SerializeField] private TMP_Text usernameLog;
    [SerializeField] private Button updateUsername;
    [SerializeField] private TMP_InputField usernameField;

    [Space(20)]

    [SerializeField] private Transform leaderboardHolder;
    [SerializeField] private GameObject leaderboardEntryPrefab;



    void Start()
    {
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
        leaderboardHandler.UpdateUsername(usernameField.text, () =>
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
    }

    public void SetupGame()
    {
        firstFlowMenu.SetActive(false);
        menu.SetActive(true);
        playButton.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey(LeaderboardHandler.USERNAME_PREF))
        {
            LeaderboardHandler.Instance.PlayerData.username = PlayerPrefs.GetString(LeaderboardHandler.USERNAME_PREF);
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

        leaderboardHandler.UpdateLeaderboardUI(() =>
        {
            GameObject lbObj = null;

            for (int i = 0; i < LeaderboardHandler.Instance.Leaderboard.Count; i++)
            {
                LeaderboardData item = LeaderboardHandler.Instance.Leaderboard[i];
                if (item.username == LeaderboardHandler.Instance.PlayerData.username && item.highscore > 0)
                {
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
}
