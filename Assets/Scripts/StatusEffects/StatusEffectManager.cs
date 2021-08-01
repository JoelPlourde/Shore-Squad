using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem {
	public static class StatusEffectManager {
		private static readonly Dictionary<string, StatusEffectData> _statusEffectDatas = new Dictionary<string, StatusEffectData>();

		public static StatusEffectData GetStatusEffectData(string name) {
			if (_statusEffectDatas.TryGetValue(name, out StatusEffectData value)) {
				return value;
			} else {
				StatusEffectData statusEffectData = Resources.Load<StatusEffectData>("Scriptable Objects/Status Effects/" + name);
				if (statusEffectData == null) {
					throw new UnityException("The StatusEffectData cannot be found at: Assets/Resources/Scriptable Objects/Status Effects/" + name);
				}
				_statusEffectDatas.Add(name, statusEffectData);
				return _statusEffectDatas[name];
			}
		}
	}
}
