using UnityEngine;

namespace ElementalRift {
    [CreateAssetMenu(fileName = "RuneData", menuName = "ScriptableObjects/Elemental Rift/Rune Data")]
    public class RuneData : ScriptableObject {

        [Tooltip("The required number of particles to imbue the rune")]
        public int RequiredParticles = 50;

        [Tooltip("The radius of attraction of the rune")]
        public float AttractionRadius = 3.0f;
    }
}