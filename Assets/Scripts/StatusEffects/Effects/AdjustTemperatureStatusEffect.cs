using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class AdjustTemperatureStatusEffect : OverTimeStatusEffect<AdjustTemperatureStatusEffect>, IStatusEffect {

		protected override void Routine() {
			foreach (Status status in Statuses) {
				if (!status.Actor.Dead) {
					status.Actor.Attributes.IncreaseTemperature(status.Magnitude);
				}
			}
		}
	}
}
