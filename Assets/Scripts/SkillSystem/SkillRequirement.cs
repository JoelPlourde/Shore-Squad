using System;

namespace SkillSystem {
    [Serializable]
    public class SkillRequirement {
        public SkillType SkillType;
        public int Level;

        public SkillRequirement(SkillType skillType, int level) {
            SkillType = skillType;
            Level = level;
        }
    }
}