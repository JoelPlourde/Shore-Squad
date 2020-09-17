﻿using UnityEngine;
using ConstructionSystem;

public class Foreman : MonoBehaviour
{
	public static Foreman Instance;

	private ConstructionBehaviour _constructionBehaviour;

	private RaycastHit _hit;

	private bool _isValid = false;

	[Tooltip("Layer on which objects can be placed in.")]
	public LayerMask _layerMask;

    private void Awake() {
		if (Instance != null) {
			throw new UnityException("Foreman is a singleton, multiple instances is not allowed.");
		}
		Instance = this;
    }

	public void StartPlacement(ConstructionData constructionData) {
		if (!IsInvoking()) {

			// Create an object based on the construction received.
			CreateConstructionBehaviour(constructionData);

			InvokeRepeating(nameof(Routine), 0, Constant.DEFAULT_REFRESH_RATE);
		}
	}

	private void CreateConstructionBehaviour(ConstructionData constructionData) {
		_constructionBehaviour = Instantiate(constructionData.Prefab).AddComponent<ConstructionBehaviour>();
		_constructionBehaviour.Initialize(constructionData, _constructionBehaviour.GetComponent<IConstructable>());
	}

	public void Routine() {
		if (_constructionBehaviour == null) {
			CancelInvoke();
		}

		_isValid = false;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, Mathf.Infinity, _layerMask)) {
			_constructionBehaviour.transform.position = _hit.point;
			_isValid = _constructionBehaviour.Constructable.IsPlacementValid();
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