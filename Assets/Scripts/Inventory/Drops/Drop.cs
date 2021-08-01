using UnityEngine;
using ItemSystem;

namespace DropSystem {
	[System.Serializable]
	public class Drop {

		[Tooltip("The item to roll.")]
		[SerializeField]
		public ItemData ItemData;

		[Tooltip("The relative weight of this drop.")]
		[Range(0, 100)]
		[SerializeField]
		private uint _weight;

		[Tooltip("The quantity of item to return on roll.")]
		[SerializeField]
		private Quantity _quantity;

		private int _amount = 0;

		/// <summary>
		/// Get the drop amount based on the minimum quantity and the maximum quantity.
		/// </summary>
		/// <returns></returns>
		public int Amount { get { return (_amount == 0) ? Random.Range((int)_quantity.Min, (int)(_quantity.Max + 1)) : _amount; } }

		public uint Weight { get { return _weight; } }

		public override string ToString() {
			return ItemData.name + "x" + Amount;
		}
	}
}
