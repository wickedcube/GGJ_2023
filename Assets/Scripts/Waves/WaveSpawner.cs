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

        StartWave(0);
    }


    public void StartWave(int idx)
    {
        if (waveSos.Count <= idx)
        {
            Debug.LogError($"Count is too much");
            return;
        }

        StartCoroutine(SpawnWave(waveSos[idx]));
    }

    private IEnumerator SpawnWave(WaveFormation waveFormation)
    {
        yield return null;

        foreach (var internalWave in waveFormation.InternalWaves)
        {
            var temp = internalWave.fileName;
            var fullExportedData = JsonConvert.DeserializeObject<FullExportedData>(Resources.Load<TextAsset>($"{temp}").text);
            foreach (var VARIABLE in fullExportedData.fullData)
            {
                for (int i = 0; i < VARIABLE.Value.Count; i++)
                {
                    var customVec = VARIABLE.Value[i];
                    var pos = new Vector3(customVec.x, customVec.y, customVec.z);
                    var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
                    enemy.Init(VARIABLE.Key);

                }
            }
            yield return new WaitForSeconds(internalWave.timeTillNextInternalWave);
        }
    }
}