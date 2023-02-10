using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Waves;
using Coherence.Connection;
using Coherence.Toolkit;

public class WaveSpawner : MonoBehaviour
{
     public CoherenceSync cSync;
    public List<WaveFormation> waveSos;
    public Action OnWaveFinished;
    private int activeWaveIdx = 0;
    private int activeEnemiesInWave = 0;
    private EnemySpawner enemySpawner;
    public CoherenceMonoBridge MonoBridge;
    int maxPlayerCount = 2;
    int currentPlayerCount = 0;
    bool isFirstClient = false;

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        MonoBridge.ClientConnections.OnCreated += connection =>
        {
            currentPlayerCount++;
            if (currentPlayerCount == maxPlayerCount && cSync.HasStateAuthority)
                StartWave(activeWaveIdx);
        };
    }


    public void StartWave(int idx, float delay = 0f)
    {
        Debug.LogError($"Starting a new wave {idx}");
        if (waveSos.Count <= idx)
        {
            // Debug.LogError($"Count is too much");
            return;
        }

        StartCoroutine(SpawnWave(waveSos[idx], delay));
    }

    private IEnumerator SpawnWave(WaveFormation waveFormation, float delay)
    {
        // var player = FindObjectOfType<PlayerController>();

        if (delay > 3)
        {
            yield return new WaitForSeconds(delay - 3f);
            PlayerHealthUI.Instance.ShowWaveIncomingText();
            yield return new WaitForSeconds(3f);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }



        var enemInWave = 0;
        foreach (var internalWave in waveFormation.InternalWaves)
        {
            var temp = internalWave.fileName;
            var fullExportedData = JsonConvert.DeserializeObject<FullExportedData>(Resources.Load<TextAsset>($"{temp}").text);
            foreach (var numericPattern in fullExportedData.fullData)
            {
                for (int i = 0; i < numericPattern.Value.Count; i++)
                {
                    enemInWave++;
                    var customVec = numericPattern.Value[i];
                    var pos = new Vector3(customVec.x, customVec.y, customVec.z) * 2;
                    // var pos = player.transform.position + new Vector3(customVec.x, customVec.y, customVec.z) * 2;
                    enemySpawner.CreateEnemyAt(pos, numericPattern.Key);
                }

                Debug.Log($"Adding {enemInWave} to enemy count");
                activeEnemiesInWave += enemInWave;
                enemInWave = 0;
            }

            yield return new WaitForSeconds(internalWave.timeTillNextInternalWave);
        }
    }

    public void EnemyDied()
    {
        activeEnemiesInWave--;
        if (activeEnemiesInWave == 0)
        {
            OnWaveFinished?.Invoke();
        }
        else
        {
            return;
        }

        Debug.LogWarning($"Starting a new wave!");
        StartWave(++activeWaveIdx, 5);
    }
}