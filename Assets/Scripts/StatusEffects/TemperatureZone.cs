using System;
using UnityEngine;

namespace StatusEffectSystem {
	public class TemperatureZone : StatusEffectZone {

		public TemperatureZoneData TemperatureZoneData;

		private float _targetTemperature;
		private bool _initialized = false;

		private void Start() {
			if (ReferenceEquals(TemperatureZoneData, null)) {
				Debug.LogError("TemperatureZoneData is not set for " + gameObject.name);
				return;
			}
			Initialize(TemperatureZoneData);
		}

		public void Initialize(TemperatureZoneData temperatureZoneData, float radius = 10.0f) {
			_magnitude = temperatureZoneData.Magnitude;
			_targetTemperature = temperatureZoneData.TargetTemperature;

			if (_collider is SphereCollider sphereCollider) {
				sphereCollider.radius = radius;
			}

			_initialized = true;
		}

		protected override void OnTriggerEnter(Collider other) {
			if (ReferenceEquals(_initialized, false)) {
				return;
			}

			Actor actor = other.gameObject.GetComponent<Actor>();

			if (!ReferenceEquals(actor, null)) {
				float duration = Math.Abs((_targetTemperature - actor.Attributes.Temperature) / _magnitude);
				AdjustTemperature(actor, _targetTemperature, duration);
			}
		}

		protected override void OnTriggerExit(Collider other) {
			if (ReferenceEquals(_initialized, false)) {
				return;
			}

			Actor actor = other.gameObject.GetComponent<Actor>();
			if (!ReferenceEquals(actor, null)) {
				float duration = Math.Abs((Constant.DEFAULT_TEMPERATURE - actor.Attributes.Temperature) / _magnitude);
				AdjustTemperature(actor, -_targetTemperature, duration);
			}
		}

		private void AdjustTemperature(Actor actor, float targetTemperature, float duration) {
			actor.Attributes.AdjustTemperature(targetTemperature, duration);
		}
	}
}
