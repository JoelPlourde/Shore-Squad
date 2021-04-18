using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem {
	public class Sun : MonoBehaviour {

		public static Sun Instance;

		private void Awake() {
			Instance = this;

			DontDestroyOnLoad(this.gameObject);
		}
	}
}