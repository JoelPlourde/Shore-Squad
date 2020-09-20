using UnityEngine;
using ConstructionSystem;

namespace EncampmentSystem {
	public class Bed : ObjectBehaviour {

		public bool requiresEncampment = true;
		public int HousingCapacity;

		public override void Disable() {
			base.Disable();
			(ZoneBehaviour as Encampment)?.Specification.UpdateHousingCapacityBy(-HousingCapacity);
		}
		
		public override void Enable() {
			base.Enable();
			(ZoneBehaviour as Encampment)?.Specification.UpdateHousingCapacityBy(HousingCapacity);
		}
	}
}
