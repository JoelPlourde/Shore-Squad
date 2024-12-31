using Ambience;
using StatusEffectSystem;
using UnityEngine;

namespace ElementalRift {
    [CreateAssetMenu(fileName = "RiftData", menuName = "ScriptableObjects/Elemental Rift/Rift Data")]
    public class RiftData : ScriptableObject {

        [Tooltip("The maximum health of an elemental rift")]
        [SerializeField]
        public int MaximumHealth = 200;

        [Tooltip("The minimum percentage of health before the Rift collapses")]
        public float MinimumHealthPercentage = 0.25f;
        
        [Tooltip("The radius of influence of the rift")]
        [SerializeField]
        public float InfluenceRadius = 10.0f;

        [Tooltip("The primary element of the Rift")]
        public ElementType PrimaryElement;

        [Tooltip("The secondary element of the Rift")]
        public ElementType SecondaryElement;

        [Header("Ambient Effects")]
        [Tooltip("The temperature zone of the Rift")]
        public TemperatureZoneData TemperatureZoneData;

        [Tooltip("The ambience zone of the Rift")]
        public AmbienceZoneData AmbienceZoneData;
    }
}