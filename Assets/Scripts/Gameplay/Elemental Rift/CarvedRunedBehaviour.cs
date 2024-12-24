using UnityEngine;

namespace ElementalRift {
    [RequireComponent(typeof(ParticleSystemForceField))]
    [RequireComponent(typeof(SphereCollider))]
    public class CarvedRunedBehaviour: MonoBehaviour {

        public ElementType ElementType;

        private ParticleSystemForceField _forceField;

        private void Awake() {
            _forceField = GetComponent<ParticleSystemForceField>();
            _forceField.endRange = 3.0f;

            SphereCollider collider = GetComponent<SphereCollider>();
            collider.isTrigger = true;

            UpdateLayerMask();
        }

        private void UpdateLayerMask() {
            string layerMask = "Default";
            switch (ElementType) {
                case ElementType.FIRE:
                    layerMask = "Fire Element";
                    break;
                case ElementType.WATER:
                    layerMask =  "Water Element";
                    break;
                case ElementType.EARTH:
                    layerMask =  "Earth Element";
                    break;
                case ElementType.AIR:
                    layerMask =  "Air Element";
                    break;
                case ElementType.LIFE:
                    layerMask =  "Life Element";
                    break;
                case ElementType.DEATH:
                    layerMask =  "Death Element";
                    break;
            }

            gameObject.layer = LayerMask.NameToLayer(layerMask);
        }
    }
}