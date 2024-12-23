using System;

namespace SkillSystem {
    [Serializable]
    public class ExperienceGain {

        public SkillType SkillType;
        public float Experience;

        public ExperienceGain(SkillType skillType, float experience) {
            SkillType = skillType;
            Experience = experience;
        }
    }
}