using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial {
	[Serializable]
	public class EncyclopediaDto {

		[SerializeField]
		public List<AdviceDto> AdviceDtos = new List<AdviceDto>();

		public static EncyclopediaDto FromEncyclopedia(Encyclopedia encyclopedia) {
			EncyclopediaDto encyclopediaDto = new EncyclopediaDto {
				AdviceDtos = new List<AdviceDto>()
			};

			if (ReferenceEquals(encyclopedia, null)) {
				return encyclopediaDto;
			}

			foreach (Advice advice in encyclopedia.GetAdvices()) {
				encyclopediaDto.AdviceDtos.Add(AdviceDto.FromAdvice(advice));
			}

			return encyclopediaDto;
		}
	}
}
