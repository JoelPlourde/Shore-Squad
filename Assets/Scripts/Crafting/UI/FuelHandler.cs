using ItemSystem;
using UI;

namespace CraftingSystem {
	namespace UI {
		public class FuelHandler : BaseHandler {

			public override bool IfCondition(Item item) {
				return item.ItemData.Burnable;
			}
		}
	}
}
