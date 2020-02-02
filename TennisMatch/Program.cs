using System;
using TennisMatch.Models;

namespace TennisMatch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tennis Match simulator");

            var gameResultGenerator = GetGameResultGenerator();

            var player1 = new Player("Andy Murray");
            var player2 = new Player("Roger Federer");

            var match = new Match(player1, player2, gameResultGenerator);
            match.Play();

            Console.WriteLine($"The winner is: {match.Winner.Name} {match.GetResult()}");
            Console.ReadLine();
        }

        private static IGameResultGenerator GetGameResultGenerator()
        {
            var randomGenerator = new RandomGenerator();
            return new GameResultGenerator(randomGenerator);
        }
    }
}
