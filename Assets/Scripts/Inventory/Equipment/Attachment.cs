using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
	namespace EquipmentSystem {
		public class Attachment {

			private GameObject _gameObject;

			public Attachment() {
				IsAttached = false;
				_gameObject = null;
			}

			public void Attach(Transform transform, Equipment equipment) {
				if (!IsAttached) {
					Equipment = equipment;

					GameObject prefab;
					if (Equipment.EquipmentData.EquipmentPrefab == null) {
						Debug.LogWarning("This Equipment prefab does not exist, a primitive will be used instead.");
						prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
					} else {
						prefab = Equipment.EquipmentData.EquipmentPrefab;
					}

					_gameObject = Object.Instantiate(prefab, transform);

					Stitcher.Stitch(_gameObject, transform.gameObject);

					IsAttached = true;
				} else {
					throw new UnityException("The attachment is already fulfill, please verify your logic.");
				}
			}

			public Equipment Detach() {
				if (IsAttached) {
					if (Application.isPlaying) {
						Object.Destroy(_gameObject);
					}

					// MIGHT be useless to set it to null.
					_gameObject = null;
					IsAttached = false;

					// Return the previous Equipment in the Attachment, it wont be overwritten yet.
					return Equipment;
				} else {
					throw new UnityException("The attachment is not filled, please verify your logic.");
				}
			}

			public bool IsAttached { get; private set; }
			public Equipment Equipment { get; private set; }
		}
	}
}
