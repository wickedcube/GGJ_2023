using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class LeaderboardHandler : MonoBehaviour
{
    public static string USERNAME_PREF = "username";
    public static string SCORE_PREF = "highscore";
    private const string GET_LEADERBOARD_DATA = "https://easyprep.co.in/_rutvij/jam_leaderboard/GetLeaderboardData.php";
    private const string UPDATE_LEADERBOARD_DATA = "https://easyprep.co.in/_rutvij/jam_leaderboard/UpdateLeaderboardData.php";
    private const string UPDATE_USERNAME = "https://easyprep.co.in/_rutvij/jam_leaderboard/UpdateUsername.php";

    [SerializeField] private LeaderboardData playerData;
    [SerializeField] private List<LeaderboardData> leaderboard;

    [Space(20)]

    [SerializeField] private TMP_Text playerUsernameText;
    [SerializeField] private TMP_Text playerScoreText;

    [Space(20)]

    [SerializeField] private TMP_Text usernameLog;
    [SerializeField] private Button updateUsername;
    [SerializeField] private TMP_InputField usernameField;

    [Space(20)]

    [SerializeField] private Transform leaderboardHolder;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    // Start is called before the first frame update
    void Start()
    {
        leaderboard = new List<LeaderboardData>();
        //UpdateLeaderboardUI();
    }

    public void UpdateScore(int score)
    {
        if(playerData.highscore < score)
        {
            playerData.highscore = score;
            PlayerPrefs.SetInt(SCORE_PREF, score);
            PlayerPrefs.Save();
        }
        StartCoroutine(UpdateDataToLeaderboard());
    }

    public void UpdateLeaderboardUI()
    {

        if (PlayerPrefs.HasKey(USERNAME_PREF))
        {
            playerData.username = PlayerPrefs.GetString(USERNAME_PREF);
            playerScoreText.text = playerData.highscore.ToString();
            playerUsernameText.text = playerData.username;
        }
        StartCoroutine(GetLeaderboardData());
    }

    private IEnumerator GetLeaderboardData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GET_LEADERBOARD_DATA))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"leaerboard fetch failed {request.error}");
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log("Received leaderboard JSON: " + json);
                leaderboard = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LeaderboardData>>(json);
                GameObject lbObj = null;
                leaderboard = leaderboard.OrderByDescending(entry => entry.highscore).ToList();
                
                for (int i = 0; i < leaderboard.Count; i++)
                {
                    LeaderboardData item = leaderboard[i];
                    if(item.username == playerData.username)
                    {
                        playerScoreText.text = item.highscore.ToString();
                        playerUsernameText.text = item.username;
                    }
                    if (item.highscore <= 0) continue;
                    lbObj = Instantiate(leaderboardEntryPrefab, leaderboardHolder);
                    if (item.username == playerData.username)
                    {
                        lbObj.transform.Find("myScore").gameObject.SetActive(true);
                    }
                    lbObj.transform.Find("rank").GetComponent<TMP_Text>().text = $"{i + 1}";
                    lbObj.transform.Find("username").GetComponent<TMP_Text>().text = item.username;
                    lbObj.transform.Find("highscore").GetComponent<TMP_Text>().text = item.highscore.ToString();
                }

            }
        }
    }

    private IEnumerator UpdateDataToLeaderboard()
    {

        //WWWForm form = new WWWForm();
        //form.AddField("username", myScore.username);
        //form.AddField("highscore", (int)myScore.highscore);

        string userData = JsonUtility.ToJson(playerData);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(userData);
        //using (UnityWebRequest request = UnityWebRequest.Put(UPDATE_LEADERBOARD_DATA, postData))
        using (UnityWebRequest request = UnityWebRequest.Put(UPDATE_LEADERBOARD_DATA, postData))
        {
            request.method = "POST";
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"leaerboard update failed {request.error}");
            }
            else
            {
                Debug.Log($"Record added successfully {request.downloadHandler.text}");
                StartCoroutine(GetLeaderboardData());
            }
        }
    }


    public void UpdateUsername()
    {
        playerData.username = usernameField.text;
        PlayerPrefs.SetString(USERNAME_PREF, playerData.username);
        PlayerPrefs.SetInt(SCORE_PREF, 0);
        PlayerPrefs.Save();
        StartCoroutine(UpdateUsernameRoutine());
    }

    private IEnumerator UpdateUsernameRoutine()
    {
        string userData = JsonUtility.ToJson(playerData);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(userData);
        using (UnityWebRequest request = UnityWebRequest.Put(UPDATE_USERNAME, postData))
        {
            request.method = "POST";
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"leaerboard update failed {request.error}");
            }
            else
            {
                if(request.downloadHandler.text == "success")
                {
                    usernameLog.text = "Username added";
                }
                else
                {
                    usernameLog.text = "Username unavailable";
                }
                Debug.Log($"leaerboard update {request.downloadHandler.text}");


            }
        }
    }


}
