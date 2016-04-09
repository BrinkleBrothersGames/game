using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class Skill
    {
        // TODO - Assumes levels are linear. May want to add logic for exponential increase.
        int EXPERIENCE_PER_LEVEL = 100;

        string name;
        double modifier;
        int experience;
        int level;

        public Skill(string name, int level)
        {
            this.name = name;
            this.level = level;
            this.experience = (level - 1) * EXPERIENCE_PER_LEVEL; // Should have 0 experience at level 1, then EXPERIENCE_PER_LEVEL for each subsequent level.
            this.modifier = GetSkillModifier();
        }

        /// <summary>
        /// Adds an integer amount of experience to the player's skill.
        /// </summary>
        /// <param name="experienceAdded"></param>
        public void AddExperience(int experienceAdded)
        {
            this.experience += experienceAdded;
        }

        /// <summary>
        /// Updates the player's skill modifier. Divide their experience by the experience per level and times by a tenth. Gives +0.1 per level.
        /// </summary>
        public void UpdateSkillModifier()
        {
            this.modifier = 0.9 + (experience * (1 / EXPERIENCE_PER_LEVEL) * (1/10));
        }

        /// <summary>
        /// This returns a float which will modify a player's action based on their skill level
        /// </summary>
        public double GetSkillModifier()
        {
            return this.modifier;
        }
    }
}
