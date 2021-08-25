using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(GridLayoutGroup))]
		public class InventoryHandler : Menu {

			public static InventoryHandler Instance;

			public GameObject SlotTemplate;

			private SlotHandler[] _slots = new SlotHandler[Inventory.MAX_STACK];

			protected override void Awake() {
				base.Awake();
				Instance = this;

				GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
				gridLayoutGroup.cellSize = new Vector2(50, 50);
				gridLayoutGroup.spacing = new Vector2(5, 5);
				gridLayoutGroup.padding.left = 10;
				gridLayoutGroup.padding.top = 10;
		
				RectTransform rectTransform = GetComponent<RectTransform>();
				rectTransform.pivot = new Vector2(1, 0);
				rectTransform.sizeDelta = new Vector2(230, 285);

				for (int i = 0; i < Inventory.MAX_STACK; i++) {
					GameObject slotHandlerObj = Instantiate(SlotTemplate, transform);
					slotHandlerObj.name = "Slot" + i;
					_slots[i] = slotHandlerObj.GetComponent<SlotHandler>();
					_slots[i].Enable(false);
				}
			}

			public override void Open(Actor actor) {
				actor.Inventory.OnDirtyItemsEvent += OnDirtyItemsEvent;
				actor.Inventory.OnRedrawEvent += RedrawEvent;

				RedrawEvent(actor.Inventory.Items);

				Canvas.enabled = true;
			}

			public override void Close(Actor actor) {
				actor.Inventory.OnDirtyItemsEvent -= OnDirtyItemsEvent;
				actor.Inventory.OnRedrawEvent -= RedrawEvent;

				foreach (SlotHandler slot in _slots) {
					slot.Enable(false);
				}

				Canvas.enabled = false;
			}

			private void OnDirtyItemsEvent(List<int> indexes, Item[] items) {
				foreach (int index in indexes) {
					_slots[index].Refresh(items[index]);
				}
			}

			private void RedrawEvent(Item[] items) {
				for (int i = 0; i < items.Length; i++) {
					if (items[i] != null) {
						_slots[i].Refresh(items[i]);
					} else {
						_slots[i].Enable(false);
					}
				}
			}
		}
	}
}
