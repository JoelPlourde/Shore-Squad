using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointerSystem {
	[System.Serializable]
	[CreateAssetMenu(fileName = "PointerData", menuName = "ScriptableObjects/Pointer Data")]
	public class PointerData: ScriptableObject {
		[SerializeField]
		public PointerMode PointerMode;

		[SerializeField]
		public Texture2D Texture;

		[SerializeField]
		public HotspotPosition HotspotPosition;

		private void Awake() {
			switch (HotspotPosition) {
				case HotspotPosition.TOP_LEFT:
					Hotspot = Vector2.zero;
					break;
				case HotspotPosition.CENTER:
					Hotspot = new Vector2(Texture.width / 2, Texture.height / 2);
					break;
				case HotspotPosition.BOTTOM_LEFT:
					Hotspot = new Vector2(0, Texture.height - 1);
					break;
			}
		}

		public Vector2 Hotspot { get; private set; }
	}

	public enum PointerMode {
		DEFAULT, TALK, ATTACK, DEFAULT_PRESSED
	}

	public enum HotspotPosition {
		CENTER, TOP_LEFT, BOTTOM_LEFT
	}
}
