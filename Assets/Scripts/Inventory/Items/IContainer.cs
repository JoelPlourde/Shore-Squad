using ItemSystem;

namespace UI {
	public interface IContainer {

		Inventory Inventory { get; }

		bool ReadOnly { get; }
		bool IsInteractable { get; }

		Item GetItemAtIndex(int index);

		bool SwapItems(int sourceIndex, int destinationIndex);

		bool AddItemAtDestination(Item item, int destinationIndex, ref Item existingItem);

		bool RemoveItemAtSource(Item item, int sourceIndex);

		bool IfCondition(Item item);
	}
}
