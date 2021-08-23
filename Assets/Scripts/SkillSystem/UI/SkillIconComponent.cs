using System;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkillSystem {
	namespace UI {
		[RequireComponent(typeof(Image))]
		public class SkillIconComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

			private string _tooltip;

			/// <summary>
			/// Initialize the component with the Skill data.
			/// </summary>
			/// <param name="skillData">The skill data</param>
			public void Initialize(SkillData skillData) {
				GetComponent<Image>().sprite = skillData.Icon;
				_tooltip = string.Format("<b><color=#{2}>{0}</color></b>\n{1}", skillData.Title, skillData.Description, ColorUtility.ToHtmlStringRGB(skillData.Color));
			}

			public void OnPointerEnter(PointerEventData eventData) {
				Tooltip.Instance.ShowTooltip(_tooltip, Constant.TOOLTIP_DELAY);
			}

			public void OnPointerExit(PointerEventData eventData) {
				Tooltip.Instance.HideTooltip();
			}
		}
	}
}
