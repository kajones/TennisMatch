using FluentAssertions;
using Moq;
using NUnit.Framework;
using TennisMatch.Models;

namespace TennisMatch.Tests
{
    [TestFixture]
    public class GenerateAGameResult
    {
        [Test]
        [TestCase(0, ScoreValues.GameWon, ScoreValues.Love)]
        [TestCase(1, ScoreValues.GameWon, ScoreValues.Fifteen)]
        [TestCase(2, ScoreValues.GameWon, ScoreValues.Thirty)]
        [TestCase(3, ScoreValues.GameWon, ScoreValues.Forty)]
        public void WhenPlayer1WinsTheGame_ThenPlayer2DoesNotExceedForty(int player2Score, ScoreValues expectedPlayer1Score, ScoreValues expectedPlayer2Score)
        {
            var randomGenerator = new Mock<IRandomGenerator>(MockBehavior.Strict);
            randomGenerator.Setup(call => call.DoesPlayer1WinGame()).Returns(true);
            randomGenerator.Setup(call => call.WhichScore(4)).Returns(player2Score);

            var gameResults = new GameResultGenerator(randomGenerator.Object);

            var game = gameResults.GetResult();

            game.Player1Score.Should().Be(expectedPlayer1Score);
            game.Player2Score.Should().Be(expectedPlayer2Score);
        }

        [Test]
        [TestCase(0, ScoreValues.Love, ScoreValues.GameWon)]
        [TestCase(1, ScoreValues.Fifteen, ScoreValues.GameWon)]
        [TestCase(2, ScoreValues.Thirty, ScoreValues.GameWon)]
        [TestCase(3, ScoreValues.Forty, ScoreValues.GameWon)]
        public void WhenPlayer2WinsTheGame_ThenPlayer1DoesNotExceedForty(int player1Score, ScoreValues expectedPlayer1Score, ScoreValues expectedPlayer2Score)
        {
            var randomGenerator = new Mock<IRandomGenerator>(MockBehavior.Strict);
            randomGenerator.Setup(call => call.DoesPlayer1WinGame()).Returns(false);
            randomGenerator.Setup(call => call.WhichScore(4)).Returns(player1Score);

            var gameResults = new GameResultGenerator(randomGenerator.Object);

            var game = gameResults.GetResult();

            game.Player1Score.Should().Be(expectedPlayer1Score);
            game.Player2Score.Should().Be(expectedPlayer2Score);
        }
    }
}
