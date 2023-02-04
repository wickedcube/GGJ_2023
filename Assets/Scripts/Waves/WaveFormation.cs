using System;
using System.Collections.Generic;
using UnityEngine;

namespace Waves
{
    [CreateAssetMenu(fileName = "new WaveFormation", menuName = "Sid/WaveFormation", order = 0)]
    public class WaveFormation : ScriptableObject
    {
        public List<InternalWave> InternalWaves;
    }

    [Serializable]
    public class InternalWave
    {
        public string fileName;
        public float timeTillNextInternalWave;
    }
}