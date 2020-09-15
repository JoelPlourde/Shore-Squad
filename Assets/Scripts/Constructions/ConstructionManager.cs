using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructionSystem;
using System.Linq;

public static class ConstructionManager {

	private static Dictionary<string, ConstructionData> _constructions;

	static ConstructionManager() {
		_constructions = Resources.LoadAll<ConstructionData>("Scriptable Objects/Constructions").ToDictionary(x => x.name.ToLower());
	}

	public static List<ConstructionData> GetConstructionDatas() {
		return _constructions.Values.ToList();
	}

	public static ConstructionData GetConstructionData(string name) {
		if (_constructions.TryGetValue(name.ToLower(), out ConstructionData constructionData)) {
			return constructionData;
		} else {
			throw new UnityException("The Construction couldn't be found by its name. Please define this Construction Data: " + name);
		}
	}
}
