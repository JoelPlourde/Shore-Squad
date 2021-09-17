using System.Collections.Generic; 

namespace ItemSystem {
	namespace EffectSystem {
		public class Eat : IItemEffect {

			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
				actor.Attributes.IncreaseFood(itemEffect.Magnitude);

				actor.Inventory.RemoveItemFromInventoryAtPosition(item.Index, 1);
			}
		}
	}
}
