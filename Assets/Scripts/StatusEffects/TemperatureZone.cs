using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public class TemperatureZone : StatusEffectZone {

		public TemperatureZoneData TemperatureZoneData;

		private StatusEffectData _oppositeStatusEffectData;
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
			_statusEffectData = temperatureZoneData.statusEffectData;
			_oppositeStatusEffectData = temperatureZoneData.OppositeStatusEffectData;
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
				int duration = (int) Math.Abs(_targetTemperature - actor.Attributes.Temperature);
				AdjustTemperature(actor, _magnitude, duration, _statusEffectData.Name, _statusEffectData);
			}
		}

		protected override void OnTriggerExit(Collider other) {
			if (ReferenceEquals(_initialized, false)) {
				return;
			}

			Debug.Log("Trigger Exit, removing status effect");
			Actor actor = other.gameObject.GetComponent<Actor>();
			if (!ReferenceEquals(actor, null)) {
				int duration = (int) Math.Abs(Constant.DEFAULT_TEMPERATURE - actor.Attributes.Temperature);
				AdjustTemperature(actor, -_magnitude, duration, _statusEffectData.Name, _oppositeStatusEffectData);
			}
		}

		private void AdjustTemperature(Actor actor, float magnitude, int duration, string name, StatusEffectData statusEffectData) {
			StatusEffectScheduler.Instance(actor.Guid).RemoveStatusEffect(name);
			StatusEffectScheduler.Instance(actor.Guid).AddStatusEffect(new Status(actor, magnitude, duration, statusEffectData));
		}
	}
}
