using UnityEngine;
using FactionSystem;
using GameSystem;

namespace EncampmentSystem {
	public class Encampment : ZoneBehaviour {

		[SerializeField]
		public long coffer;							// The current value of currency in the coffers.

		[SerializeField]
		private FactionType _factionType = FactionType.FACTIONLESS;       // The Faction of the encampment.

		[SerializeField]
		private EncampmentType _encampmentType = EncampmentType.CAMP;       // Type of encampment

		private float _upkeepCost = 0;              // The upkeep cost of the encampment.

		[SerializeField]
		private Specification _specification = new Specification();

		public void AddToCoffer(int value) {
			coffer += value;
		}

		public void CalculateInfluenceRadius() {
			InfluenceRadius = Constant.INFLUENCE_RADIUS_INCREMENT * Specification.Population + Constant.BASE_INFLUENCE_RADIUS;
		}

		public void CalculateUpkeepCost() {
			// TODO : Refactor this to use a configuration file.
			switch (_factionType) {
				case FactionType.FACTIONLESS:
					_upkeepCost = 0;
					break;
				case FactionType.WILDLING:
					_upkeepCost = 0;
					break;
				case FactionType.SENTINELS:
					_upkeepCost = 5;
					break;
				case FactionType.INQUISITOR:
					_upkeepCost = 10;
					break;
				default:
					break;
			}
		}

		public Specification Specification { get { return _specification; } }
		public EncampmentType EncampmentType { get { return _encampmentType; } set { _encampmentType = value; } }
		public FactionType FactionType { get { return _factionType; } set { _factionType = value; } }
	}
}

