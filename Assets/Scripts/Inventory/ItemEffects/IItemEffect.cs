namespace ItemSystem {
	namespace EffectSystem {
		public interface IItemEffect {

			void Activate(Actor actor, Item item, ItemEffect itemEffect);
		}
	}
}
