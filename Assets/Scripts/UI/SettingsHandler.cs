using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public class SettingsHandler : MonoBehaviour, IMenu {

		public static SettingsHandler Instance;

		private void Awake() {
			Canvas = GetComponent<Canvas>();
			Instance = this;

			Button saveButton = transform.Find("SaveButton").GetComponent<Button>();
			saveButton.onClick.AddListener(() => {
				SaveManager.Instance.SaveGame();
			});

			Button loadButton = transform.Find("LoadButton").GetComponent<Button>();
			loadButton.onClick.AddListener(() => {
				throw new UnityException("Not yet implemented!");
			});

			Button exitButton = transform.Find("ExitButton").GetComponent<Button>();
			exitButton.onClick.AddListener(() => {
				Application.Quit();
			});
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
