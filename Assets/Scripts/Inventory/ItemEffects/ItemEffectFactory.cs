using System.Collections.Generic;
using UnityEngine;
using System;

namespace ItemSystem {
	namespace EffectSystem {
		public static class ItemEffectFactory {

			private readonly static Dictionary<ItemEffectType, Type> _itemEffects = new Dictionary<ItemEffectType, Type>() {
				{ ItemEffectType.EQUIP, typeof(Equip)},
				{ ItemEffectType.EAT, typeof(Eat) },
				{ ItemEffectType.DRINK, typeof(Drink) },
				{ ItemEffectType.STATUS_EFFECT, typeof(StatusEffect) },
				{ ItemEffectType.PROCESS, typeof(Process) }
			};

			public static void Activate(Actor actor, Item item) {
				foreach (ItemEffect itemEffect in item.ItemData.ItemEffects) {
					if (_itemEffects.TryGetValue(itemEffect.ItemEffectType, out Type effectType)) {
						// Create a new instance of the effect type
						IItemEffect effect = (IItemEffect)Activator.CreateInstance(effectType);

						effect.Activate(actor, item, itemEffect);
					} else {
						throw new UnityException("This ItemEffectType is not yet implemented: " + itemEffect.ItemEffectType);
					}
				}
			}
		}
	}
}