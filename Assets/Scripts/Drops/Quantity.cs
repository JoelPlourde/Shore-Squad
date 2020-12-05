using System;
using UnityEngine;

namespace DropSystem {
	[Serializable]
	public class Quantity {

		[Tooltip("Minimum amount that the drop is going to roll for. [Inclusive]")]
		[SerializeField]
		public uint Min = 1;

		[Tooltip("Maximum amount that the drop is going to roll for. [Inclusive]")]
		[SerializeField]
		public uint Max = 1;
	}
}
