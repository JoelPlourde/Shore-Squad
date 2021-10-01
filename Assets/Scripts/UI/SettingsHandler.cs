using UnityEngine;

namespace UI {
	public class SettingsHandler : Menu {

		public static SettingsHandler Instance;

		protected override void Awake() {
			base.Awake();
			Instance = this;
		}

		public override void Open(Actor actor) {
			Canvas.enabled = true;
		}

		public override void Close(Actor actor) {
			Canvas.enabled = false;
		}
	}
}
