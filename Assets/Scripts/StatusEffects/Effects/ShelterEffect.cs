using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class ShelterEffect : StatusEffect<ShelterEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Sheltered = true;
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Sheltered = false;
		}
	}
}
