using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem {
    [RequireComponent(typeof(Rigidbody))]
    public class InteractableItem : InteractableBehavior, IInteractable {

		[Tooltip("The radius at which the player should stopped at.")]
		public float InteractionRadius;

        [SerializeField]
        protected ItemData _itemData;

        public void OnInteractEnter(Actor actor) {
			Item item = new Item(_itemData, 1);

			actor.Inventory.AddItemsToInventory(new List<Item> { item }, out List<Item> remainingItems);
            if (remainingItems.Count > 0) {
                Debug.Log("Inventory is full!");
                return;
            }

            OnInteractExit(actor);
        }

        public void OnInteractExit(Actor actor) {
            if (!this) {
                return;
            }
            Destroy(this.gameObject);
        }

		public float GetInteractionRadius() {
			return InteractionRadius;
		}

        public bool IsPickup() {
            return true;
        }

        protected override OutlineType GetOutlineType() {
            return OutlineType.ITEM;
        }
    }
}