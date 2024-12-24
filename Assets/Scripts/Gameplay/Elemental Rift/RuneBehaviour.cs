using UnityEngine;

namespace ElementalRift {
    [RequireComponent(typeof(ParticleSystemForceField))]
    [RequireComponent(typeof(SphereCollider))]
    public class RuneBehaviour : MonoBehaviour {

        public ElementType ElementType;

        private ParticleSystemForceField _forceField;

        private void Awake() {
            _forceField = GetComponent<ParticleSystemForceField>();
            _forceField.endRange = 3.0f;

            SphereCollider collider = GetComponent<SphereCollider>();
            collider.isTrigger = true;

            ElementData elementData = ElementManager.Instance.GetElementData(ElementType);
            gameObject.layer = ToSingleLayer(elementData.LayerMask);
        }

        private int ToSingleLayer(LayerMask mask) {
            int value = mask.value;
            if (value == 0) return 0;
            for (int l = 1; l < 32; l++) {
                if ((value & (1 << l)) != 0) {
                    return l;
                }
            }
            return -1;
        }
    }
}