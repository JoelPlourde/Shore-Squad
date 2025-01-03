using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tutorial {
	public class Encyclopedia {

		private Dictionary<AdviceType, Advice> _advices = new Dictionary<AdviceType, Advice>();

		/// <summary>
		/// Load the Advices from the save file.
		/// </summary>
		/// <param name="adviceDtos"></param>
		public Encyclopedia(EncyclopediaDto encyclopediaDto) {
			if (encyclopediaDto.AdviceDtos.Count == 0) {
				// Populate the Encyclopedia for the first time.
				foreach (AdviceData adviceData in AdviceManager.Instance.GetAdviceDatas()) {
					_advices.Add(adviceData.AdviceType, new Advice(adviceData));
				}
			} else {
				foreach (AdviceDto adviceDto in encyclopediaDto.AdviceDtos) {
					_advices.Add(adviceDto.AdviceType, new Advice(adviceDto));
				}
			}
		}

		/// <summary>
		/// Mark the advice as read.
		/// </summary>
		/// <param name="adviceType">The type of advice.</param>
		public void MarkAdviceAsRead(AdviceType adviceType) {
			if (_advices.TryGetValue(adviceType, out Advice advice)) {
				advice.MarkAdviceAsRead();
			} else {
				throw new UnityException("Please define this AdviceType: " + adviceType);
			}
		}

		/// <summary>
		/// Get all advices, used by the save method.
		/// </summary>
		/// <returns>All advices as a list.</returns>
		public List<Advice> GetAdvices() {
			return _advices.Values.ToList();
		}
	}
}
