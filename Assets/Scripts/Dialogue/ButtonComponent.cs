using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueSystem {
	[RequireComponent(typeof(Button))]
	public class ButtonComponent : MonoBehaviour {

		private Button _button;
		private Text _text;

		private void Awake() {
			_button = GetComponent<Button>();
			_text = GetComponentInChildren<Text>();
		}

		public void UpdateText(string content) {
			if (!ReferenceEquals(_text, null)) {
				_text.text = content;
			}
		}

		public void SubscribeListener(UnityAction unityAction) {
			_button.onClick.AddListener(unityAction);
		}

		public void UnsubscribeListeners() {
			_button.onClick.RemoveAllListeners();
		}
	}
}
