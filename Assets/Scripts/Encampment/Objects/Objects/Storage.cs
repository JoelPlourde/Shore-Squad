using ItemSystem;

namespace EncampmentSystem {
	public class Storage : ObjectBehaviour {

		public int Capacity;

		public override void Initialize() {
			base.Initialize();
			Inventory = new Inventory(Capacity);
		}

		public override void Enable() {
			base.Enable();
			Encampment encampment = ParentZone as Encampment;
			encampment.Specification.UpdateStorageCapacityBy(Capacity);
			encampment.Resources.RegisterStorage(this);
		}

		public override void Disable() {
			base.Disable();
			Encampment encampment = ParentZone as Encampment;
			encampment.Specification.UpdateStorageCapacityBy(-Capacity);
			encampment.Resources.UnregisterStorage(this);
		}

		public Inventory Inventory { get; private set; }
	}
}
