using System.Collections.Generic;
using TennisMatch.Models;

namespace TennisMatch
{
    public class GameResultGenerator : IGameResultGenerator
    {
        private IRandomGenerator randomGenerator;

        private IList<Game> Player1Wins => new List<Game>
            {
                new Game(ScoreValues.GameWon, ScoreValues.Love),
                new Game(ScoreValues.GameWon, ScoreValues.Fifteen),
                new Game(ScoreValues.GameWon, ScoreValues.Thirty),
                new Game(ScoreValues.GameWon, ScoreValues.Forty) // This game went to deuce but then player 1 won two points
            };

        private IList<Game> Player2Wins => new List<Game>
            {
                new Game(ScoreValues.Love, ScoreValues.GameWon),
                new Game(ScoreValues.Fifteen, ScoreValues.GameWon),
                new Game(ScoreValues.Thirty, ScoreValues.GameWon),
                new Game(ScoreValues.Forty, ScoreValues.GameWon) // Deuce game then player 2 won two points
            };

        public GameResultGenerator(IRandomGenerator randomGenerator)
        {
            this.randomGenerator = randomGenerator;
        }

        public Game GetResult()
        {
            var player1WinsGame = randomGenerator.DoesPlayer1WinGame();

            if (player1WinsGame)
                return GetRandomResultForPlayerWinning(Player1Wins);

            return GetRandomResultForPlayerWinning(Player2Wins);
        }

        private Game GetRandomResultForPlayerWinning(IList<Game> gameResults)
        {
            var scoreToUse = randomGenerator.WhichScore(gameResults.Count);
            return gameResults[scoreToUse];
        }
    }
}
