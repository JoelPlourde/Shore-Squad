using UnityEngine;

namespace ConstructionSystem {
	public interface IConstructable {

		void Initialize();

		void Enable();

		void Disable();

		bool IsPlacementValid();

		void Rotate();
	}
}
