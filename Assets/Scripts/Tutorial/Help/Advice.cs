using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial {
	public class Advice {

		/// <summary>
		/// Initialize the Advice for the first time.
		/// </summary>
		/// <param name="adviceData">The AdviceData</param>
		public Advice(AdviceData adviceData) {
			AdviceData = adviceData;
			IsRead = false;
		}

		/// <summary>
		/// Load Advice from save file.
		/// </summary>
		/// <param name="adviceDto">The AdviceDTO to load it from.</param>
		public Advice(AdviceDto adviceDto) {
			AdviceData = AdviceManager.Instance.GetAdviceData(adviceDto.AdviceType);
			IsRead = adviceDto.IsRead;
		}

		/// <summary>
		/// Dismiss the Advice, mark it as read.
		/// </summary>
		public void MarkAdviceAsRead() {
			IsRead = true;
		}

		public AdviceData AdviceData { get; private set; }
		public bool IsRead { get; private set; }
	}
}
