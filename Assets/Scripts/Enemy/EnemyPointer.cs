using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyPointer : MonoBehaviour
    {
        private void Start()
        {
            FindObjectOfType<PointerManager>()?.Register(this);
        }


        private void OnBecameInvisible()
        {
            FindObjectOfType<PointerManager>()?.OnEnemyInvisible(this);
        }

        private void OnBecameVisible()
        {
            FindObjectOfType<PointerManager>()?.OnEnemyVisible(this);
        }

        private void OnDisable()
        {
            FindObjectOfType<PointerManager>()?.DeRegister(this);
        }
    }
}