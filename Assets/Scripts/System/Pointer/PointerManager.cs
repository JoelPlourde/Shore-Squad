using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointerSystem {
	public class PointerManager : MonoBehaviour {

		public static PointerManager Instance;

		private static Dictionary<PointerMode, PointerData> _pointers = new Dictionary<PointerMode, PointerData>();

		private void Awake() {
			Instance = this;

			PointerData[] pointerDatas = Resources.LoadAll<PointerData>("Scriptable Objects/Pointers");

			foreach (PointerData pointerData in pointerDatas) {
				_pointers.Add(pointerData.PointerMode, pointerData);
			}

			SetPointer(PointerMode.DEFAULT);
		}

		public void SetPointer(PointerMode pointerMode) {
			if (_pointers.TryGetValue(pointerMode, out PointerData pointerData)) {
				Cursor.SetCursor(pointerData.Texture, pointerData.Hotspot, CursorMode.Auto);
			} else {
				throw new UnityException("This pointer mode couldn't be found under Resources/Scriptable Objects/Pointers");
			}
		}
	}
}
