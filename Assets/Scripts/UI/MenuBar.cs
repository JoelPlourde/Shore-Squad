using ItemSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class MenuBar : MonoBehaviour {

		public static MenuBar Instance;

		private MenuButton[] _menuButtons;
		private Dictionary<int, Menu> _interfaceStatus;

		private void Awake() {
			Instance = this;

			_menuButtons = GetComponentsInChildren<MenuButton>();
			foreach (MenuButton menuButton in _menuButtons) {
				menuButton.Initialize(OnClick);
			}

			// TODO fill-in.
			_interfaceStatus = new Dictionary<int, Menu>() {
				{ 0, InventoryHandler.Instance}
			};
		}

		public void OnClick(int index) {
			ToggleInterface(index);
		}

		private void ToggleInterface(int index) {
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
	}
}
