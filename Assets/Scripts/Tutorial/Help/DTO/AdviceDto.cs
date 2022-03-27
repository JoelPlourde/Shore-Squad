using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial {
	[Serializable]
	public class AdviceDto {

		[SerializeField]
		public AdviceType AdviceType;

		[SerializeField]
		public bool IsRead;

		public static AdviceDto FromAdvice(Advice advice) {
			return new AdviceDto {
				AdviceType = advice.AdviceData.AdviceType,
				IsRead = advice.IsRead
			};
		}
	}
}
