using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
	namespace EquipmentSystem {
		[Serializable]
		public class EquipmentStats {

			[SerializeField]
			public List<Statistic> Statistics = new List<Statistic>(1);
		}
	}
}
