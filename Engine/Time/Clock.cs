using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Engine.Time;
using System.Collections.Generic;

namespace SurvivalGame.Engine
{
    // TODO - Will eventually need to implement the Event class, which should contain definitions for all events.
    public class Clock
    {
        // TODO - We need to play around with this value to get the right 'day length'. Not urgent now.
        int TIME_IN_DAY = 1000;
        List<Effect> effectList = new List<Effect>();
        // Contains an event name, and an integer representing how often it should occur
        Dictionary<string, int> TIMED_EVENTS = new Dictionary<string, int>()
        {
            {"incrementHunger", 100},
            {"incrementThirst", 50},
            {"incrementTiredness", 150},
            {"updateCreatureNeeds", 250}
        };

        private int time;
        public Player player;
        public Map map;

        public Clock(Player player, Map map)
        {
            time = 0;
            this.player = player;
            this.map = map;
        }

        public void SetTime(int newTime)
        {
            this.time = newTime;
        }

        public int GetTime()
        {
            return this.time;
        }

        public void AddTime(int timeIncrement)
        {
            // Want to increase time step by step, so we don't miss an event
            while (timeIncrement > 0)
            {
                this.time += 1;
                timeIncrement -= 1;

                CheckForTimedEvents();
                UpdateEffects();
            }

            if(time > TIME_IN_DAY)
            {
                // Gets modulo of time if it comes out greater than TIME_IN_DAY
                time = time % TIME_IN_DAY;
            }
        }

        public void CheckForTimedEvents()
        {
            Dictionary<string, int> needChange = new Dictionary<string, int>();
            bool updateStats = false;

            // Check for all timed events
            foreach(string eventName in TIMED_EVENTS.Keys)
            {
                if(time % TIMED_EVENTS[eventName] == 0)
                {
                    switch (eventName)
                    {
                        case ("incrementHunger"):
                            needChange.Add("hunger", -1);
                            updateStats = true;
                            break;
                        case ("incrementThirst"):
                            needChange.Add("thirst", -1);
                            updateStats = true;
                            break;
                        case ("incrementTiredness"):
                            needChange.Add("tiredness", -1);
                            updateStats = true;
                            break;
                        case ("updateCreatureNeeds"):
                            foreach(Creature creature in map.presentCreatures)
                            {
                                creature.UpdateCreatureNeeds(new Dictionary<string, int>()
                                {
                                    {"hunger", -1},
                                    {"thirst", -1},
                                    {"tiredness", -1}
                                });
                            }
                            break;
                    }
                }
            }

            if (updateStats)
            {
                player.UpdatePlayerNeeds(needChange);
            }
        }

        public void UpdateEffects()
        {
            foreach(Effect effect in effectList.ToArray())
            {
                effect.DecreaseDuration(1);

                if(effect.duration == 0)
                {
                    effect.RemoveEffect(player);

                    effectList.Remove(effect);
                }
            }
        }

        public void AddTimedEffect(Effect effect)
        {
            effect.ApplyEffect();

            effectList.Add(effect);
        }
    }
}
