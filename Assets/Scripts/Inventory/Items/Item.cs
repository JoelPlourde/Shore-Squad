using UnityEngine;

namespace ItemSystem {
	public class Item {

		public ItemData ItemData;
		public int Amount;

		public Item(ItemData itemData, int amount) {
			ItemData = itemData;
			Amount = amount;
			Index = -1;
		}

		public Item(ItemData itemData, int amount, int index) : this(itemData, amount) {
			Index = index;
		}

		public override bool Equals(object obj) {
			if ((obj == null) || !GetType().Equals(obj.GetType())) {
				return false;
			} else {
				Item item = (Item)obj;
				return ItemData == item.ItemData && Amount == item.Amount;
			}
		}

		public override int GetHashCode() {
			return ItemData.ID.GetHashCode();
		}

		public override string ToString() {
			return ItemData.ToString() + "x" + Amount + " at: " + Index;
		}

		public int Index { get; private set; }
	}
}