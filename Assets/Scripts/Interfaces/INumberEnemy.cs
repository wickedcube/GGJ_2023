using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface INumberEnemy
    {
        int Value { get; }
        void SetValue(int val);

        bool CanTakeDamage(object obj);

        bool TakeDamage(object obj);
    }
}
