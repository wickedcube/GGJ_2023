using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class PointerManager : MonoBehaviour
    {
        public Dictionary<EnemyPointer, GameObject> dict = new();

        public GameObject Prefab;

        public void Register(EnemyPointer pointer)
        {
            if (dict.ContainsKey(pointer))
            {
                return;
            }

            dict.Add(pointer, InstantiateFor(pointer));
        }

        public void OnEnemyInvisible(EnemyPointer pointer)
        {
            dict[pointer].gameObject.SetActive(true);
        }

        public void OnEnemyVisible(EnemyPointer pointer)
        {
            dict[pointer].gameObject.SetActive(false);
        }

        public void DeRegister(EnemyPointer pointer)
        {
            if (dict.ContainsKey(pointer) && dict[pointer])
            {
                Destroy(dict[pointer]);    
            }
        }

        private GameObject InstantiateFor(EnemyPointer pointer)
        {
            return Instantiate(Prefab, transform);
        }


        private void Update()
        {
            foreach (var (key, value) in dict)
            {
                if (value != null)
                {
                    ManagePointer(key, value.GetComponent<RectTransform>());   
                }
            }
        }

        private void ManagePointer(EnemyPointer targetPosition, RectTransform pointerRect)
        {
            Vector3 toPosition = targetPosition.transform.position;
            Vector3 fromPosition = FindObjectOfType<PlayerController>().transform.position;

            var vec = Camera.main.WorldToViewportPoint(toPosition);


            if (IsInBounds(vec))
            {
                dict[targetPosition].gameObject.SetActive(false);
                return;
            }
            else
            {
                dict[targetPosition].gameObject.SetActive(true);
            }
            
            vec = new Vector3(GCV(vec.x), GCV(vec.y), GCV(vec.z));
            pointerRect.anchorMin = vec;
            pointerRect.anchorMax = vec;

            pointerRect.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg*Mathf.Atan2(vec.y - 0.5f, vec.x- 0.5f));
            // pointerRect.rotation.z = Mathf.Atan2(vec.y, vec.x);

            float GCV(float val)
            {
                return Mathf.Clamp(val, 0, 1);
            }

            bool IsInBounds(Vector3 vec)
            {
                if (vec.x < 1 && vec.y < 1 && vec.x > 0 && vec.y > 0)
                {
                    return true;
                }

                return false;
            }
            
            
            // fromPosition.z = 0f;
            // Vector3 dir = (toPosition - fromPosition).normalized;
            // float angle = 0;
            // pointerRect.localEulerAngles = new Vector3(0, 0, angle);
            //
            // float borderSize = 100f;
            //
            // Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            // bool isOffscreen = targetPositionScreenPoint.x <= borderSize ||
            //                    targetPositionScreenPoint.x >= Screen.width - borderSize ||
            //                    targetPositionScreenPoint.y <= borderSize ||
            //                    targetPositionScreenPoint.y >= Screen.height - borderSize;
            // Debug.Log(isOffscreen + " " + targetPositionScreenPoint);
            //
            // if (isOffscreen)
            // {
            //     Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            //     cappedTargetScreenPosition.x =
            //         Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
            //     cappedTargetScreenPosition.y =
            //         Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);
            //
            //     Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
            //     pointerRect.position = pointerWorldPosition;
            //     pointerRect.localPosition = new Vector3(pointerRect.localPosition.x, pointerRect.localPosition.y, 0f);
            // }
            // else
            // {
            //     Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(targetPositionScreenPoint);
            //     pointerRect.position = pointerWorldPosition;
            //     pointerRect.localPosition = new Vector3(pointerRect.localPosition.x, pointerRect.localPosition.y, 0f);
            // }
        }
    }
}