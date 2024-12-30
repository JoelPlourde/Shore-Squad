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
			HEAT_RESISTANCE,        // This value correlates with the ability to adapt to hot environment
			MINING_SPEED,           // This value correlates with the speed at which the character strike a mining node.
			MINING_STRENGTH,        // This value correlates with the damage inflicted to the mining node.
			LUCK,                   // This value correlates with the ability to critically strike a node.
			WOODWORKING_SPEED,		// This value correlates with the speed at which the character strike a lumber node.
			WOODWORKING_STRENGTH	// This value correlates with the damage inflicted to the lumber node.
		}
	}
}
