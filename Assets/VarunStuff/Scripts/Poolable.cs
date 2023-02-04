using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public abstract class Poolable : MonoBehaviour
    {
        private ObjectPool Pool { set; get; }

        public void ReturnToPool()
        {
            if (this.Pool == default)
            {
                Debug.LogError($" No Poolable assigned {this.gameObject.name}");
                return;
            }
            OnReturnedToPool();

            Pool.ReturnPoolable(this);
        }

        public void SetPoolable(ObjectPool p) => this.Pool = p;

        protected abstract void OnReturnedToPool();
    }
}

