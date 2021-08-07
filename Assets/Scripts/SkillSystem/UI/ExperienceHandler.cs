using System;
using UI;
using UnityEngine;

namespace SkillSystem {
	namespace UI {
		public class ExperienceHandler : Menu {

			public static ExperienceHandler Instance;

			private SkillComponent[] _skillComponents;

			protected override void Awake() {
				base.Awake();
				Instance = this;

				_skillComponents = GetComponentsInChildren<SkillComponent>();
			}

			public override void Open(Actor actor) {
				foreach (SkillType skillType in Enum.GetValues(typeof(SkillType))) {
					_skillComponents[(int) skillType].Initialize(SkillManager.Instance.GetSkillData(skillType), actor.Skills.GetSkillLevels[skillType]);
				}

				actor.Skills.OnLevelUp += OnLevelUp;
				actor.Skills.OnExperienceGain += OnExperienceGain;

				Canvas.enabled = true;
			}

			public override void Close(Actor actor) {
				Canvas.enabled = false;

				actor.Skills.OnLevelUp -= OnLevelUp;
				actor.Skills.OnExperienceGain -= OnExperienceGain;
			}

			/// <summary>
			/// Callback whenever an Actor levels up in any of its skills.
			/// </summary>
			/// <param name="skillType">The type of the skill that leveled up.</param>
			/// <param name="level">The updated level information.</param>
			private void OnLevelUp(SkillType skillType, Level level) {
				_skillComponents[(int) skillType].TextComponent.Refresh(level);
			}

			/// <summary>
			/// Callback whenever an actor gain experience in any of its skills.
			/// </summary>
			/// <param name="skillType">The type of the skill that gained experience in.</param>
			/// <param name="level">The update level information.</param>
			private void OnExperienceGain(SkillType skillType, Level level) {
				_skillComponents[(int)skillType].TextComponent.OnGainExperience(level);
			}
		}
	}
}
