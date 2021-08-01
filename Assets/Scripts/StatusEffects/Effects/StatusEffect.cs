using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public abstract class StatusEffect<T> : Singleton<T> where T : IStatusEffect, new() {

		public List<Status> Statuses { get; private set; } = new List<Status>();


		public virtual void Apply(Status status) {
			Statuses.Add(status);
		}

		public virtual void Unapply(Status status) {
			Statuses.RemoveAll(x => x.Guid == status.Guid);
		}
	}

}