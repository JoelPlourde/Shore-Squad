using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElementalRift {
	public class ElementManager : MonoBehaviour {

		public static ElementManager Instance;

		private Dictionary<string, ElementData> _elementDatas;

		private void Awake() {
			Instance = this;
			ElementData[] elementDatas = Resources.LoadAll<ElementData>("Scriptable Objects/Elements");
			_elementDatas = elementDatas.ToDictionary(x => GetId(x.ElementType));
		}

		public ElementData GetElementData(ElementType elementType) {
			if (_elementDatas.TryGetValue(GetId(elementType), out ElementData elementData)) {
				return elementData;
			} else {
				throw new UnityException("The Element couldn't be found by its id. Please define this Element Type: " + elementType);
			}
		}

		private string GetId(ElementType elementType) {
			return elementType.ToString();
		}
	}
}
