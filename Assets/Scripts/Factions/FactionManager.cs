using System;
using System.Collections.Generic;
using EncampmentSystem;
using UnityEngine;
using System.Linq;

namespace FactionSystem {
	public static class FactionManager {
		public static Dictionary<FactionType, Faction> Factions;
		public static Dictionary<FactionType, FactionData> FactionDatas;

		static FactionManager() {
			Factions = Enum.GetValues(typeof(FactionType)).OfType<FactionType>().ToDictionary(x => x, x => new Faction(x));
			FactionDatas = Resources.LoadAll<FactionData>("Scriptable Objects/Factions")
				.ToDictionary(x => (FactionType)Enum.Parse(typeof(FactionType), x.name.ToUpper()));
		}

		public static void RegisterEncampment(FactionType factionType, Encampment encampment) {
			Factions[factionType].Encampments.Add(encampment);
		}

		public static void UnregisterEncampment(FactionType factionType, Encampment encampment) {
			Factions[factionType].Encampments.Remove(encampment);
		}

		public static List<Encampment> GetEncampments(FactionType factionType) {
			return Factions[factionType].Encampments;
		}

		public static FactionData GetFactionData(FactionType factionType) {
			return FactionDatas[factionType];
		}
	}
}
