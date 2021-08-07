using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillSystem {
	public class SkillManager : MonoBehaviour {

		public static SkillManager Instance;

		private Dictionary<SkillType, SkillData> _skillDatas;

		private void Awake() {
			Instance = this;
			_skillDatas = Resources.LoadAll<SkillData>("Scriptable Objects/Skills").ToDictionary(x => x.SkillType);
		}

		public SkillData GetSkillData(SkillType skillType) {
			if (_skillDatas.TryGetValue(skillType, out SkillData skillData)) {
				return skillData;
			} else {
				throw new UnityException("The Skill couldn't be found by its Skill Type.");
			}
		}
	}
}
