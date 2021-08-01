using UnityEngine;
using System.Collections.Generic;

public static class Stitcher {
	/// <summary>
	/// Stitch clothing onto an avatar.  Both clothing and avatar must be instantiated however clothing may be destroyed after.
	/// </summary>
	/// <param name="sourceClothing"></param>
	/// <param name="targetAvatar"></param>
	/// <returns>Newly created clothing on avatar</returns>
	public static GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar) {
		TransformCatalog boneCatalog = new TransformCatalog(targetAvatar.transform);

		SkinnedMeshRenderer[] skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();

		GameObject targetClothing = AddChild(sourceClothing, targetAvatar.transform);
		foreach (SkinnedMeshRenderer sourceRenderer in skinnedMeshRenderers) {
			SkinnedMeshRenderer targetRenderer = AddSkinnedMeshRenderer(sourceRenderer, targetClothing);
			targetRenderer.bones = TranslateTransforms(sourceRenderer.bones, boneCatalog);
		}

		for (int i = 0; i < targetClothing.transform.childCount; i++) {
			GameObject.Destroy(targetClothing.transform.GetChild(i).gameObject);
		}

		return targetClothing;
	}

	private static GameObject AddChild(GameObject source, Transform parent) {
		source.transform.parent = parent;
		source.transform.localPosition = Vector3.zero;
		source.transform.localRotation = Quaternion.identity;
		return source;
	}

	private static SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, GameObject parent) {
		SkinnedMeshRenderer target = parent.AddComponent<SkinnedMeshRenderer>();
		target.sharedMesh = source.sharedMesh;
		target.sharedMaterials = source.sharedMaterials;
		return target;
	}

	private static Transform[] TranslateTransforms(Transform[] sources, TransformCatalog transformCatalog) {
		Transform[] targets = new Transform[sources.Length];
		for (int index = 0; index < sources.Length; index++)
			targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
		return targets;
	}


	#region TransformCatalog
	private class TransformCatalog : Dictionary<string, Transform> {
		#region Constructors
		public TransformCatalog(Transform transform) {
			Catalog(transform);
		}
		#endregion

		#region Catalog
		private void Catalog(Transform transform) {
			try {
				Add(transform.name, transform);
			} catch {
			};
			foreach (Transform child in transform) {
				Catalog(child);
			}
		}
		#endregion
	}
	#endregion


	#region DictionaryExtensions
	private class DictionaryExtensions {
		public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key) {
			source.TryGetValue(key, out TValue value);
			return value;
		}
	}
	#endregion
}
