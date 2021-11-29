using ItemSystem.EquipmentSystem;
using System;
using UI;
using UnityEngine;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(Canvas))]
		public class EquipmentHandler : MonoBehaviour, IMenu {

			public static EquipmentHandler Instance;

			private Armory _currentArmory;

			private SlotHandler[] _slots = new SlotHandler[Enum.GetNames(typeof(SlotType)).Length];

			private void Awake() {
				Canvas = GetComponent<Canvas>();
				Instance = this;

				_slots = GetComponentsInChildren<SlotHandler>();

				foreach (SlotHandler slotHandler in _slots) {
					slotHandler.Refresh(null);
				}
			}

			public void Open(Actor actor) {
				_currentArmory = actor.Armory;

				actor.Armory.OnEquipmentAddedEvent += OnEquipmentAdded;
				actor.Armory.OnEquipmentRemovedEvent += OnEquipmentRemoved;

				int index = 0;
				foreach (SlotType slotType in (SlotType[])Enum.GetValues(typeof(SlotType))) {
					index = (int)slotType;
					if (actor.Armory.Equipments[slotType].IsAttached) {
						_slots[index].Refresh(new Item(actor.Armory.Equipments[slotType].Equipment.EquipmentData, actor.Armory.Equipments[slotType].Equipment.Amount));
					} else {
						_slots[index].Disable();
					}
				}

				Canvas.enabled = true;
			}

			public void Close(Actor actor) {
				Canvas.enabled = false;

				_currentArmory = null;

				actor.Armory.OnEquipmentAddedEvent -= OnEquipmentAdded;
				actor.Armory.OnEquipmentRemovedEvent -= OnEquipmentRemoved;

				foreach (SlotHandler slotHandler in _slots) {
					slotHandler.Disable();
				}
			}

			private void OnEquipmentAdded(Equipment equipment) {
				_slots[(int)equipment.EquipmentData.SlotType].Refresh(new Item(equipment.EquipmentData, equipment.Amount));
			}

			private void OnEquipmentRemoved(Equipment equipment) {
				_slots[(int)equipment.EquipmentData.SlotType].Disable();
			}

			public Canvas Canvas { get; set; }
		}
	}
}
