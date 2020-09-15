using UnityEngine;
using FactionSystem;

namespace EncampmentSystem {
	[RequireComponent(typeof(Encampment))]
	public class Flag : ObjectBehaviour {

		public Encampment Encampment { get; private set; }

		private void Awake() {
			Initialize();
			Enable();
		}

		public override void Initialize() {
			base.Initialize();
			Encampment = GetComponent<Encampment>();
		}

		public override void Disable() {
			base.Disable();
			gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = FactionManager.GetFactionData(Encampment.FactionType).Flag;
			FactionManager.UnregisterEncampment(Encampment.FactionType, Encampment);
			Encampment.FactionType = FactionType.FACTIONLESS;
		}

		public override void Enable() {
			base.Enable();
			Encampment.Map.DrawRectangle(NavMeshObstacle, Encampment, Color.red);
			gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = FactionManager.GetFactionData(Encampment.FactionType).Flag;
			FactionManager.RegisterEncampment(Encampment.FactionType, Encampment);
		}

		public override bool IsPlacementValid() {
			return true;
		}

		public override void RegisterZone(ZoneBehaviour zoneBehaviour) {
		}

		public override void UnregisterZone() {
		}
	}
}
