using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem {
	public class OptionHandler : MonoBehaviour {

		private RectTransform _rectTransform;

		private void Awake() {
			Debug.Log("Awake!");

			_rectTransform = GetComponent<RectTransform>();
			_rectTransform.localScale = Vector3.zero;
		}

		void OnEnable() {
			Debug.Log("On Enable!");
			LeanTween.scale(gameObject, Vector3.one, 0.2f);
		}

		void OnDisable() {
			_rectTransform.localScale = Vector3.zero;
		}
	}
}
