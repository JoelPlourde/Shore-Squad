using UnityEngine;

namespace Ambience {
    [CreateAssetMenu(fileName = "AmbienceZoneData", menuName = "ScriptableObjects/Ambience/Ambience Zone Data")]
    public class AmbienceZoneData : ScriptableObject {

        [Tooltip("The color tint of the zone")]
        public Color Tint;
    }
}