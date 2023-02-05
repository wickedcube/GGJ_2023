using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Waves
{

    [Serializable]
    public struct CustomVector
    {
        public float x;
        public float y;
        public float z;

        public CustomVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    [Serializable]
    public class FullExportedData
    {
        public Dictionary<int, List<CustomVector>> fullData;
    }
    public class AddNumberButton : MonoBehaviour
    {
        public TMP_InputField exportFileText;
        public Transform contentParent;
        public TMP_InputField text;
        public Button button;
        
        private Dictionary<int, (Button, Color)> list = new();

        public float scale;
        public GridGen GridGen;
        private int activeNumber = -1;

        public void OnExportButtonClicked()
        {
            var fileName = exportFileText.text;
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError($"Filename is empty!!");
                return;
            }

            var temp = new FullExportedData();
            temp.fullData = new Dictionary<int, List<CustomVector>>();
            foreach (var kvp in pattern)
            {
                temp.fullData.Add(kvp.Key, new List<CustomVector>());
                foreach (var o in kvp.Value)
                {
                    var pos = (o.transform.position - GridGen.GetGridCenter) * scale;
                    temp.fullData[kvp.Key].Add(new CustomVector(pos.x,pos.y,pos.z));
                }
            }
            File.WriteAllText($"{Application.dataPath}/Jsons/Resources/{fileName}.json", JsonConvert.SerializeObject(temp));
            AssetDatabase.Refresh();
        }
        public void OnAddButtonClicked()
        {
            if (string.IsNullOrEmpty(text.text))
            {
                Debug.LogError($"text is empty");
                return;
            }

            if (int.TryParse(text.text, out int t))
            {
                if (list.ContainsKey(t))
                {
                    Debug.LogError($"Already contains {t}!");
                    return;
                }
                InstantiateButton(t);
            }
            else
            {
                Debug.LogError($"Not an nubmer!");
                return;
            }
        }

        private void InstantiateButton(int t)
        {
            var b = Instantiate(button, contentParent);
            var col = Random.ColorHSV();
            list.Add(t, (b, col));
            b.GetComponent<Image>().color = col;
            b.GetComponentInChildren<TextMeshProUGUI>().text = t.ToString();
            b.onClick.AddListener(() =>
            {
                ButtonClicked(t);
            });
        }

        private void ButtonClicked(int t)
        {
            activeNumber = t;
        }

        private Dictionary<int, List<GameObject>> pattern = new();
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var hitObject = hit.collider.gameObject;
                    RemoveFromOthers(hitObject);

                    if (!pattern.ContainsKey(activeNumber))
                    {
                        if (activeNumber == -1)
                        {
                            Debug.LogError($"Please select a button");
                            return;
                        }
                        pattern.Add(activeNumber, new List<GameObject>());   
                    }

                    pattern[activeNumber].Add(hitObject);
                    hitObject.GetComponentInChildren<Renderer>().material.color = list[activeNumber].Item2;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var hitObject = hit.collider.gameObject;
                    RemoveFromOthers(hitObject);
                    hitObject.GetComponentInChildren<Renderer>().material.color = Color.white;
                }
            }
        }

        private void RemoveFromOthers(GameObject hitObject)
        {
            foreach (var keyValuePair in pattern)
            {
                if (keyValuePair.Value.Contains(hitObject))
                {
                    keyValuePair.Value.Remove(hitObject);
                    return;
                }   
            }
        }
    }
}