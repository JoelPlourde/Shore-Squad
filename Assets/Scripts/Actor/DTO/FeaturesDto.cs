using BodySystem;
using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class FeaturesDto {

		[SerializeField]
		public int Sex;

		[SerializeField]
		public int Eyes;

		[SerializeField]
		public int Mouth;

		[SerializeField]
		public int Eyebrow;

		[SerializeField]
		public int Hairstyle;

		[SerializeField]
		public string HairColor;

		[SerializeField]
		public string SkinColor;

		[SerializeField]
		public string UnderwearColor;

		public FeaturesDto() {
			Sex = 0;
			Eyes = 0;
			Mouth = 0;
			Eyebrow = 0;
			Hairstyle = 0;
			HairColor = "#FFEB04";
			SkinColor = "#B98D50";
			UnderwearColor = "#FFFFFF";
		}

		public FeaturesDto(Body body, Face face) {
			Sex = (int)body.SexType;
			Eyes = face.features[FeatureType.EYES];
			Mouth = face.features[FeatureType.MOUTH];
			Eyebrow = face.features[FeatureType.EYEBROW];
			// TODO HAIR
			SkinColor = "#" + ColorUtility.ToHtmlStringRGB(body.CharacteristicsColor[Characteristic.SKIN]);
			UnderwearColor = "#" + ColorUtility.ToHtmlStringRGB(body.CharacteristicsColor[Characteristic.UNDERWEAR]);
			HairColor = "#" + ColorUtility.ToHtmlStringRGB(body.CharacteristicsColor[Characteristic.HAIR]);
		}
	}
}
