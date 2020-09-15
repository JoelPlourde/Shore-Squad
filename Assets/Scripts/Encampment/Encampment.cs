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

		[SerializeField]
		private int _housingCapacity = 0;           // Number of housing capacity of the encampment.

		[SerializeField]
		private int _storageCapacity = 0;			// Number of storage capacity of the encampment.

		[SerializeField]
		[Range(0, 50)]
		private int _currentHousing = 0;            // Number of bed that is assigned.

		private float _upkeepCost = 0;              // The upkeep cost of the encampment.

		public void AddToCoffer(int value) {
			coffer += value;
		}

		public void IncreaseHousingCapacityBy(int value) {
			_housingCapacity += value;
		}

		public void AdjustStorageCapacityBy(int value) {
			_storageCapacity += value;
		}

		public void CalculateInfluenceRadius() {
			InfluenceRadius = Constant.INFLUENCE_RADIUS_INCREMENT * _currentHousing + Constant.BASE_INFLUENCE_RADIUS;
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

		public int CurrentHousing { get { return _currentHousing; } }
		public int HousingCapacity { get { return _housingCapacity; } }
		public EncampmentType EncampmentType { get { return _encampmentType; } set { _encampmentType = value; } }
		public FactionType FactionType { get { return _factionType; } set { _factionType = value; } }
	}
}

