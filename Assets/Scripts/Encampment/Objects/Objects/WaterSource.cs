using UnityEngine;

namespace EncampmentSystem {
	public class WaterSource : ObjectBehaviour {

		[Tooltip("The maximum percentage of water in the terrain this water source can utilize.")]
		[Range(0, 100)]
		public int Value;

	}
}
