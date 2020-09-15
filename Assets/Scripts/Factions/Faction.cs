using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EncampmentSystem;

namespace FactionSystem {
	public class Faction {

		public FactionType FactionType { get; private set; }

		public List<Encampment> Encampments { get; private set; }

		public Faction(FactionType factionType) {
			FactionType = factionType;
			Encampments = new List<Encampment>();
		}
	}
}
