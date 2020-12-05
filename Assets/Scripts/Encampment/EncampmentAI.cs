using ConstructionSystem;
using GameSystem;
using System.Collections.Generic;
using UnityEngine;

namespace EncampmentSystem {
	namespace AI {
		[RequireComponent(typeof(Encampment))]
		public class EncampmentAI : MonoBehaviour {

			public Texture2D TemplateTexture;

			private Map _templateMap;
			private Encampment _encampment;

			private List<Area> _availableAreas;

			private int i = 0;

			private void Awake() {
				_encampment = GetComponent<Encampment>();

				_templateMap = new Map(TemplateTexture.width, TemplateTexture.height, TemplateTexture);
				_availableAreas = Map.GetAreasFromMap(_templateMap);
			}

			public void Update() {
				if (Input.GetKeyUp(KeyCode.Y)) {
					Routine();
				}
			}

			private void Routine() {
				if (i < 1) {
					BuildInfrastructure();
				} else {
					BuildFreshWaterInfrastructure();
					BuildHousingInfrastructure();
					BuildStorageInfrastructure();
				}

				i++;
				/*
				if (_encampment.Specification.WaterCapacity <= _encampment.Specification.Population) {
					BuildFreshWaterInfrastructure();
				} else if (_encampment.Specification.StorageCapacity <= 5 + _encampment.Specification.Population) {
					BuildStorageInfrastructure();
				} else if (_encampment.Specification.HousingCapacity <= _encampment.Specification.Population) {
					BuildHousingInfrastructure();
				}*/
			}

			/// <summary>
			/// Register new areas for the AI to place object in.
			/// </summary>
			/// <param name="areas"></param>
			public void RegisterAreas(List<Area> areas) {
				foreach (var area in areas) {
					if (!_availableAreas.Contains(area)) {
						_availableAreas.Add(area);
					} else {
						Debug.Log(area);
						Debug.Log(_availableAreas.FindIndex(0, x => x.Equals(area)));
						Debug.Log("Area is already registered in the available areas.");
					}
				}
			}

			/// <summary>
			/// Place an object unto the map based on the available areas.
			/// </summary>
			/// <param name="constructionData"></param>
			public void Construct(ConstructionData constructionData) {
				ConstructionBehaviour constructionBehaviour = Foreman.CreateConstructionBehaviour(constructionData);
				ObjectBehaviour objectBehaviour = constructionBehaviour.GetComponent<ObjectBehaviour>();
				objectBehaviour.Initialize();

				bool placed = false;
				List<Area> splitArea = new List<Area>();
				int count = _availableAreas.Count;
				while (count > 0) {
					// Generate a random index to check if the areas is ok.
					int i = Random.Range(0, _availableAreas.Count);
					if (_availableAreas[i].ObstacleCanFitInTheArea(objectBehaviour.Obstacle, out bool rotate)) {

						Debug.Log("The obstacle " + objectBehaviour.Obstacle.Size + " can fit in: " + _availableAreas[i] + " with rotation: " + rotate);

						// If the Obstacle fits if the area is rotated, rotate it.
						if (rotate) {
							constructionBehaviour.Rotate();
						}

						// Remove the obstacle from the area.
						if (Area.RemoveObstacleFromArea(_availableAreas[i], objectBehaviour.Obstacle, ref splitArea)) {
							Vector3 origin = Map.GetMapOriginInWorldPos(_encampment.Map, _encampment.transform.position);
							Vector3 objectPos = new Vector3(_availableAreas[i].Origin.x, 0, _availableAreas[i].Origin.y);
							Vector3 pos = objectPos + origin + (new Vector3(_availableAreas[i].Size.x, 0, _availableAreas[i].Size.y) / 2);

							// TODO get height at position on terrain.
							constructionBehaviour.transform.position = pos;

							if (!_encampment.Map.IsPositionValid(objectBehaviour.Obstacle, _encampment)) {
								break;
							}


							objectBehaviour.RegisterZone(_encampment);
							constructionBehaviour.StartConstruction();

							_availableAreas.Remove(_availableAreas[i]);
							RegisterAreas(splitArea);
							placed = true;
							break;
						} else {
							throw new UnityException("It should work.");
						}
					} else {
						Debug.Log("The obstacle " + objectBehaviour.Obstacle.Size + " cannot fit in: " + _availableAreas[i]);
					}
					count--;
				}

				if (!placed) {
					Destroy(constructionBehaviour.gameObject);
				}
			}

			void OnDrawGizmos() {
				if (Application.isPlaying) {
					foreach (var area in _availableAreas) {
						Vector3 origin = Map.GetMapOriginInWorldPos(_encampment.Map, _encampment.transform.position);
						Vector3 areaHalfSize = new Vector3(area.Size.x / 2, 0, area.Size.y / 2);
						Gizmos.color = Color.yellow;
						Gizmos.DrawWireCube(origin + areaHalfSize + new Vector3(area.Origin.x, 0, area.Origin.y), new Vector3(area.Size.x, 1, area.Size.y));
					}
				}
			}


			public void BuildFreshWaterInfrastructure() {
				switch (_encampment.EncampmentType) {
					case EncampmentType.CAMP:
						Construct(ConstructionManager.GetConstructionData("pit"));
						break;
					case EncampmentType.CAMPMENT:
					case EncampmentType.ENCAMPMENT:
					case EncampmentType.VILLAGE:
					case EncampmentType.FORT:
						Construct(ConstructionManager.GetConstructionData("well"));
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
						Construct(ConstructionManager.GetConstructionData("bed"));
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
						Construct(ConstructionManager.GetConstructionData("chest"));
						break;
				}
			}

			public void BuildInfrastructure() {
				switch (_encampment.EncampmentType) {
					case EncampmentType.CAMP:
					case EncampmentType.CAMPMENT:
					case EncampmentType.ENCAMPMENT:
					case EncampmentType.VILLAGE:
					case EncampmentType.FORT:
					default:
						Construct(ConstructionManager.GetConstructionData("shed"));
						break;
				}
			}
		}
	}
}
