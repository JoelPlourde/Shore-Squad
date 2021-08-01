using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class DeathEffect : StatusEffect<DeathEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Dead = true;
			status.Actor.OnDeath();
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Dead = false;
			status.Actor.OnResurrection();
		}
	}
}
