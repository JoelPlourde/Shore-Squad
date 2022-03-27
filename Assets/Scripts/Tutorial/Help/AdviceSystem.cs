using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial {
	public class AdviceSystem : MonoBehaviour, ISaveable {

		public static AdviceSystem Instance;

		private Encyclopedia _encyclopedia;

		private void Awake() {
			Instance = this;
		}

		/// <summary>
		/// Show the Advice
		/// </summary>
		public void ShowAdvice(AdviceType adviceType) {

			// TODO Display the Advice Handler
		}

		/// <summary>
		/// Method to mark advice as read.
		/// </summary>
		/// <param name="adviceType">The type of advice.</param>
		public void MarkAdviceAsRead(AdviceType adviceType) {
			_encyclopedia.MarkAdviceAsRead(adviceType);
		}

		/// <summary>
		/// Load the Encyclopedia from the save file.
		/// </summary>
		/// <param name="save">The save file to load from.</param>
		public void Load(Save save) {
			_encyclopedia = new Encyclopedia(save.EncyclopediaDto);
		}

		/// <summary>
		/// Save the Encryclopedia in the save file.
		/// </summary>
		/// <param name="save">The save file to save in.</param>
		public void Save(Save save) {
			save.EncyclopediaDto = EncyclopediaDto.FromEncyclopedia(_encyclopedia);
		}
	}
}
