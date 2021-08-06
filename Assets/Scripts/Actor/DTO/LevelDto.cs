using SkillSystem;
using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class LevelDto {

		[SerializeField]
		public int Value;

		[SerializeField]
		public float Experience;

		public LevelDto() {}

		public LevelDto(Level level) {
			Value = level.Value;
			Experience = level.Experience;
		}
	}
}
