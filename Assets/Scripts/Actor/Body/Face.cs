using SaveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BodySystem {
	public class Face {

		private SkinnedMeshRenderer _skinnedMeshRenderer;

		private static readonly int COLUMNS = 4;
		private static readonly int ROWS = 2;

		public Dictionary<FeatureType, int> features = new Dictionary<FeatureType, int>() {
			{ FeatureType.EYES, 0},
			{ FeatureType.MOUTH, 0},
			{ FeatureType.EYEBROW, 0}
		};

		/// <summary>
		/// Initialize the Body
		/// </summary>
		/// <param name="skinnedMeshRenderer"></param>
		public void Initialize(SkinnedMeshRenderer skinnedMeshRenderer, FeaturesDto featuresDto) {
			_skinnedMeshRenderer = skinnedMeshRenderer;

			ChangeFeature(FeatureType.EYES, featuresDto.Eyes);
			ChangeFeature(FeatureType.MOUTH, featuresDto.Mouth);
			ChangeFeature(FeatureType.EYEBROW, featuresDto.Eyebrow);
		}

		/// <summary>
		/// Change the features of the character.
		/// </summary>
		/// <param name="featureType">The features to be modified</param>
		/// <param name="value">Value between 0 and 7.</param>
		public void ChangeFeature(FeatureType featureType, int value) {
			if (value < 0 || value > ((COLUMNS * ROWS) - 1)) {
				throw new UnityException("The value must be located between 0 and " + (COLUMNS * ROWS - 1) + ". Please verify your logic.");
			}

			Material[] materials = _skinnedMeshRenderer.materials;
			Material material = new Material(materials[(int)featureType]);
			Vector2 offset = material.GetTextureOffset("_MainTex");

			offset.x = (value % COLUMNS) * (1 / COLUMNS);
			offset.y = (value >= COLUMNS) ? (1 / ROWS) : 0;

			material.SetTextureOffset("_MainTex", offset);

			materials[(int)featureType] = material;
			_skinnedMeshRenderer.materials = materials;

			features[featureType] = value;
		}
	}
}
