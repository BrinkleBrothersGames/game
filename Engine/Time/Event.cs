using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Engine.Time
{
    public class Event
    {
        // TODO - Actually build this. Each event should have a name and an id
        // TODO - Need a class called "AllEvents" or somesuch, which should initialize all events. AllEvents should contain logic for every event.

        int INCREASE_HUNGER_EVENT = 1;
        int INCREASE_THIRST_EVENT = 2;
        int INCREASE_TIREDNESS_EVENT = 3;

        string eventName;
        int eventId;
    }
}
