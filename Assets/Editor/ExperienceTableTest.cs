using NUnit.Framework;
using SkillSystem;
using UnityEngine;

public class ExperienceTableTest
{
	[Test]
	public void GetExperienceRequiredAt_Minimum_test() {
		Assert.That(ExperienceTable.GetExperienceRequiredAt(Constant.MINIMUM_LEVEL), Is.EqualTo(0));
	}

	[Test]
	public void GetExperienceRequiredAt_Mininum_plus_one_test() {
		Assert.That(ExperienceTable.GetExperienceRequiredAt(Constant.MINIMUM_LEVEL + 1), Is.EqualTo(100));
	}

	[Test]
	public void GetExperienceRequiredAt_below_minimum_test() {
		Assert.Throws<UnityException>(() => ExperienceTable.GetExperienceRequiredAt(Constant.MINIMUM_LEVEL - 1));
	}

	[Test]
	public void GetExperienceRequiredAt_above_maximum_test() {
		Assert.Throws<UnityException>(() => ExperienceTable.GetExperienceRequiredAt(Constant.MAXIMUM_LEVEL + 1));
	}
}
