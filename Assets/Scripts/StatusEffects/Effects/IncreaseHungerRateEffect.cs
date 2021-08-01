using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class IncreaseHungerRateEffect : StatusEffect<IncreaseHungerRateEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Attributes.IncreaseHungerRate(status.Magnitude);
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Attributes.ReduceHungerRate(status.Magnitude);
		}
	}
}
