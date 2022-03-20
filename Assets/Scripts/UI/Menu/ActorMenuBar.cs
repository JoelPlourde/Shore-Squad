using ItemSystem.UI;
using SkillSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	/// <summary>
	/// The Menu Bar is a bar specific to Actor-related menu.
	/// </summary>
	public class ActorMenuBar : BaseMenuBar {

		public static ActorMenuBar Instance;

		private new void Awake() {
			Instance = this;

			base.Awake();

			UserInputs.Instance.Subscribe(_menuButtons[(int) MenuType.INVENTORY].GetKeyCode(), delegate { OnClick((int) MenuType.INVENTORY); });
			UserInputs.Instance.Subscribe(_menuButtons[(int) MenuType.EQUIPMENT].GetKeyCode(), delegate { OnClick((int) MenuType.EQUIPMENT); });
			UserInputs.Instance.Subscribe(_menuButtons[(int) MenuType.EXPERIENCE].GetKeyCode(), delegate { OnClick((int) MenuType.EXPERIENCE); });

			_interfaceStatus = new Dictionary<MenuType, IMenu>() {
				{ MenuType.INVENTORY, InventoryHandler.Instance},
				{ MenuType.EQUIPMENT, EquipmentHandler.Instance },
				{ MenuType.EXPERIENCE, ExperienceHandler.Instance }
			};
		}

		protected override void ToggleInterface(MenuType index) {
			if (Squad.Any(out Actor actor)) {
				ToggleMenu(_interfaceStatus[index], actor);
			}
		}
	}
}
