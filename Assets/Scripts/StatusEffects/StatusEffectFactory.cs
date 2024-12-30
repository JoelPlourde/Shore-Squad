using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public static class StatusEffectFactory {

		private readonly static Dictionary<StatusEffectType, IStatusEffect> statusEffects = new Dictionary<StatusEffectType, IStatusEffect>() {
			{ StatusEffectType.DAMAGE_OVER_TIME, DamageOverTimeEffect.Instance },
			{ StatusEffectType.SLOW, SlowEffect.Instance },
			{ StatusEffectType.SLOW_OVER_TIME, SlowOverTimeEffect.Instance },
			{ StatusEffectType.HEALTH_REGENERATION, HealthRegenerationStatusEffect.Instance },
			{ StatusEffectType.HEAL_OVER_TIME, HealOverTimeEffect.Instance },
			{ StatusEffectType.STUN, StunEffect.Instance },
			{ StatusEffectType.SHELTER, ShelterEffect.Instance },
			{ StatusEffectType.FEAR, FearEffect.Instance },
			{ StatusEffectType.INCREASE_HUNGER_RATE, IncreaseHungerRateEffect.Instance },
			{ StatusEffectType.REDUCE_HUNGER_RATE, ReduceHungerRateEffect.Instance },
			{ StatusEffectType.HUNGER_OVER_TIME, HungerStatusEffect.Instance },
			{ StatusEffectType.DEATH, DeathEffect.Instance }
		};

		public static IStatusEffect GetStatusEffect(StatusEffectType statusEffectType) {
			if (statusEffects.TryGetValue(statusEffectType, out IStatusEffect statusEffect)) {
				return statusEffect;
			} else {
				throw new UnityException("This Status Effect Type has not yet been implemented.");
			}
		}
	}
}
