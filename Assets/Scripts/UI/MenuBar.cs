﻿using ItemSystem.UI;
using SkillSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class MenuBar : MonoBehaviour {

		public static MenuBar Instance;

		private MenuButton[] _menuButtons;
		private Dictionary<MenuType, Menu> _interfaceStatus;

		private void Awake() {
			Instance = this;

			_menuButtons = GetComponentsInChildren<MenuButton>();
			foreach (MenuButton menuButton in _menuButtons) {
				menuButton.Initialize(OnClick);
			}

			_interfaceStatus = new Dictionary<MenuType, Menu>() {
				{ MenuType.INVENTORY, InventoryHandler.Instance},
				{ MenuType.EQUIPMENT, EquipmentHandler.Instance },
				{ MenuType.EXPERIENCE, ExperienceHandler.Instance },
				{ MenuType.SETTINGS, SettingsHandler.Instance }
			};
		}

		/// <summary>
		/// On Click on the button of the menu, open/close the interface to have only a single interface opened.
		/// </summary>
		/// <param name="index">The index clicked.</param>
		public void OnClick(int index) {
			MenuType menuType = (MenuType)index;

			if (CurrentMenu == menuType) {
				CurrentMenu = MenuType.NONE;
			} else {
				if (CurrentMenu != MenuType.NONE) {
					ToggleInterface(CurrentMenu);
				}

				CurrentMenu = menuType;
			}

			ToggleInterface(menuType);
		}

		/// <summary>
		/// Toggle the interface indicates by the index
		/// </summary>
		/// <param name="index">The index of the interface to toggle.</param>
		private void ToggleInterface(MenuType index) {
			if (Squad.Any(out Actor actor)) {
				if (_interfaceStatus.TryGetValue(index, out Menu menu)) {
					if (!menu.Canvas.enabled) {
						menu.Open(actor);
					} else {
						menu.Close(actor);
					}
				} else {
					throw new UnityException("This feature is not yet implemented.");
				}
			}
		}

		public MenuType CurrentMenu { get; private set; } = MenuType.NONE;
	}
}
