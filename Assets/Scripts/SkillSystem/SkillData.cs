using System;
using UnityEngine;

namespace SkillSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/Skill Data")]
	public class SkillData : ScriptableObject {

		[SerializeField]
		[Tooltip("The type of this skill (this must be unique)")]
		public SkillType SkillType;

		[SerializeField]
		[Tooltip("Title of the Skill")]
		public string Title;

		[SerializeField]
		[Tooltip("The description of this skill")]
		public string Description;

		[SerializeField]
		[Tooltip("The icon that represents this skill.")]
		public Sprite Icon;

		[SerializeField]
		[Tooltip("Competencies that the character unlocks by leveling up this skill.")]
		public Competency[] Competencies;
	}
}
