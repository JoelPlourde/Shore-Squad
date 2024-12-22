using System;
using UnityEngine;
using CraftingSystem;
using System.Collections.Generic;

namespace ItemSystem {
	namespace EffectSystem {
		[Serializable]
		public class ItemEffect {

			[SerializeField]
			public ItemEffectType ItemEffectType;

			[SerializeField]
			[Tooltip("If provided, it will override the name of the effect shown.")]
			public String effectName;

			// TODO Hide these field if the type is not EAT OR DRINK
			[SerializeField]
			[Tooltip("Magnitude, specific to the effect itself.")]
			public float Magnitude;

			// TODO Hide these field if the type is not STATUS EFFECT
			[Header("Status Effect")]
			[SerializeField]
			[Tooltip("The Duration the status effect will be applied for.")]
			public int Duration;
			[SerializeField]
			[Tooltip("The status effect that will be applied, if any.")]
			public StatusEffectSystem.StatusEffectData StatusEffectData;

			// TODO Hide this field if the type is not PROCESS
			[Header("Process")]
			[SerializeField]
			[Tooltip("The recipe that will be crafted, if any.")]
			public List<RecipeData> RecipeDatas;
		}
	}
}
