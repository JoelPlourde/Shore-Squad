using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public abstract class BaseMenuBar : MonoBehaviour {

		protected MenuButton[] _menuButtons;
		protected Dictionary<MenuType, IMenu> _interfaceStatus;

		protected virtual void Awake() {
			_menuButtons = GetComponentsInChildren<MenuButton>();
			foreach (MenuButton menuButton in _menuButtons) {
				menuButton.Initialize(OnClick);
			}
		}

		/// <summary>
		/// On Click on the button of the menu, open/close the interface to have only a single interface opened.
		/// </summary>
		/// <param name="index">The index clicked.</param>
		protected void OnClick(int index) {
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
		protected abstract void ToggleInterface(MenuType index); 

		/// <summary>
		/// Toggle the menu.
		/// </summary>
		/// <param name="menu">The menu</param>
		/// <param name="actor">The actor</param>
		protected void ToggleMenu(IMenu menu, Actor actor) {
			if (!menu.Canvas.enabled) {
				menu.Open(actor);
			} else {
				menu.Close(actor);
			}
		}

		public MenuType CurrentMenu { get; protected set; } = MenuType.NONE;
	}
}
