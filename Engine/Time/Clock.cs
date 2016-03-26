using SurvivalGame.Content.Characters;
using System.Collections.Generic;

namespace SurvivalGame.Engine
{
    // TODO - Will eventually need to implement the Event class, which should contain definitions for all events.
    public class Clock
    {
        // TODO - We need to play around with this value to get the right 'day length'. Not urgent now.
        int TIME_IN_DAY = 1000;
        // Contains an event name, and an integer representing how often it should occur
        Dictionary<string, int> TIMED_EVENTS = new Dictionary<string, int>()
        {
            {"incrementHunger", 100},
            {"incrementThirst", 50},
            {"incrementTiredness", 150}
        };

        private int time;
        public Player player;

        public Clock(Player player)
        {
            time = 0;
            this.player = player;
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
            }

            if(time > TIME_IN_DAY)
            {
                // Gets modulo of time if it comes out greater than TIME_IN_DAY
                time = time % TIME_IN_DAY;
            }
        }

        public void CheckForTimedEvents()
        {
            // Check for all timed events
            foreach(string eventName in TIMED_EVENTS.Keys)
            {
                if(time % TIMED_EVENTS[eventName] == 0)
                {
                    switch (eventName)
                    {
                        case ("incrementHunger"):
                            player.needs.UpdateNeeds(-1, "hunger");
                            break;
                        case ("incrementThirst"):
                            player.needs.UpdateNeeds(-1, "thirst");
                            break;
                        case ("incrementTiredness"):
                            player.needs.UpdateNeeds(-1, "tiredness");
                            break;
                    }
                }
            }
        }
    }
}
