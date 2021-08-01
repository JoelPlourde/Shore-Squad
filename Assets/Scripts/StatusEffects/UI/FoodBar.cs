using UnityEngine;

namespace UI {
	namespace Portrait {
		public class FoodBar : Bar {

			public void Initialize(Actor actor) {
				UpdateQuarter(actor.Attributes.Food);
				actor.Attributes.OnUpdateFoodEvent += UpdateQuarter;
			}

			public void OnDelete(Actor actor) {
				actor.Attributes.OnUpdateFoodEvent -= UpdateQuarter;
			}
		}
	}
}
