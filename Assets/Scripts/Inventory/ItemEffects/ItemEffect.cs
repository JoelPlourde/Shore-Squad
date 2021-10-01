using System;
using UnityEngine;

namespace ItemSystem {
	namespace EffectSystem {
		[Serializable]
		public class ItemEffect {

			[SerializeField]
			public ItemEffectType ItemEffectType;

			[SerializeField]
			[Tooltip("Magnitude, specific to the effect itself.")]
			public float Magnitude;

			[Header("Status Effect")]
			[SerializeField]
			[Tooltip("The Duration the status effect will be applied for.")]
			public int Duration;

			[SerializeField]
			[Tooltip("The status effect that will be applied, if any.")]
			public StatusEffectSystem.StatusEffectData StatusEffectData;
		}
	}
}
