using ItemSystem.UI;
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

			UserInputs.Instance.Subscribe(KeyCode.E, delegate { OnClick(0); });
			UserInputs.Instance.Subscribe(KeyCode.R, delegate { OnClick(1); });
			UserInputs.Instance.Subscribe(KeyCode.T, delegate { OnClick(2); });
			UserInputs.Instance.Subscribe(KeyCode.Escape, delegate {
				if (Squad.HasSelected) {
					Squad.UnselectAll();
				} else {
					OnClick(3);
				}
			});

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

				SoundSystem.Instance.PlaySound(SoundManager.Instance.GetAudioClip("menu_close"), 0.5f);
			} else {
				if (CurrentMenu != MenuType.NONE) {
					ToggleInterface(CurrentMenu);
				}

				SoundSystem.Instance.PlaySound(SoundManager.Instance.GetAudioClip("menu_open"), 0.3f);

				CurrentMenu = menuType;
			}

			ToggleInterface(menuType);
		}

		/// <summary>
		/// Toggle the interface indicates by the index
		/// </summary>
		/// <param name="index">The index of the interface to toggle.</param>
		private void ToggleInterface(MenuType index) {
			if (index == MenuType.SETTINGS) {
				ToggleMenu(_interfaceStatus[index], null);
				return;
			}

			if (Squad.Any(out Actor actor)) {
				ToggleMenu(_interfaceStatus[index], actor);
			}
		}

		/// <summary>
		/// Toggle the menu.
		/// </summary>
		/// <param name="menu">The menu</param>
		/// <param name="actor">The actor</param>
		private void ToggleMenu(Menu menu, Actor actor) {
			if (!menu.Canvas.enabled) {
				menu.Open(actor);
			} else {
				menu.Close(actor);
			}
		}

		public MenuType CurrentMenu { get; private set; } = MenuType.NONE;
	}
}
