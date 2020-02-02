using System;

namespace TennisMatch
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random random = new Random();

        public bool DoesPlayer1WinGame()
        {
            return random.Next(2) == 0;
        }

        public int WhichScore(int numberOfScoresAvailable)
        {
            return random.Next(numberOfScoresAvailable);
        }
    }
}
