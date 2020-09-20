using ConstructionSystem;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EncampmentSystem {
	namespace AI {
		[RequireComponent(typeof(Encampment))]
		public class EncampmentAI : MonoBehaviour {

			public Texture2D TemplateTexture;

			private Map _templateMap;
			private Encampment _encampment;

			private List<Area> areas;

			private void Awake() {
				_encampment = GetComponent<Encampment>();

				_templateMap = new Map(TemplateTexture.width, TemplateTexture);
				Tester.Instance.DrawMap(_templateMap);
				areas = Map.GetAreasFromMap(_templateMap);
			}

			public void Update() {
				if (Input.GetKeyUp(KeyCode.Y)) {
					Routine();
				}
			}

			private void Routine() {
				int i = Random.Range(0, 3);
				if (i == 0) {
					BuildFreshWaterInfrastructure();
				} else if (i == 1) {
					BuildStorageInfrastructure();
				} else if (i == 2) {
					BuildHousingInfrastructure();
				}
				/*
				if (_encampment.Specification.WaterCapacity <= _encampment.Specification.Population) {
					BuildFreshWaterInfrastructure();
				} else if (_encampment.Specification.StorageCapacity <= 5 + _encampment.Specification.Population) {
					BuildStorageInfrastructure();
				} else if (_encampment.Specification.HousingCapacity <= _encampment.Specification.Population) {
					BuildHousingInfrastructure();
				}*/
			}

			public void BuildFreshWaterInfrastructure() {
				switch (_encampment.EncampmentType) {
					case EncampmentType.CAMP:
						Foreman(ConstructionManager.GetConstructionData("pit"));
						break;
					case EncampmentType.CAMPMENT:
					case EncampmentType.ENCAMPMENT:
					case EncampmentType.VILLAGE:
					case EncampmentType.FORT:
						Foreman(ConstructionManager.GetConstructionData("well"));
						break;
				}
			}

			public void BuildHousingInfrastructure() {
				switch (_encampment.EncampmentType) {
					case EncampmentType.CAMP:
					case EncampmentType.CAMPMENT:
					case EncampmentType.ENCAMPMENT:
					case EncampmentType.VILLAGE:
					case EncampmentType.FORT:
					default:
						Foreman(ConstructionManager.GetConstructionData("bed"));
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
				GameObject @object = Instantiate(constructionData.Prefab);
				ObjectBehaviour objectBehaviour = @object.GetComponent<ObjectBehaviour>();
				objectBehaviour.Initialize();
				Vector2Int size = Map.GetSizeFromObstacle(objectBehaviour.Obstacle);

				bool placed = false;
				List<Area> splitArea = new List<Area>();
				int count = areas.Count;
				while (count > 0) {
					int i = Random.Range(0, areas.Count);
					if (areas[i].IsLessOrEquals(size, out bool rotate)) {

						if (Area.RemoveRectangleFromArea(areas[i], size, ref splitArea)) {
							Vector2Int objectPos = new Vector2Int(-(_encampment.Map.Size.x >> 1), -(_encampment.Map.Size.y >> 1));		// Account for zone size
							objectPos += new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));// Account for zone world position
							objectPos += areas[i].Origin;																				// Account for the area origin
							objectPos += new Vector2Int(size.x >> 1, size.y >> 1);														// Account for the object size

							// TODO get height at position on terrain.
							@object.transform.position = new Vector3(objectPos.x, transform.position.y, objectPos.y);

							if (!_encampment.Map.IsPositionValid(objectBehaviour.Obstacle, _encampment)) {
								count--;
								continue;
							}

							objectBehaviour.RegisterZone(_encampment);
							objectBehaviour.Enable();
							placed = true;
							areas.RemoveAt(i);
							break;
						} else {
							throw new UnityException("It should work.");
						}
					}
					count--;
				}

				if (!placed) {
					Destroy(@object);
				}

				if (splitArea.Count > 0) {
					areas.AddRange(splitArea);
				}
			}
		}
	}
}
