using UnityEngine;
using UnityEngine.EventSystems;

namespace SkillSystem {
	namespace UI {
		public class SkillComponent : MonoBehaviour {

			private bool _initialized = false;

			private void Awake() {
				IconComponent = transform.GetComponentInChildren<SkillIconComponent>();
				if (ReferenceEquals(IconComponent, null)) {
					throw new UnityException("Please verify that this component has an IconComponent as his children.");
				}

				TextComponent = transform.GetComponentInChildren<SkillTextComponent>();
				if (ReferenceEquals(TextComponent, null)) {
					throw new UnityException("Please verify that this component has an TextComponent as his children.");
				}
			}

			public void Initialize(SkillData skillData, Level level) {
				if (!_initialized) {
					IconComponent.Initialize(skillData);
					TextComponent.Initialize(level);
					_initialized = true;
				}
				TextComponent.Refresh(level);
			}

			public SkillIconComponent IconComponent { get; private set; }
			public SkillTextComponent TextComponent { get; private set; }
		}
	}
}
