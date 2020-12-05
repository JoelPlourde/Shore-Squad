using UnityEngine;

namespace DropSystem {
	[System.Serializable]
	[CreateAssetMenu(fileName = "DropTable", menuName = "ScriptableObjects/Drop Table")]
	public class DropTable : ScriptableObject {

		[Tooltip("The drops that defines this drop table.")]
		[SerializeField]
		private Drop[] _drops;

		private int _sum;

		private void Awake() {
			ID = name.ToLower().Replace(" ", "_");
			_sum = 0;
			foreach (Drop drop in _drops) {
				_sum += (int)drop.Weight;
			}
		}

		/// <summary>
		/// Get a random drop based on the defined drop table.
		/// </summary>
		/// <returns>Return a single drop.</returns>
		public Drop GetRandomDrop() {
			int weight = 0;
			foreach (Drop drop in _drops) {
				weight += (int) drop.Weight;
				if (weight > Random.Range(0, _sum)) {
					return drop;
				}
			}
			return _drops[_drops.Length - 1];
		}

		public Drop[] Drops { get { return _drops; } }
		public string ID { get; set; }
	}
}
