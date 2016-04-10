namespace SurvivalGame.Engine
{
    public class Clock
    {
        // TODO - We need to play around with this value to get the right 'day length'. Not urgent now.
        int TIME_IN_DAY = 1000;

        private int time;

        public Clock()
        {
            time = 0;
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
            this.time += timeIncrement;

            if(time > TIME_IN_DAY)
            {
                // Gets modulo of time if it comes out greater than TIME_IN_DAY
                time = time % TIME_IN_DAY;
            }
        }
    }
}
