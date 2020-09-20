using UnityEngine;
using EncampmentSystem;
using ConstructionSystem;

public class ConstructionBehaviour : MonoBehaviour
{
	[Range(0, 100)]
	public float progress;

	public void Initialize(ConstructionData constructionData, IConstructable constructable) {
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		ConstructionData = constructionData;
		Constructable = constructable;
		Constructable.Initialize();
	}

	public void StartConstruction() {
		if (ConstructionData.ConstructionTime == 0) {
			Construct();
			return;
		}

		if (!IsInvoking()) {
			InvokeRepeating(nameof(Routine), 0f, 1f);
		}
	}

	private void Routine() {
		progress += 100 / ConstructionData.ConstructionTime;
		if (progress >= 100) {
			Construct();
		}
	}

	private void Construct() {
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

	public ConstructionData ConstructionData { get; private set; }
	public IConstructable Constructable { get; private set; }
}
