using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class HungerStatusEffect : OverTimeStatusEffect<HungerStatusEffect>, IStatusEffect {

		protected override void Routine() {
			foreach (Status status in Statuses) {
				if (!status.Actor.Dead) {
					status.Actor.Attributes.ReduceFood(status.Actor.Attributes.HungerRate);
				}
			}
		}

		public override float TickRate {
			get { return 5f; }
		}
	}
}
