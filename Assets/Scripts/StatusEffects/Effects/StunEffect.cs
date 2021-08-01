using System.Collections.Generic;

namespace StatusEffectSystem {
	public class StunEffect : StatusEffect<StunEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Stunned = true;
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Stunned = false;
		}
	}
}
