using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public interface IStatusEffect {

		List<Status> Statuses { get; }

		void Apply(Status status);

		void Unapply(Status status);
	}
}
