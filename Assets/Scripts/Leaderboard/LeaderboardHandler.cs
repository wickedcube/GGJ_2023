using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardHandler : MonoBehaviour
{
    public static string USERNAME_PREF = "username";
    public static string SCORE_PREF = "highscore";
    private const string GET_LEADERBOARD_DATA = "https://easyprep.co.in/_rutvij/jam_leaderboard/GetLeaderboardData.php";
    private const string UPDATE_LEADERBOARD_DATA = "https://easyprep.co.in/_rutvij/jam_leaderboard/UpdateLeaderboardData.php";
    private const string UPDATE_USERNAME = "https://easyprep.co.in/_rutvij/jam_leaderboard/UpdateUsername.php";

    [SerializeField] private LeaderboardData playerData;
    [SerializeField] private List<LeaderboardData> leaderboard;

    public LeaderboardData PlayerData => playerData;
    public List<LeaderboardData> Leaderboard => leaderboard;
    public static LeaderboardHandler Instance;
    // Start is called before the first frame update


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        LeaderboardHandler.Instance.PlayerData.username = PlayerPrefs.GetString(LeaderboardHandler.USERNAME_PREF);
        LeaderboardHandler.Instance.PlayerData.highscore = PlayerPrefs.GetInt(LeaderboardHandler.SCORE_PREF);
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
        if (PlayerPrefs.HasKey(LeaderboardHandler.USERNAME_PREF) && !string.IsNullOrWhiteSpace(playerData.username))
        {
            StartCoroutine(UpdateDataToLeaderboard());
        }
    }

    public void UpdateLeaderboardUI(System.Action onComplete)
    {
        StartCoroutine(GetLeaderboardData(onComplete));
    }

    private IEnumerator GetLeaderboardData(System.Action onComplete)
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
                leaderboard = leaderboard.OrderByDescending(entry => entry.highscore).ToList();
                onComplete?.Invoke();

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
            }
        }
    }


    public void UpdateUsername(string username, System.Action onComplete, System.Action onFailed)
    {
        playerData.username = username;
        PlayerPrefs.SetString(USERNAME_PREF, playerData.username);
        PlayerPrefs.SetInt(SCORE_PREF, 0);
        PlayerPrefs.Save();
        StartCoroutine(UpdateUsernameRoutine(onComplete,onFailed));
    }

    private IEnumerator UpdateUsernameRoutine(System.Action onComplete, System.Action onFailed)
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
                Debug.Log($"leaerboard update {request.downloadHandler.text}");
                if(request.downloadHandler.text == "success")
                {
                    onComplete?.Invoke();
                }
                else
                {
                    onFailed?.Invoke();
                }


            }
        }
    }


}
