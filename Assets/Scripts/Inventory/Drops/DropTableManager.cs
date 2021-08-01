using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DropSystem;
using System;
using System.Linq;

#if UNITY_EDITOR
public class DropTableManager : MonoBehaviour
{
	private static Dictionary<string, DropTable> _dropTables;

	static DropTableManager() {
		DropTable[] dropTables = Resources.LoadAll<DropTable>("Scriptable Objects/Drop Tables");
		Array.ForEach(dropTables, dropTable => {
			dropTable.ID = dropTable.name.ToLower().Replace(" ", "_");
		});
		_dropTables = dropTables.ToDictionary(x => x.ID);
	}

	public static DropTable GetDropTable(string name) {
		if (_dropTables.TryGetValue(name.ToLower().Replace(" ", "_"), out DropTable dropTable)) {
			return dropTable;
		} else {
			throw new UnityException("The Drop Table couldn't be found by its name. Please define this Drop Table: " + name);
		}
	}
}
#endif
