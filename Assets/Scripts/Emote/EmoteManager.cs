using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EmoteSystem {
	public class EmoteManager : MonoBehaviour {

		public static EmoteManager Instance;

		private Dictionary<string, EmoteData> _emoteDatas;

		private void Awake() {
			Instance = this;
			EmoteData[] emoteDatas = Resources.LoadAll<EmoteData>("Scriptable Objects/Emotes");
			_emoteDatas = emoteDatas.ToDictionary(x => GetId(x.emoteType));
		}

		public EmoteData GetEmoteData(EmoteType emoteType) {
			if (_emoteDatas.TryGetValue(GetId(emoteType), out EmoteData emoteData)) {
				return emoteData;
			} else {
				throw new UnityException("The Emote couldn't be found by its id. Please define this Emote Type: " + emoteType);
			}
		}

		private string GetId(EmoteType emoteType) {
			return emoteType.ToString();
		}
	}
}
