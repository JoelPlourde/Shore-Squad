using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TroopSystem;
using FactionSystem;

public static class TroopManager
{
	private static Dictionary<string, GameObject> _troops;

	static TroopManager() {
		_troops = Resources.LoadAll<GameObject>("Prefabs/Troops").ToDictionary(x => x.name.ToLower());
	}

	public static TroopBehaviour InstantiateTroop(string name, Vector3 position, FactionType factionType) {
		if (_troops.TryGetValue(name.ToLower(), out GameObject troopObject)) {
			TroopBehaviour troopBehaviour = Object.Instantiate(troopObject).GetComponent<TroopBehaviour>();
			troopBehaviour.transform.position = position;
			troopBehaviour.FactionType = factionType;
			return troopBehaviour;
		} else {
			throw new UnityException("The Construction couldn't be found by its name. Please define this Construction Data: " + name);
		}
	}
}
