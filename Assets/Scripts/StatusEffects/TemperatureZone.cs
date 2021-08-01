using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class TemperatureZone : StatusEffectZone {

		public StatusEffectData OppositeStatusEffectData;

		[Range(-60f, 100f)]
		public float TargetTemperature = 20f;

		protected override void OnTriggerEnter(Collider other) {
			Actor actor = other.gameObject.GetComponent<Actor>();
			if (!ReferenceEquals(actor, null)) {
				AdjustTemperature(actor, (TargetTemperature - actor.Attributes.Temperature) / Constant.TIME_BETWEEN_TEMPERATURE_CHANGES, StatusEffectData.Name, StatusEffectData);
			}
		}

		protected override void OnTriggerExit(Collider other) {
			Actor actor = other.gameObject.GetComponent<Actor>();
			if (!ReferenceEquals(actor, null)) {
				AdjustTemperature(actor, (Constant.DEFAULT_TEMPERATURE - actor.Attributes.Temperature) / Constant.TIME_BETWEEN_TEMPERATURE_CHANGES, StatusEffectData.Name, OppositeStatusEffectData);
			}
		}

		private void AdjustTemperature(Actor actor, float magnitude, string name, StatusEffectData statusEffectData) {
			StatusEffectScheduler.Instance(actor.Guid).RemoveStatusEffect(name);
			StatusEffectScheduler.Instance(actor.Guid).AddStatusEffect(new Status(actor, magnitude, statusEffectData));
		}
	}
}
