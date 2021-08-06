namespace ItemSystem {
	namespace EquipmentSystem {
		public class Equipment {

			public EquipmentData EquipmentData { get; private set; }
			public int Amount { get; private set; }

			public Equipment(EquipmentData equipmentData, int amount) {
				EquipmentData = equipmentData;
				Amount = amount;
			}
		}
	}
}
