﻿using UnityEngine;

namespace StatusEffectSystem {
	public class SlowOverTimeEffect : OverTimeStatusEffect<SlowOverTimeEffect>, IStatusEffect {

		public override void Unapply(Status status) {
			status.Actor.Attributes.ResetSpeed(status.Magnitude * status.MaxDuration);
			base.Unapply(status);
		}

		protected override void Routine() {
			foreach (Status status in Statuses) {
				if (!status.Actor.Dead) {
					status.Actor.Attributes.ReduceSpeed(status.Magnitude * (status.MaxDuration - status.Duration + 1));
				}
			}
		}
	}
}
