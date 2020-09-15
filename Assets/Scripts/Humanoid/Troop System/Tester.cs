using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TroopSystem;
using EncampmentSystem;

public class Tester : MonoBehaviour
{
	public List<Encampment> Encampments1;
	public List<Encampment> Encampments2;

	void Start()
    {
		TaxCollector inquisitor = TroopManager.InstantiateTroop("tax_collector", new Vector3(-20, 0, 0), FactionSystem.FactionType.INQUISITOR) as TaxCollector;
		inquisitor.StartCollection(100, Encampments1);

		TaxCollector sentinels = TroopManager.InstantiateTroop("tax_collector", new Vector3(20, 0, 0), FactionSystem.FactionType.SENTINELS) as TaxCollector;
		sentinels.StartCollection(100, Encampments2);
	}
}
