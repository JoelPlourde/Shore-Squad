using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
	namespace EffectSystem {
		public static class ItemEffectFactory {

			private readonly static Dictionary<ItemEffectType, IItemEffect> _itemEffects = new Dictionary<ItemEffectType, IItemEffect>() {
				{ ItemEffectType.EQUIP, new Equip()},
				{ ItemEffectType.EAT, new Eat() },
				{ ItemEffectType.DRINK, new Drink() },
				{ ItemEffectType.STATUS_EFFECT, new StatusEffect() }
			};

			public static void Activate(Actor actor, Item item) {
				foreach (ItemEffect itemEffect in item.ItemData.ItemEffects) {
					if (_itemEffects.TryGetValue(itemEffect.ItemEffectType, out IItemEffect effect)) {
						effect.Activate(actor, item, itemEffect);
					} else {
						throw new UnityException("This ItemEffectType is not yet implemented: " + itemEffect.ItemEffectType);
					}
				}
			}
		}
	}
}