using ItemSystem;
using UnityEngine;

namespace ElementalRift {
    public class RuneBehaviour : MonoBehaviour {

        [Tooltip("The current particle count of the Rune")]
        public int ParticleCount = 0;

        [Tooltip("The RuneData associated to the Rune")]
        public RuneData RuneData;

        [Tooltip("The ItemData associated to the Rune")]
        public ItemData ItemData;

        [Tooltip("The elemental type of the Rune")]
        public ElementType ElementType;

        private void Awake() {
            if (ReferenceEquals(RuneData, null)) {
                Debug.LogError("You must provide a Rune Data.");
                return;
            }

            if (ReferenceEquals(ItemData, null)) {
                Debug.LogError("You must provide an Item Data.");
                return;
            }

            GameObject forceField = new GameObject("Force Field", typeof(ParticleSystemForceField), typeof(BoxCollider));
            forceField.transform.parent = transform;
            forceField.transform.localPosition = Vector3.zero;

            ElementData elementData = ElementManager.Instance.GetElementData(ElementType);
            forceField.gameObject.layer = ToSingleLayer(elementData.LayerMask);

            forceField.GetComponent<ParticleSystemForceField>().endRange = RuneData.AttractionRadius;

            BoxCollider collider = forceField.GetComponent<BoxCollider>();
            collider.isTrigger = false;

            Collider parentCollider = transform.GetComponent<Collider>();
            collider.size = parentCollider.bounds.size;
            collider.center = parentCollider.bounds.center - transform.position;
        }

        /**
        * This method updates the Rune based on the particle count.
        **/
        public void UpdateRune(int particleCount) {
            ParticleCount += particleCount;
            if (ParticleCount >= RuneData.RequiredParticles) {

                // Disable all the colliders
                transform.GetComponent<Collider>().enabled = false;
                transform.GetChild(0).GetComponent<Collider>().enabled = false;

                // Spawn the relevant ItemData
                Item item = new Item(ItemData, 1);
                ItemManager.Instance.PlaceItemInWorld(item, transform.position, transform.rotation);

                // Destroy itself
                Destroy(gameObject);
            }
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