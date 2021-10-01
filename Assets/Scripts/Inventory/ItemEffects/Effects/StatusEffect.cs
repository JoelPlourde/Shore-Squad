using StatusEffectSystem;
using UnityEngine;

namespace ItemSystem {
	namespace EffectSystem {
		public class StatusEffect : IItemEffect {

			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
				actor.StatusEffectScheduler.AddStatusEffect(new StatusEffectSystem.Status(actor, itemEffect.Magnitude, itemEffect.Duration, itemEffect.StatusEffectData));
				actor.Inventory.RemoveItemFromInventoryAtPosition(item.Index, 1);
			}
		}
	}
}
