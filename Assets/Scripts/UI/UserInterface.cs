using UnityEngine;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	public class UserInterface : MonoBehaviour {

		private RectTransform _rectTransform;
		private CanvasScaler _canvasScaler;

		private void Awake() {
			Instance = this;

			DontDestroyOnLoad(this);

			Portraits = transform.Find("Portraits");
			if (ReferenceEquals(Portraits, null)) {
				throw new UnityException("Verify the structure of the UIController to include a child named: Portraits");
			}

			_rectTransform = GetComponent<RectTransform>();

			_canvasScaler = GetComponent<CanvasScaler>();
			_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			_canvasScaler.referenceResolution = new Vector2(1920, 1080);
		}

		public static UserInterface Instance { get; private set; }
		public Transform Portraits { get; private set; }
		public float ScreenHeight { get { return _rectTransform.sizeDelta.y; } }
		public float ScreenWeight { get { return _rectTransform.sizeDelta.x; } }
	}
}
