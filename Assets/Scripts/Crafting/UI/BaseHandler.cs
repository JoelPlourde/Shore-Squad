using ItemSystem;
using ItemSystem.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	// This class has children with SlotHandler components, the goal of this class is to expose method that enable to refresh the 
	// display of the slot underneath.
	public class BaseHandler : MonoBehaviour, IContainer {

		[SerializeField]
		[Tooltip("Whether or not Items can be dragged within this User Interface or not.")]
		protected bool _readonly = false;

		[SerializeField]
		[Tooltip("Whether or not Items can be used/interacted within this User Interface or not.")]
		protected bool _interactable = false;

		public event Action OnItemBeginDragEvent;
		public event Action OnItemEndDragEvent;

		protected void Start() {
			Slots = transform.GetComponentsInChildren<SlotHandler>(true);
			if (ReferenceEquals(Slots, null) || Slots.Length == 0) {
				throw new UnityException("This BaseHandler does not have any children(s) with the SlotHandler component, please verify.");
			}

			foreach (var slotHandler in Slots) {
				slotHandler.Disable();
				slotHandler.OnBeginDragEvent += OnBeginDragEvent;
				slotHandler.OnEndDragEvent += OnEndDragEvent;
			}
		}

		private void OnDestroy() {
			foreach (var slotHandler in Slots) {
				slotHandler.OnBeginDragEvent -= OnBeginDragEvent;
				slotHandler.OnEndDragEvent -= OnEndDragEvent;
			}
		}

		/// <summary>
		/// Initialize this Handler based on the Inventory in parameters.
		/// </summary>
		/// <param name="inventory">The Inventory.</param>
		public void Initialize(Inventory inventory) {
			Inventory = inventory;
			int count = inventory.Items.Length;
			for (int i = 0; i < Slots.Length; i++) {
				Slots[i].Disable();
				if (i < count) {
					Slots[i].gameObject.SetActive(true);
					Slots[i].Initialize(this, _readonly, i);
				} else {
					Slots[i].gameObject.SetActive(false);
				}
			}
		}

		#region Events
		/// <summary>
		/// Event triggered whenever specifics indexes needs to be redraw.
		/// </summary>
		/// <param name="indexes">The list of indexes which are dirty.</param>
		/// <param name="items">The list of all items.</param>
		public void OnDirtyItemsEvent(List<int> indexes, Item[] items) {
			foreach (int index in indexes) {
				Slots[index].Refresh(items[index]);
			}
		}

		/// <summary>
		/// Event triggered whenever one of the SlotHandler with an Item has begin dragging.
		/// </summary>
		public void OnBeginDragEvent() {
			OnItemBeginDragEvent?.Invoke();
		}

		/// <summary>
		/// Event triggered whenever one of the SlotHandler with an Item has end dragging.
		/// </summary>
		public void OnEndDragEvent() {
			OnItemEndDragEvent?.Invoke();
		}

		/// <summary>
		/// Redraw all the Items.
		/// </summary>
		/// <param name="items">The items to be redrawn.</param>
		public void RedrawEvent(Item[] items) {
			for (int i = 0; i < items.Length; i++) {
				if (items[i] != null) {
					Slots[i].Refresh(items[i]);
				} else {
					Slots[i].Disable();
				}
			}
		}
		#endregion

		#region Container
		/// <summary>
		/// Add an item at Destination as instructed by the user.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="destinationIndex">Where to add the item</param>
		/// <param name="existingItem">If there is already something at this slot.</param>
		/// <returns>Whether the operation could be completed or not.</returns>
		public bool AddItemAtDestination(Item item, int destinationIndex, ref Item existingItem) {
			if (!IfCondition(item)) {
				return false;
			}

			if (_readonly) {
				return false;
			}
			return Inventory.AddItemInInventoryAtPosition(item, destinationIndex, ref existingItem);
		}

		/// <summary>
		/// Remove an Item at source as instructed by the user.
		/// </summary>
		/// <param name="item">The item</param>
		/// <param name="sourceIndex">Where the item originated from.</param>
		/// <returns>Whether the operation could be completed or not.</returns>
		public bool RemoveItemAtSource(Item item, int sourceIndex) {
			return Inventory.RemoveItemFromInventoryAtPosition(sourceIndex, item.Amount);
		}

		/// <summary>
		/// Swap two items within the same container.
		/// </summary>
		/// <param name="sourceIndex">Where the item originated from.</param>
		/// <param name="destinationIndex">Where the item has been deposited.</param>
		/// <returns>Whether the operation could be completed or not.</returns>
		public bool SwapItems(int sourceIndex, int destinationIndex) {
			if (!IfCondition(Inventory.Items[sourceIndex]) || !IfCondition(Inventory.Items[destinationIndex])) {
				return false;
			}

			if (_readonly) {
				return false;
			}

			return Inventory.SwapItems(sourceIndex, destinationIndex);
		}

		/// <summary>
		/// Simply apply a condition to verify if an Item can be placed in this Container or not.
		/// </summary>
		/// <param name="item">The item</param>
		/// <returns>True if so, false if not.</returns>
		public virtual bool IfCondition(Item item) {
			return true;
		}

		/// <summary>
		/// Return the Item at an index
		/// </summary>
		/// <param name="index">The index</param>
		/// <returns>The Item at the index.</returns>
		public Item GetItemAtIndex(int index) {
			return Inventory.GetItemAtIndex(index);
		}
		#endregion

		public bool IsInteractable { get { return _interactable; } }
		public bool ReadOnly { get { return _readonly; } }
		public Inventory Inventory { get; private set; }
		public SlotHandler[] Slots { get; private set; }
	}
}
