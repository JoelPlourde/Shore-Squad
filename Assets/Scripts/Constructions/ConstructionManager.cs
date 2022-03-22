using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructionSystem;

public static class ConstructionManager {

	private static Dictionary<string, ConstructionData> _constructions;

	static ConstructionManager() {
		Architect = Resources.Load<ArchitectData>("Scriptable Objects/Constructions/Architect");

		_constructions = new Dictionary<string, ConstructionData>();

		foreach (Category category in Architect.Categories) {
			foreach (ConstructionData constructionData in category.ConstructionDatas) {
				_constructions.Add(constructionData.name.ToLower(), constructionData);
			}
		}
	}

	public static ConstructionData GetConstructionData(string name) {
		if (_constructions.TryGetValue(name.ToLower(), out ConstructionData constructionData)) {
			return constructionData;
		} else {
			throw new UnityException("The Construction couldn't be found by its name. Please define this Construction Data: " + name);
		}
	}

	public static ArchitectData Architect { get; private set; }
}
