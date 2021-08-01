using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class FearEffect : StatusEffect<FearEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Fleeing = true;
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Fleeing = false;
		}
	}
}