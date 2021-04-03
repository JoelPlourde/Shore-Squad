using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public class Tooltip : MonoBehaviour {
		private RectTransform _rectTransform;
		private RectTransform _childrenRectTransform;
		private ContentSizeFitter _contentSizeFitter;
		private Canvas _canvas;
		private Text _text;

		private Vector3 _offset = new Vector2(-20, 20);
		private Vector2 _padding = new Vector2(20, 10);

		private void Awake() {
			if (ReferenceEquals(Instance, null)) {
				Instance = this;
			}

			_canvas = GetComponent<Canvas>();
			if (ReferenceEquals(_canvas, null)) {
				throw new UnityException("Canvas component is null.");
			}

			_text = GetComponentInChildren<Text>();
			if (ReferenceEquals(_text, null)) {
				throw new UnityException("Text component is null.");
			}

			_rectTransform = GetComponent<RectTransform>();
			_childrenRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

			_canvas.enabled = false;
		}

		public void ShowTooltip(string text) {
			transform.position = Input.mousePosition - _offset;
			_text.text = text;
			StartCoroutine(UpdateContentSize());
		}

		public void HideTooltip() {
			CancelInvoke();
			_canvas.enabled = false;
			CancelInvoke();
		}

		private void FollowMouse() {
			transform.position = Input.mousePosition - _offset;
		}

		private IEnumerator UpdateContentSize() {
			yield return new WaitForEndOfFrame();
			_rectTransform.sizeDelta = _childrenRectTransform.sizeDelta + _padding;
			InvokeRepeating(nameof(FollowMouse), 0f, 0.016f);
			_canvas.enabled = true;
		}

		public static Tooltip Instance { get; private set; }
	}
}