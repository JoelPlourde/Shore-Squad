using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Button))]
	public class MenuButton : InteractiveButton {

		private Button _button;

		[SerializeField]
		private KeyCode _keyCode;

		public void Initialize(UnityAction<int> onClick) {
			_button = GetComponent<Button>();
			_button.onClick.AddListener(delegate { onClick(transform.GetSiblingIndex()); });
		}

		private void OnDestroy() {
			if (!ReferenceEquals(_button, null)) {
				_button.onClick.RemoveAllListeners();
			}
		}

		public override string GetTooltip() {
			return _tooltip + " [" + _keyCode.ToString() + "]";
		}

		public KeyCode GetKeyCode() {
			return _keyCode;
		}
	}
}
