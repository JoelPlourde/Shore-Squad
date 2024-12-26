using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;

namespace ElementalRift {
    [RequireComponent(typeof(SphereCollider))]
    public class RiftBehaviour : MonoBehaviour {

        public RiftData riftData;

        public float _percentage = 1.0f;

        public LayerMask _excludeLayerMask;
        public LayerMask _includeLayerMask;

        private Dictionary<ElementType, int> _healthByElements = new Dictionary<ElementType, int>();
        private List<RuneBehaviour> _runeBehaviours = new List<RuneBehaviour>();
        private OrbBehaviour _orbBehaviour;
        private SphereCollider _sphereCollider;

        private void Awake() {
            if (ReferenceEquals(riftData, null)) {
                Debug.LogError("You must provide a Rift Data.");
                return;
            }

            // Initialize the OrbBehaviour
            _orbBehaviour = GetComponentInChildren<OrbBehaviour>(true);
            _orbBehaviour.Initialize(this);

            // Initialize the Trigger collider
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.center = transform.position;
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = riftData.InfluenceRadius;
            _sphereCollider.excludeLayers = _excludeLayerMask;
            _sphereCollider.includeLayers = _includeLayerMask;

            InitializeRift(riftData);
        }

        private void InitializeRift(RiftData riftData) {
            int primaryHealth = riftData.MaximumHealth / 2;
            int secondaryHealth = riftData.MaximumHealth / 4;

            AddElementalHealthToOrb(riftData.PrimaryElement, (int) primaryHealth);
            AddElementalHealthToOrb(riftData.SecondaryElement, (int) secondaryHealth);

            _orbBehaviour.gameObject.SetActive(true);
            _orbBehaviour.SpawnOrb(riftData.PrimaryElement, riftData.SecondaryElement);
        }

        private void AddElementalHealthToOrb(ElementType elementType, int health = 100) {
            if (_healthByElements.ContainsKey(elementType)) {
                _healthByElements[elementType] += health;
            } else {
                _healthByElements.Add(elementType, health);
            }
        }

        /**
        * This method reduces the health of the rift based on the element type and the damage.
        **/
        public void ReduceHealth(GameObject rune, ElementType elementType, int damage) {
            if (_healthByElements.ContainsKey(elementType)) {
                _healthByElements[elementType] -= damage;
                if (_healthByElements[elementType] <= 0) {
                    _healthByElements.Remove(elementType);
                }

                _percentage = CalculateHealthPercentage();

                if (_percentage <= riftData.MinimumHealthPercentage) {
                    _orbBehaviour.CollapseOrb(elementType);
                    _orbBehaviour.gameObject.SetActive(false);
                }

                UpdateOrb();

                // Iterate over the _runeBehaviours in reverse order:

                for (int i = _runeBehaviours.Count - 1; i >= 0; i--) {
                    RuneBehaviour runeBehaviour = _runeBehaviours[i];
                    if (runeBehaviour == null) {
                        _runeBehaviours.RemoveAt(i);
                        continue;
                    }

                    if (runeBehaviour.gameObject == rune) {
                        runeBehaviour.UpdateRune(damage);
                    }
                }
            }
        }

        /**
        * This method updates the Orb based on the health of the Rift.
        **/
        private void UpdateOrb() {
            // Iterate over the elements to find the two most dominant elements
            var sortedDict = _healthByElements.OrderBy(x => x.Value);

            ElementType primaryElement = sortedDict.First().Key;
            ElementType secondaryElement;

            if (sortedDict.Count() == 1) {
                secondaryElement = ElementType.NONE;
            } else {
                secondaryElement = sortedDict.Skip(1).First().Key;
            }

            _orbBehaviour.ChangeElement(primaryElement, secondaryElement);

            _orbBehaviour.ScaleOrb(_percentage);
        }

        /**
        * This method calculates the % of health of the Rift based on the health of each element.
        **/
        private float CalculateHealthPercentage() {
            // For each element, add the int value to the overall health
            int health = 0;
            foreach (KeyValuePair<ElementType, int> entry in _healthByElements) {
                health += entry.Value;
            }
            return health / (float) riftData.MaximumHealth;
        }

        #region Trigger Methods
        /**
        * This method is called when a Rune enters the Rift to register it.
        **/
        private void OnTriggerEnter(Collider other) {
            if (ReferenceEquals(other.transform.parent, null)) {
                return;
            }

            RuneBehaviour runeBehaviour = other.transform.parent.GetComponent<RuneBehaviour>();
            if (ReferenceEquals(runeBehaviour, null)) {
                return;
            }

            _runeBehaviours.Add(runeBehaviour);
        }

        /**
        * This method is called when a Rune enters the Rift to unregister it.
        **/
        private void OnTriggerExit(Collider other) {
            Debug.Log("OnTriggerExit!");
            if (ReferenceEquals(other.transform.parent, null)) {
                return;
            }

            RuneBehaviour runeBehaviour = other.transform.parent.GetComponent<RuneBehaviour>();
            if (ReferenceEquals(runeBehaviour, null)) {
                return;
            }

            _runeBehaviours.Remove(runeBehaviour);
        }
        #endregion
    }
}