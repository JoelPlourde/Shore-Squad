using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EncampmentSystem {
	[System.Serializable]
	public class Specification {

		public int MilitaryStrength;
		public int Population;
		public int HousingCapacity;
		public int StorageCapacity;
		public int RationCapacity;
		public int WaterCapacity;
		public int HeatCapacity;

		public void UpdateMilitaryStrength(int value) {
			MilitaryStrength += value;
		}

		public void UpdatePopulation(int value) {
			Population += value;
		}

		public void UpdateHousingCapacityBy(int value) {
			HousingCapacity += value;
		}

		public void UpdateStorageCapacityBy(int value) {
			StorageCapacity += value;
		}

		public void UpdateRationCapacityBy(int value) {
			RationCapacity += value;
		}

		public void UpdateWaterCapacityBy(int value) {
			WaterCapacity += value;
		}

		public void UpdateHeatCapacityBy(int value) {
			HeatCapacity += value;
		}
	}
}
