using ItemSystem.EffectSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Item Data")]
	public class ItemData : ScriptableObject {

		[SerializeField]
		[Tooltip("Image of this item")]
		public Sprite Sprite;

		[SerializeField]
		[Tooltip("Prefab of this item as it appears in the world.")]
		public GameObject Prefab;

		[SerializeField]
		[Tooltip("The type of item")]
		public ItemType ItemType = ItemType.DEFAULT;

		[SerializeField]
		[Tooltip("The effects that this item has when activated.")]
		public List<ItemEffect> ItemEffects = new List<ItemEffect>();

		[SerializeField]
		[Tooltip("Determine whether or not the object can be stacked in the inventory or not.")]
		public bool Stackable = false;

		[SerializeField]
		[Tooltip("Determine whether or not the item is burnable.")]
		public bool Burnable = false;

		[SerializeField]
		[HideInInspector]
		[Tooltip("The value of power this Item provide when burned.")]
		public int Power = 0;

		public override bool Equals(object other) {
			if ((other == null) || !GetType().Equals(other.GetType())) {
				return false;
			} else {
				return ID == ((ItemData)other).ID;
			}
		}

		public override int GetHashCode() {
			if (ID == null) {
				return 0;
			}
			return ID.GetHashCode();
		}

		public override string ToString() {
			return "Item : { " +
				"ID: " + ID +
				", Name: " + name + 
				" }";
		}

		public string ID { get; set; }
	}
}
