using System;
using Coherence;
using Coherence.Toolkit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source
{
    public class PlayerSpawner : MonoBehaviour
    {
        public Transform[] startPositions;
        public GameObject PlayerPrefab;
        public Transform cam;
        private GameObject instantiatedPlayer;

        public void OnConnectedToSomething()
        {
            if (SimulatorUtility.IsSimulator)
            {
                Debug.Log($"MonoBridge :: Simulator is connected");
            }
            else
            {
                Debug.Log($"MonoBridge :: Client is connected");
                instantiatedPlayer = Instantiate(PlayerPrefab);
                instantiatedPlayer.GetComponent<PlayerController>().Init(GetComponent<CoherenceMonoBridge>(), cam);
                instantiatedPlayer.transform.position = startPositions[Random.Range(0, startPositions.Length)].position;
                // instantiatedPlayer.layer = LayerMask.NameToLayer("LocalPlayer");

                var monoBridhe = GetComponent<CoherenceMonoBridge>();
                monoBridhe.ClientConnections.OnSynced += manager =>
                {
                    if (manager.ClientConnectionCount == 1)
                    {
                        // instantiatedPlayer.GetComponentInChildren<PlayerHealthUI>().SetWaitingTextStatus(true);
                    }
                    else
                    {
                        instantiatedPlayer.GetComponentInChildren<PlayerHealthUI>().SetWaitingTextStatus(false);
                    }
                };
            }
        }

        public void OnDisconnectedFromSomething()
        {
            if (SimulatorUtility.IsSimulator)
            {
                Debug.Log($"MonoBridge :: Simulator is DISconnected");
            }
            else
            {
                if (instantiatedPlayer)
                {
                    Destroy(instantiatedPlayer);
                }
            }
        }

        public void OnConnectionErrorInSomething()
        {
            Debug.Log($"MonoBridge :: {nameof(OnConnectionErrorInSomething)}");
        }
    }
}