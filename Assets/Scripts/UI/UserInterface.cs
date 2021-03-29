using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	public class UserInterface : MonoBehaviour {

		private CanvasScaler _canvasScaler;

		private void Awake() {
			if (Instance == null) {
				Instance = this;
			}

			DontDestroyOnLoad(this);

			Portraits = transform.Find("Portraits");
			if (Portraits == null) {
				throw new UnityException("Verify the structure of the UIController to include a child named: Portraits");
			}

			_canvasScaler = GetComponent<CanvasScaler>();
			_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			_canvasScaler.referenceResolution = new Vector2(1920, 1080);
		}

		public static UserInterface Instance { get; private set; }
		public Transform Portraits { get; private set; }
	}
}
