using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tutorial {
	public class AdviceManager : MonoBehaviour {

		public static AdviceManager Instance;

		private Dictionary<AdviceType, AdviceData> _adviceDatas;

		private void Awake() {
			Instance = this;

			AdviceData[] adviceDatas = Resources.LoadAll<AdviceData>("Scriptable Objects/Tutorial");
			Array.ForEach(adviceDatas, adviceData => {
				adviceData.AdviceType = FromAdviceName(adviceData.name.ToUpper().Replace(" ", "_"));
			});
			_adviceDatas = adviceDatas.ToDictionary(x => x.AdviceType);
		}

		/// <summary>
		/// Get the Advice Data from a AdviceType
		/// </summary>
		/// <param name="adviceType">The identifier AdviceType</param>
		/// <returns>The AdviceData if found, else throws an exception.</returns>
		public AdviceData GetAdviceData(AdviceType adviceType) {
			if (_adviceDatas.TryGetValue(adviceType, out AdviceData adviceData)) {
				return adviceData;
			} else {
				throw new UnityException("The Advice couldn't be found by its AdviceType. Please define this Advice Data: " + adviceType);
			}
		}

		/// <summary>
		/// Get all Advice Datas, used at the initilization.
		/// </summary>
		/// <returns>All advice datas.</returns>
		public List<AdviceData> GetAdviceDatas() {
			return _adviceDatas.Values.ToList();
		}

		private static AdviceType FromAdviceName(string adviceName) {
			if (Enum.TryParse(adviceName, out AdviceType adviceType)) {
				return adviceType;
			} else {
				throw new UnityException("Please verify the following advice: " + adviceName);
			}
		}
	}
}
