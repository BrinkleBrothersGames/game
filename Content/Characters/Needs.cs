using Game.Content.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class Needs
    {
        // TODO - These should be settable. Constructor method should offer alternatives to base ones
        public int MAX_HUNGER = 10;
        public int MAX_THIRST = 10;
        public int MAX_TIREDNESS = 10;
        public int MAX_HEALTH = 10;

        // TODO - investigate how to create these get/set methods properly. Otherwise, do by hand
        public int hungerLevel;
        public int thirstLevel;
        public int tirednessLevel;
        public int health;
        public bool isAlive = true;

        /// <summary>
        /// Creates needs at full defaul level
        /// </summary>
        public Needs()
        {
            this.hungerLevel = 10;
            this.thirstLevel = 10;
            this.tirednessLevel = 10;
            this.health = 10;
        }

        /// <summary>
        /// Sets needs from input values.
        /// </summary>
        /// <param name="hungerLevel"></param>
        /// <param name="thirstLevel"></param>
        /// <param name="tirednessLevel"></param>
        /// <param name="health"></param>
        public Needs(int hungerLevel, int thirstLevel, int tirednessLevel, int health)
        {
            this.hungerLevel = hungerLevel;
            this.thirstLevel = thirstLevel;
            this.tirednessLevel = tirednessLevel;
            this.health = health;
        }

        /// <summary>
        /// Checks whether the hunger need is a valid value.
        /// </summary>
        public void CheckHungerLimit()
        {
            if (this.hungerLevel <= 0)
            {
                this.hungerLevel = 0;
            }
            else if (this.hungerLevel > MAX_HUNGER)
            {
                this.hungerLevel = MAX_HUNGER;
            }
        }

        /// <summary>
        /// Checks whether the thirst need is a valid value.
        /// </summary>
        public void CheckThirstLimit()
        {
            if (this.thirstLevel <= 0)
            {
                this.thirstLevel = 0;
            }
            else if (this.thirstLevel > MAX_HUNGER)
            {
                this.thirstLevel = MAX_HUNGER;
            }
        }

        // TODO - this should damage player stats if low enough.
        /// <summary>
        /// Checks whether the tiredness need is a valid value.
        /// </summary>
        /// <returns></returns>
        public void CheckTirednessLimit()
        {
            if (this.tirednessLevel <= 0)
            {
                this.tirednessLevel = 0;
            }
            else if (this.tirednessLevel > MAX_HUNGER)
            {
                this.tirednessLevel = MAX_HUNGER;
            }
        }

        /// <summary>
        /// Ensures all needs are within pre-defined limits.
        /// </summary>
        public void CheckNeedLimits()
        {
            CheckHungerLimit();
            CheckThirstLimit();
            CheckTirednessLimit();
            CheckHealthLimit();
        }
        
        /// <summary>
        /// Checks if hunger and thrist are low enough to cause damage.
        /// </summary>
        /// <returns>True if needs low enough to cause damage. False otherwise.</returns>
        public bool NeedsTooLow()
        {
            if(hungerLevel == 0 || thirstLevel == 0)
            {
                return true;
            }

            return false;
        }

        // TODO - need to make design decision with Alex - should full hunger be bad, or good?
        /// <summary>
        /// Updates a single need for character.
        /// </summary
        /// <param name="amount">Value need is to be changed by. Positive or negative.</param>
        /// <param name="need">Name of the need to be changed</param>
        public void UpdateNeeds(int amount, string need)
        {
            // Add amount to relevant need
            switch (need)
            {
                case ("hunger"):
                    this.hungerLevel += amount;
                    break;
                case ("thirst"):
                    this.thirstLevel += amount;
                    break;
                case ("tiredness"):
                    this.tirednessLevel += amount;
                    break;
                case ("health"):
                    this.health += amount;
                    break;
            }
        }

        /// <summary>
        /// Updates needs from a dictionary object. Confirms needs are in correct range, and inflicts damage if they are not.
        /// </summary>
        /// <param name="needsDictionary">Dictionary<string, int> of the need and the amount to increase by</string></param>
        public void UpdateNeeds(Dictionary<string, int> needsDictionary)
        {
            foreach (string need in needsDictionary.Keys)
            {
                UpdateNeeds(needsDictionary[need], need);
            }
            
            CheckNeedLimits();

            if (NeedsTooLow())
            {
                UpdateHealth(-1);
            }
        }

        /// <summary>
        /// Adds a positive or negative amount of health to the health need
        /// </summary>
        /// <param name="amount">The amount of health to be added. Can be negative.</param>
        public void UpdateHealth(int amount)
        {
            this.health += amount;

            CheckHealthLimit();
        }

        /// <summary>
        /// Updates the creature's health, then removes the creature if it dies.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="map"></param>
        /// <param name="creature"></param>
        public void UpdateCreatureHealth(int amount, Map map, Creature creature)
        {
            this.health += amount;

            CheckHealthLimit();

            Map.RemoveDeadCreature(map, creature);
        }

        /// <summary>
        /// Checks the health. If too high, lowers it. If zero or less, changes isAlive to false.
        /// </summary>
        public void CheckHealthLimit()
        {
            if (this.health > MAX_HEALTH)
            {
                this.health = MAX_HEALTH;
            }
            else if (this.health <= 0)
            {
                this.isAlive = false;

            }
        }
    }
}
