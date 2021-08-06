using SkillSystem;
using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class SkillsDto {

		[SerializeField]
		public LevelDto[] levelDtos;

		public SkillsDto() {}

		public SkillsDto(Skills skills) {
			levelDtos = new LevelDto[Enum.GetValues(typeof(SkillType)).Length];
			for (int i = 0; i < levelDtos.Length; i++) {
				levelDtos[i] = new LevelDto(skills.GetLevel((SkillType) i));
			}
		}
	}
}