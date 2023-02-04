using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LeaderboardHandler leaderboardHandler;


    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject firstFlowMenu;
    [SerializeField] private Button playButton;


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
        leaderboardHandler.UpdateUsername();
        SetupGame();
    }

    public void SetupFirstFlow()
    {
        firstFlowMenu.SetActive(true);
        menu.SetActive(false);
    }

    public void SetupGame()
    {
        firstFlowMenu.SetActive(false);
        menu.SetActive(true);
        leaderboardHandler.UpdateLeaderboardUI();
    }
}
