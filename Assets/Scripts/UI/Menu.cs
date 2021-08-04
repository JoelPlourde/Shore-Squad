using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public abstract class Menu : MonoBehaviour {

		protected virtual void Awake() {
			Canvas = GetComponent<Canvas>();
		}

		public abstract void Open(Actor actor);

		public abstract void Close(Actor actor);

		public Canvas Canvas { get; private set; }
	}
}
