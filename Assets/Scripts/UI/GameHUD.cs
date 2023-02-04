using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button back;
    [SerializeField] private GameObject playerUI;

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
}
