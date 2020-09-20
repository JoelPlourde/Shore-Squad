using UnityEngine;

namespace EncampmentSystem {
	public class WaterSource : ObjectBehaviour {

		[Tooltip("The maximum percentage of water in the terrain this water source can utilize.")]
		[Range(0, 100)]
		public int Value;

		public override void Disable() {
			base.Disable();
			// TODO FINISH THIS
			(ZoneBehaviour as Encampment)?.Specification.UpdateWaterCapacityBy(-Value);
		}

		public override void Enable() {
			base.Enable();
			(ZoneBehaviour as Encampment)?.Specification.UpdateWaterCapacityBy(Value);
		}
	}
}
