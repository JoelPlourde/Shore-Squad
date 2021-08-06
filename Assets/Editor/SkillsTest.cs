using NUnit.Framework;
using SaveSystem;
using SkillSystem;
using System;

public class SkillsTest
{
	[Test]
	public void Initialize_test() {
		Skills skills = new Skills();
		skills.Initialize(GetSkillsDtoFixture(out int[] values));

		for (int i = 0; i < Enum.GetValues(typeof(SkillType)).Length; i++) {
			Assert.That(skills.GetLevel((SkillType)i).Value, Is.EqualTo(values[i]));
		}
	}

	[Test]
	public void GainExperience_test() {
		Skills skills = new Skills();
		SkillsDto skillsDto = GetSkillsDtoFixture(out int[] values);
		skillsDto.levelDtos[0].Value = 1;
		skillsDto.levelDtos[0].Experience = 0;

		skills.Initialize(skillsDto);
		skills.GainExperience((SkillType) 0, 100);
		Assert.That(skills.GetLevel((SkillType)0).Value, Is.EqualTo(2));

		skills.GainExperience((SkillType)0, 1);
		Assert.That(skills.GetLevel((SkillType)0).Value, Is.EqualTo(2));

		skills.GainExperience((SkillType)0, 10000);
		Assert.That(skills.GetLevel((SkillType)0).Value, Is.GreaterThanOrEqualTo(3));
	}

	private SkillsDto GetSkillsDtoFixture(out int[] values) {
		int length = Enum.GetValues(typeof(SkillType)).Length;

		SkillsDto skillsDto = new SkillsDto {
			levelDtos = new LevelDto[length]
		};

		values = new int[length];
		for (int i = 0; i < length; i++) {
			values[i] = UnityEngine.Random.Range(1, 100);
			skillsDto.levelDtos[i] = new LevelDto {
				Value = values[i]
			};
		}

		return skillsDto;
	}
}
