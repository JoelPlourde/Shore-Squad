namespace ItemSystem {
	namespace EffectSystem {
		public class Eat : IItemEffect {

			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
				if (actor.Inventory.RemoveItemFromInventoryAtPosition(item.Index, 1)) {
					actor.Emotion.PlayEmote(EmoteSystem.EmoteType.EAT);

					actor.Attributes.IncreaseFood(itemEffect.Magnitude);
				}
			}
		}
	}
}
