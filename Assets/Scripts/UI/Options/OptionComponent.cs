using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Button))]
	public class OptionComponent : MonoBehaviour {

		private Button _button;
		private Text _text;

		private void Awake() {
			_button = GetComponent<Button>();
			_text = GetComponentInChildren<Text>();
		}

		public void Enable(string action, string message, ref float preferredWidth, UnityAction unityAction) {
			if (ReferenceEquals(_button, null)) {
				Awake();
			}

			_text.text = string.Format("<color=#BFBFBF>{0}</color> {1}", action, message);
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(unityAction);

			gameObject.SetActive(true);

			if (preferredWidth < _text.preferredWidth) {
				preferredWidth = _text.preferredWidth;
			}
		}

		public void Disable() {
			if (ReferenceEquals(_button, null)) {
				Awake();
			}

			gameObject.SetActive(false);
		}
	}
}
