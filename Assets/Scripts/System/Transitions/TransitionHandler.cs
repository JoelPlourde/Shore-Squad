using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class TransitionHandler : MonoBehaviour {

		public static TransitionHandler Instance;

		[Tooltip("Speed of the transition")]
		public float TransitionSpeed = 1f;

		private RectTransform _rectTransform;
		private Tuple<RectTransform, RectTransform> _panels;

		private void Awake() {
			Instance = this;

			_rectTransform = GetComponent<RectTransform>();
		}

		/// <summary>
		/// Fade-In
		/// </summary>
		/// <param name="sceneName">The name of the scene.</param>
		/// <param name="callback">The callback after the fade-in</param>
		public void FadeIn(string sceneName, Action<string> callback) {
			_panels = CreatePanels(true);

			LeanTween.moveLocalY(_panels.Item1.gameObject, _panels.Item1.offsetMin.y / 2f, TransitionSpeed).setOnComplete(x => {
				Destroy(_panels.Item1.gameObject);
			});

			LeanTween.moveLocalY(_panels.Item2.gameObject, _panels.Item1.offsetMin.y / 1.5f, TransitionSpeed).setOnComplete(x => {
				Destroy(_panels.Item2.gameObject);

				callback?.Invoke(sceneName);
			});
		}

		/// <summary>
		/// Fade-Out
		/// </summary>
		/// <param name="sceneName">The name of the scene.</param>
		/// <param name="callback">The callback after the fade-out</param>
		public void FadeOut(string sceneName, Action<string> callback) {
			_panels = CreatePanels(false);

			LeanTween.moveLocalY(_panels.Item1.gameObject, -_panels.Item1.offsetMin.y, TransitionSpeed).setOnComplete(x => {
				Destroy(_panels.Item1.gameObject);
			});

			LeanTween.moveLocalY(_panels.Item2.gameObject, _panels.Item1.offsetMin.y, TransitionSpeed).setOnComplete(x => {
				Destroy(_panels.Item2.gameObject);

				callback?.Invoke(sceneName);
			});
		}

		private Tuple<RectTransform, RectTransform> CreatePanels(bool direction) {
			return new Tuple<RectTransform, RectTransform>(CreatePanel("up", 1, 0, direction), CreatePanel("down", 0, 1, !direction));
		}

		private RectTransform CreatePanel(string name, int anchor, int pivot, bool direction) {
			GameObject panel = new GameObject(name, typeof(Image));

			panel.transform.SetParent(transform);

			Image image = panel.GetComponent<Image>();
			image.color = Color.black;

			RectTransform rectTransform = panel.GetComponent<RectTransform>();

			rectTransform.anchorMin = new Vector2(0, anchor);
			rectTransform.anchorMax = new Vector2(1, anchor);
			rectTransform.pivot = new Vector2(0.5f, pivot);

			float halfHeight = UserInterface.Instance.ScreenHeight / 2f;
			rectTransform.offsetMax = new Vector2(25f, direction ? halfHeight : 0f);
			rectTransform.offsetMin = new Vector2(-25f, direction ? 0f : -halfHeight);

			rectTransform.localScale = Vector3.one;

			return rectTransform;
		}
	}
}
