using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	/// <summary>
	/// The Game Menu bar is a bar related to game-generic menu.
	/// </summary>
	public class GameMenuBar : BaseMenuBar {

		public static GameMenuBar Instance;

		private new void Awake() {
			Instance = this;

			_menuButtons = GetComponentsInChildren<MenuButton>();
			_menuButtons[0].Initialize(delegate { OnClick((int)MenuType.SETTINGS); });

			UserInputs.Instance.Subscribe(_menuButtons[0].GetKeyCode(), delegate { OnClick((int) MenuType.SETTINGS); });

			_interfaceStatus = new Dictionary<MenuType, IMenu>() {
				{ MenuType.SETTINGS, SettingsHandler.Instance }
			};
		}

		protected override void ToggleInterface(MenuType index) {
			ToggleMenu(_interfaceStatus[index], null);
		}
	}
}
