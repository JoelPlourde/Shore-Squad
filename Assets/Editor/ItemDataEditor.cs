using ItemSystem.EffectSystem;
using UnityEditor;

namespace ItemSystem {
	[CustomEditor(typeof(ItemData))]
	public class ItemDataEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			ItemData itemData = (ItemData) target;

			if (itemData.ItemType == ItemType.CONSUMABLE) {
				if (itemData.ItemEffects.Count == 0) {
					itemData.ItemEffects.Add(new ItemEffect {
						ItemEffectType = ItemEffectType.EAT,
						Magnitude = 1
					});
				}
			}
		}
	}
}
