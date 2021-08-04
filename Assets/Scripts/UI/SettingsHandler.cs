using UnityEngine;

namespace UI {
	public class SettingsHandler : Menu {

		public static SettingsHandler Instance;

		protected override void Awake() {
			base.Awake();
			Instance = this;
			Debug.Log("Settings is initializeds!!");
		}

		public override void Open(Actor actor) {
			Debug.Log("Open!");
			Canvas.enabled = true;
		}

		public override void Close(Actor actor) {
			Canvas.enabled = false;
		}
	}
}
