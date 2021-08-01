using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class SlowEffect : StatusEffect<SlowEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Attributes.ReduceSpeed(status.Magnitude);
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Attributes.ResetSpeed(status.Magnitude);
		}
	}
}
