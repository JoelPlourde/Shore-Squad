using ConstructionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConstructionSystem {
	namespace UI {
		public class ConstructionManagerComponent : MonoBehaviour {

			private void Awake() {
				foreach (var item in ConstructionManager.GetConstructionDatas()) {
					CreateButton(item);
				}
			}

			public void CreateButton(ConstructionData constructionData) {
				GameObject parent = new GameObject();
				parent.gameObject.name = constructionData.name;
				parent.transform.SetParent(transform);
				parent.AddComponent<Button>().onClick.AddListener(() => OnClick(constructionData.name));
				parent.AddComponent<Image>().sprite = constructionData.Sprite;
				GameObject child = new GameObject();
				Text text = child.AddComponent<Text>();
				text.text = constructionData.name;
				text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				text.fontSize = 24;
				text.alignment = TextAnchor.MiddleCenter;
				text.color = Color.black;
				child.transform.SetParent(parent.transform);
			}

			private void OnClick(string name) {
				Foreman.Instance.StartPlacement(ConstructionManager.GetConstructionData(name));
			}
		}
	}
}
