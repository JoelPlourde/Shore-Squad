namespace ItemSystem {
	namespace EffectSystem {
		public class Drink : IItemEffect {

			// This is just a placeholder for now, 
			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
				// Do whatever the drink does.


				actor.Emotion.PlayEmote(EmoteSystem.EmoteType.DRINK);

				actor.Inventory.RemoveItemFromInventoryAtPosition(item.Index, 1);
			}
		}
	}
}