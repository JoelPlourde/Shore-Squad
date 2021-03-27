using UnityEngine;
using EncampmentSystem;
using ConstructionSystem;
using TaskSystem;

namespace ConstructionSystem {
	[RequireComponent(typeof(ObjectBehaviour))]
	public class ConstructionBehaviour : MonoBehaviour {
		private ObjectBehaviour _objectBehaviour;

		public void Initialize(ConstructionData constructionData, IConstructable constructable) {
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			ConstructionData = constructionData;
			Constructable = constructable;
			Constructable.Initialize();
		}

		public void StartConstruction() {
			_objectBehaviour = GetComponent<ObjectBehaviour>();

			// Tell the encampment to register a Construct task for this object.
			(_objectBehaviour.ParentZone as Encampment)?.RegisterTask(new ConstructArguments(this));

			// Draw the object on the map to prevent further object to placed here.
			_objectBehaviour.ZoneBehaviour.Map.DrawRectangle(_objectBehaviour.Obstacle, _objectBehaviour.ZoneBehaviour, Color.red);

			if (ConstructionData.ConstructionTime == 0) {
				Construct();
				return;
			}
		}

		// RETURN BOOL TO DETERMINE IF YOU ARE DONE.
		public bool Progress() {
			if (ConstructionData.ConstructionTime == 0) {
				Construct();
				return Done;
			}

			CurrentProgress += 100 / ConstructionData.ConstructionTime;
			if (CurrentProgress >= 100) {
				Construct();
				return Done;
			}

			return Done;
		}

		private void Construct() {
			Done = true;
			Constructable.Enable();
			StopConstruction();
		}

		public void StopConstruction() {
			CancelInvoke();
			Destroy(this);
		}

		public void Rotate() {
			Constructable.Rotate();
		}

		public float CurrentProgress { get; private set; }
		public bool Done { get; private set; }
		public ConstructionData ConstructionData { get; private set; }
		public IConstructable Constructable { get; private set; }
	}
}
