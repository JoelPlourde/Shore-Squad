using UnityEngine;

namespace StatusEffectSystem {
    [CreateAssetMenu(fileName = "TemperatureZoneData", menuName = "ScriptableObjects/Status Effects/Temperature Zone Data")]
    public class TemperatureZoneData : ScriptableObject {
        
        public float Magnitude = 1.0f;

        [Range(-60f, 100f)]
        public float TargetTemperature = 20f;
    }
}