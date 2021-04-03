using UnityEngine;
using ConstructionSystem;
using EncampmentSystem;

public class Foreman : SingletonBehaviour<Foreman>
{
	private ConstructionBehaviour _constructionBehaviour;
	private RaycastHit _hit;
	private bool _isValid = false;

	[Tooltip("Layer on which objects can be placed in.")]
	public LayerMask _layerMask;

	public void StartPlacement(ConstructionData constructionData) {
		if (!IsInvoking()) {
			_constructionBehaviour = CreateConstructionBehaviour(constructionData);
			InvokeRepeating(nameof(Routine), 0, Constant.DEFAULT_REFRESH_RATE);
		}
	}

	public static ConstructionBehaviour CreateConstructionBehaviour(ConstructionData constructionData) {
		ConstructionBehaviour constructionBehaviour = Instantiate(constructionData.Prefab).AddComponent<ConstructionBehaviour>();
		constructionBehaviour.Initialize(constructionData, constructionBehaviour.GetComponent<IConstructable>());
		return constructionBehaviour;
	}

	public void Routine() {
		if (ReferenceEquals(_constructionBehaviour, null)) {
			CancelInvoke();
		}

		_isValid = false;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, Mathf.Infinity, _layerMask)) {
			_constructionBehaviour.transform.position = MapUtils.GetMapPositionFromWorldPosition(_hit.point);
			_isValid = _constructionBehaviour.Constructable.IsPlacementValid();
		}

		// Rotate
		if (Input.GetKeyUp(KeyCode.R)) {
			_constructionBehaviour.Rotate();
		}

		if (Input.GetMouseButtonUp(0)) {
			if (_isValid) {
				_constructionBehaviour.StartConstruction();
				StopPlacement();
			} else {
				Debug.Log("Placement is impossible.");
			}
		} else if (Input.GetMouseButtonUp(1)) {
			CancelPlacement();
		}
	}

	private void CancelPlacement() {
		StopPlacement();
		Destroy(_constructionBehaviour.gameObject);
	}

	private void StopPlacement() {
		CancelInvoke();
	}
}
