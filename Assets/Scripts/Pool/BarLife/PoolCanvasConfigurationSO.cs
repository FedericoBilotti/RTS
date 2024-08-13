using System;
using UnityEngine;

namespace Pool.BarLife
{
    public class PoolCanvasConfigurationSO : ScriptableObject
    {
        [SerializeField] private CanvasConfig[] _canvas;
        public CanvasConfig[] Canvas => _canvas;
    }
        
    [Serializable]
    public struct CanvasConfig
    {
        public Units.Visual.BarLife barLife;
        public string id;
        public int amount;
    }
}