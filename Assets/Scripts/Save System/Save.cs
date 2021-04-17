using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class Save {

		[SerializeField]
		public List<QuestDto> QuestDtos = new List<QuestDto>();

		[SerializeField]
		public List<ActorDto> ActorDtos = new List<ActorDto>();
	}
}
