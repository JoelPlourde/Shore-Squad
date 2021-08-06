using System.Collections;
using UnityEngine;

namespace SkillSystem {
	public static class ExperienceTable {

		private static readonly float[] _experienceTable;

		static ExperienceTable() {
			_experienceTable = new float[100];

			for (int i = 0; i < 100; i++) {
				// TODO Tweak this experience formula.
				_experienceTable[i] = i * 100f;
			}
		}

		/// <summary>
		///  Get the minimum amount of experience required to reach the provided level.
		///  
		/// Ex: 
		/// Q. What is the minimum amount of experience required to reach lv. 5 ?
		/// A. Xf
		/// </summary>
		/// <param name="level">The level to check the required experience</param>
		/// <returns>The minimum amount of experience to reach that level.</returns>
		public static float GetExperienceRequiredAt(int level) {
			if (level > 100 || level <= 0) {
				throw new UnityException("The level must be between 1 and 100 inclusive.");
			}
			return _experienceTable[level - 1];
		}
	}
}
