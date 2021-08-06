using System;
using UnityEngine;

namespace SkillSystem {
	[Serializable]
	public class Competency {

		[SerializeField]
		[Tooltip("A short description to quickly describe what this competency is.")]
		public string Descriptive;

		[SerializeField]
		[Tooltip("The icon that represents this competency.")]
		public Sprite Icon;

		[SerializeField]
		[Range(1, 100)]
		[Tooltip("What is the required level to unlock this competency")]
		public int Requirement = 1;

		[SerializeField]
		[TextArea(1, 5)]
		[Tooltip("A detailed description of what this competency is and if there is any further information related to it.")]
		public string Description;
	}
}
