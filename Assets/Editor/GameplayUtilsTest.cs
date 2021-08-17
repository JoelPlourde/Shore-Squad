using NUnit.Framework;
using UnityEngine;

namespace UnitTest {
	public class GameplayUtilsTest {

		[Test]
		public void CalculateRepeatRateBasedOnSpeed_test() {
			float maxRepeatRate = GameplayUtils.CalculateRepeatRateBasedOnSpeed(0);
			float minRepeatRate = GameplayUtils.CalculateRepeatRateBasedOnSpeed(50);
			Debug.Log(minRepeatRate);
			Debug.Log(maxRepeatRate);
			Assert.That(minRepeatRate > maxRepeatRate);
		}

		[Test]
		public void CalculateIfCriticalStrikeBasedOnLuck_test() {
			Assert.That(!GameplayUtils.CalculateIfCriticalStrikeBasedOnLuck(0));
			Assert.That(GameplayUtils.CalculateIfCriticalStrikeBasedOnLuck(Constant.MAXIMUM_LUCK));
		}

		[Test]
		public void CalculateDamageBasedOnStrength_test() {
			Assert.That(GameplayUtils.CalculateDamageBasedOnStrength(0, 100, 0), Is.EqualTo(100));
			Assert.That(GameplayUtils.CalculateDamageBasedOnStrength(0, 100, 100), Is.EqualTo(150));
			Assert.That(GameplayUtils.CalculateDamageBasedOnStrength(100, 10, 0), Is.EqualTo(0));
			Assert.That(GameplayUtils.CalculateDamageBasedOnStrength(10, 100, 0), Is.EqualTo(90));
		}
	}
}
