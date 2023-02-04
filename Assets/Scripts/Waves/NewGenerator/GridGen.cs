using System;
using System.Collections.Generic;
using UnityEngine;

namespace Waves
{
    public class GridGen : MonoBehaviour
    {
        private Color unSelectedCol = Color.green;
        private Color selectedColor = Color.red;

        public GameObject prefab;
        public int gridSize = 10;
        public float spacing = 2.0f;

        public Vector3 GetGridCenter => new Vector3(((gridSize - 1) * spacing / 2), 0, ((gridSize - 1) * spacing / 2));
        void Start()
        {
            var camPos = (gridSize - 1) * spacing / 2;
            var vecPos = new Vector3(camPos, 10, camPos);
            Camera.main.transform.position = vecPos;
            
            var og = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            og.transform.position = new Vector3(camPos, 0, camPos);
            og.transform.localScale = Vector3.one * 0.1f;
            Destroy(og.GetComponentInChildren<Collider>());
            
            
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Vector3 position = new Vector3(x * spacing, 0, y * spacing);
                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }

        private List<GameObject> selectedGameObjects = new ();

       
    }
}