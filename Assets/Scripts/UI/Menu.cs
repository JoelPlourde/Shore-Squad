using UnityEngine;

namespace UI {
	public interface IMenu {

		void Open(Actor actor);

		void Close(Actor actor);

		Canvas Canvas { get; set; }
	}
}
