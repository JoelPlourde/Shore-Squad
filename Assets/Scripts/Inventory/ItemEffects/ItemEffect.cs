using System;
using UnityEngine;

namespace ItemSystem {
	namespace EffectSystem {
		[Serializable]
		public class ItemEffect {

			[SerializeField]
			public ItemEffectType ItemEffectType;

			[SerializeField]
			public float Magnitude;
		}
	}
}
