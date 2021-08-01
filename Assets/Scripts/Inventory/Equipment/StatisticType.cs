namespace ItemSystem {
	namespace EquipmentSystem {
		public enum StatisticType {
			ARMOR,                  // This value correlates with the damage reduction.
			CONSTITUTION,           // This value indicates the amount of health, health regeneration
			STRENGTH,               // This value is linked to melee-based attacks
			INTELLIGENCE,           // This value is linked to magic-based attacks
			DEXTERITY,              // This value is linked to ranged-based attacks.
			FAITH,                  // This value is linked to protection-based spells
			COLD_RESISTANCE,        // This value correlates with the ability to adapt to cold environment
			HEAT_RESISTANCE         // This value correlates with the ability to adapt to hot environment
		}
	}
}
