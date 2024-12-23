using UnityEngine;
using System.Collections.Generic;

public class InteractableBehavior : MonoBehaviour {

    private MeshRenderer _meshRenderer;

    private int RENDER_QUEUE = 3000;

    protected void Awake() {
        // Find the mesh renderer
        _meshRenderer = GetComponent<MeshRenderer>();

        if (ReferenceEquals(_meshRenderer, null)) {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }

    public void OnMouseEnter() {
        if (ReferenceEquals(_meshRenderer, null)) {
            Debug.LogWarning("No MeshRenderer found on this object.");
            return;
        }

        List<Material> materials = new List<Material>(_meshRenderer.materials);
        foreach (Material material in materials) {
            material.renderQueue = RENDER_QUEUE + 1000;
        }
        materials.Add(new Material(InteractionManager.Instance.GetOutlineMaterial(GetOutlineType())) {
            renderQueue = RENDER_QUEUE
        });
        _meshRenderer.materials =  materials.ToArray();
    }

    public void OnMouseExit() {
        List<Material> materials = new List<Material>(_meshRenderer.materials);
        foreach (Material material in materials) {
            material.renderQueue = RENDER_QUEUE - 1000;
        }
        materials.RemoveAt(materials.Count - 1);
        _meshRenderer.materials =  materials.ToArray();
    }

    protected virtual OutlineType GetOutlineType() {
        return OutlineType.INTERACTABLE;
    }
}