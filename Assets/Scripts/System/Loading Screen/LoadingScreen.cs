using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem {
	[RequireComponent(typeof(Canvas))]
	public class LoadingScreen : MonoBehaviour {

		public static LoadingScreen Instance;

		private Canvas _canvas;
		private Slider _slider;

		private LTDescr _descr;
		private Text _text;

		private void Awake() {
			Instance = this;

			_canvas = GetComponent<Canvas>();
			_slider = GetComponentInChildren<Slider>();
			_text   = GetComponentInChildren<Text>();
		}

		public void ShowLoadingScreen() {
			_slider.value = 0;

			_canvas.enabled = true;
		}

		public void HideLoadingScreen() {
			_canvas.enabled = false;

			_descr = null;
		}

		public void UpgradeProgressBar(float progress) {
			if (!ReferenceEquals(_descr, null)) {
				LeanTween.cancel(_descr.id);
			}

			_descr = LeanTween.value(gameObject, _slider.value, progress, 1f).setEase(LeanTweenType.linear).setOnUpdate((float value) => {
				_slider.value = value;
				_text.text = (value * 100f) + "%";
			});
		}
	}
}
