using ItemSystem.EquipmentSystem;
using System;
using UI;
using UnityEngine;

namespace ItemSystem {
	namespace UI {
		public class EquipmentHandler : Menu {

			public static EquipmentHandler Instance;

			private SlotHandler[] _slots = new SlotHandler[Enum.GetNames(typeof(SlotType)).Length];

			protected override void Awake() {
				base.Awake();
				Instance = this;

				_slots = GetComponentsInChildren<SlotHandler>();

				foreach (SlotHandler slotHandler in _slots) {
					slotHandler.Refresh(null);
				}
			}

			public override void Open(Actor actor) {
				actor.Armory.OnEquipmentAddedEvent += OnEquipmentAdded;
				actor.Armory.OnEquipmentRemovedEvent += OnEquipmentRemoved;

				int index = 0;
				foreach (SlotType slotType in (SlotType[])Enum.GetValues(typeof(SlotType))) {
					index = (int) slotType;
					if (actor.Armory.Equipments[slotType].IsAttached) {
						_slots[index].Refresh(new Item(actor.Armory.Equipments[slotType].Equipment.EquipmentData, actor.Armory.Equipments[slotType].Equipment.Amount));
					} else {
						_slots[index].Enable(false);
					}
				}

				Canvas.enabled = true;
			}

			public override void Close(Actor actor) {
				Canvas.enabled = false;

				actor.Armory.OnEquipmentAddedEvent -= OnEquipmentAdded;
				actor.Armory.OnEquipmentRemovedEvent -= OnEquipmentRemoved;

				foreach (SlotHandler slotHandler in _slots) {
					slotHandler.Enable(false);
				}
			}

			private void OnEquipmentAdded(Equipment equipment) {
				_slots[(int)equipment.EquipmentData.SlotType].Refresh(new Item(equipment.EquipmentData, equipment.Amount));
			}

			private void OnEquipmentRemoved(Equipment equipment) {
				_slots[(int)equipment.EquipmentData.SlotType].Enable(false);
			}
		}
	}
}
