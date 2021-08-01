using UnityEngine;

namespace StatusEffectSystem {
	[RequireComponent(typeof(Collider))]
	public class StatusEffectZone : MonoBehaviour {

		[Range(0.0f, 10.0f)]
		public float Magnitude = 1.0f;
		public StatusEffectData StatusEffectData;

		protected Status _status;
		private Collider _collider;

		private void Awake() {
			_collider = GetComponent<Collider>();
			_collider.isTrigger = true;
		}

		protected virtual void OnTriggerEnter(Collider other) {
			Actor actor = other.gameObject.GetComponent<Actor>();
			if (!ReferenceEquals(actor, null)) {
				_status = new Status(actor, Magnitude, StatusEffectData);
				StatusEffectScheduler.Instance(actor.Guid).AddStatusEffect(_status);
			}
		}

		protected virtual void OnTriggerExit(Collider other) {
			Actor actor = other.gameObject.GetComponent<Actor>();
			if (!ReferenceEquals(actor, null)) {
				StatusEffectScheduler.Instance(actor.Guid).RemoveStatusEffect(_status.StatusEffectData.Name);
			}
		}
	}
}
