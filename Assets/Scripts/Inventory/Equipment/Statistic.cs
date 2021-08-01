using System;
using UnityEngine;

namespace ItemSystem {
	namespace EquipmentSystem {
		[Serializable]
		public class Statistic {
			[SerializeField]
			[Tooltip("Indicates the type of statistic.")]
			public StatisticType StatisticType;

			[SerializeField]
			[Tooltip("Indicates the influence of the statistic")]
			public int Value;
		}
	}
}