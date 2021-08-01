using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Button))]
	public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

		private static readonly Vector3 SCALE_UP = new Vector3(1.2f, 1.2f, 1.2f);

		private static readonly Vector3 SCALE_DOWN = new Vector3(1.15f, 1.15f, 1.15f);

		#region Listeners
		private Button _button;

		public void Initialize(UnityAction<int> onClick) {
			_button = GetComponent<Button>();
			_button.onClick.AddListener(delegate { onClick(transform.GetSiblingIndex()); });
		}

		private void OnDestroy() {
			if (!ReferenceEquals(_button, null)) {
				_button.onClick.RemoveAllListeners();
			}
		}
		#endregion

		#region OnPointerEvent
		public void OnPointerDown(PointerEventData eventData) {
			LeanTween.scale(gameObject, SCALE_DOWN, 0.05f);
		}

		public void OnPointerUp(PointerEventData eventData) {
			LeanTween.scale(gameObject, SCALE_UP, 0.05f);
		}

		public void OnPointerEnter(PointerEventData pointerEventData) {
			LeanTween.scale(gameObject, SCALE_UP, 0.25f);
		}

		public void OnPointerExit(PointerEventData pointerEventData) {
			if (!ReferenceEquals(gameObject, null)) {
				LeanTween.cancel(gameObject);
				LeanTween.scale(gameObject, Vector3.one, 0.1f);
			} 
		}
		#endregion
	}
}
