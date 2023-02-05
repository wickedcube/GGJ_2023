using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Waves;


// [Serializable]
// public class SpawnTag
// {
//     public Transform targetTrans;
//     // public TargetMarker Identifier;
// }
public class WaveSpawner : MonoBehaviour
{
    public List<WaveFormation> waveSos;
    public DummyEnemy enemyPrefab;

    // private Dictionary<TargetMarker, Transform> spawnPositionsDict = new();
    public Action OnWaveFinished;
    private int activeWaveIdx = 0;
    private int activeEnemiesInWave = 0;
    private void Start()
    {
        // var temp = FindObjectsOfType<WavePointTagger>();
        // if (spawnPositionsDict == null)
        // {
        //     spawnPositionsDict = new();
        // }
        //
        // foreach (var VARIABLE in temp)
        // {
        //     if (spawnPositionsDict.ContainsKey(VARIABLE.targetMarker))
        //     {
        //         Debug.LogError($"{VARIABLE.targetMarker} is duplicated on {VARIABLE.transform}", VARIABLE.gameObject);
        //     }
        //     spawnPositionsDict.Add(VARIABLE.targetMarker, VARIABLE.transform);
        // }

        StartWave(activeWaveIdx);
    }


    public void StartWave(int idx, float delay = 0f)
    {
        if (waveSos.Count <= idx)
        {
            // Debug.LogError($"Count is too much");
            return;
        }

        StartCoroutine(SpawnWave(waveSos[idx], delay));
    }

    private IEnumerator SpawnWave(WaveFormation waveFormation, float delay)
    {
        var player = FindObjectOfType<PlayerController>();
        yield return new WaitForSeconds(delay);

        foreach (var internalWave in waveFormation.InternalWaves)
        {
            var temp = internalWave.fileName;
            var fullExportedData = JsonConvert.DeserializeObject<FullExportedData>(Resources.Load<TextAsset>($"{temp}").text);
            foreach (var VARIABLE in fullExportedData.fullData)
            {
                for (int i = 0; i < VARIABLE.Value.Count; i++)
                {
                    activeEnemiesInWave++;
                    var customVec = VARIABLE.Value[i];
                    var pos = new Vector3(customVec.x, customVec.y, customVec.z) * 2;
                    var enemy = Instantiate(enemyPrefab, player.transform.position + pos, Quaternion.identity);
                    enemy.Init(VARIABLE.Key);
                }
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
        
        StartWave(++activeWaveIdx, 5);
    }
}