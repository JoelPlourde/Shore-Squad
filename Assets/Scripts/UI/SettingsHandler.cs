using UnityEngine;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public class SettingsHandler : MonoBehaviour, IMenu {

		public static SettingsHandler Instance;

		private void Awake() {
			Canvas = GetComponent<Canvas>();
			Instance = this;
		}

		public void Open(Actor actor) {
			Canvas.enabled = true;
		}

		public void Close(Actor actor) {
			Canvas.enabled = false;
		}

		public Canvas Canvas { get; set; }
	}
}
