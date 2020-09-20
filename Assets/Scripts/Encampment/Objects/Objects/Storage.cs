namespace EncampmentSystem {
	public class Storage : ObjectBehaviour {

		public int Capacity;

		public override void Disable() {
			base.Disable();
			(ZoneBehaviour as Encampment)?.Specification.UpdateStorageCapacityBy(-Capacity);
		}

		public override void Enable() {
			base.Enable();
			(ZoneBehaviour as Encampment)?.Specification.UpdateStorageCapacityBy(Capacity);
		}
	}
}
