using ConstructionSystem;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EncampmentSystem {
	namespace AI {
		[RequireComponent(typeof(Encampment))]
		public class EncampmentAI : MonoBehaviour {

			public Texture2D TemplateTexture;

			private Map _templateMap;
			private Requirements _requirements;
			private Encampment _encampment;

			private List<Area> areas;

			private void Awake() {
				_encampment = GetComponent<Encampment>();
				_requirements = new Requirements();

				_templateMap = new Map(TemplateTexture.width, TemplateTexture);
				Tester2.Instance.DrawMap(_templateMap);

				areas = Map.GetAreasFromMap(_templateMap);
				foreach (var item in areas) {
					Debug.Log(item.Origin + " and " + item.Size);
				}
			}

			private void Routine() {
				/*
				if (_requirements.FreshWater <= _encampment.CurrentHousing) {
					BuildFreshWaterInfrastructure();
				}
				*/

				// Ration

				// Storage
				if (_requirements.StorageCapacity <= 5 + _encampment.CurrentHousing) {

				}
			}

			public void BuildFreshWaterInfrastructure() {
				switch (_encampment.EncampmentType) {
					case EncampmentType.CAMP:
						ConstructionManager.GetConstructionData("water pit");
						break;
				}
			}

			public void BuildStorageInfrastructure() {
				switch (_encampment.EncampmentType) {
					case EncampmentType.CAMP:
					case EncampmentType.CAMPMENT:
					case EncampmentType.ENCAMPMENT:
					case EncampmentType.VILLAGE:
					case EncampmentType.FORT:
					default:
						Foreman(ConstructionManager.GetConstructionData("basket"));
						break;
				}
			}

			public void Foreman(ConstructionData constructionData) {

			}
		}
	}
}
