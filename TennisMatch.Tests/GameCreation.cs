using System;
using FluentAssertions;
using NUnit.Framework;
using TennisMatch.Models;

namespace TennisMatch.Tests
{
    [TestFixture]
    public class GameCreation
    {
        [Test]
        public void WhenNeitherPlayerHasWonTheGame_ThenAnErrorIsThrown()
        {
            // Not trying to handle mid-game scores so expect a game to have been completed
            var ex = Assert.Throws<Exception>(() => new Game(ScoreValues.Forty, ScoreValues.Thirty));
            ex.Message.Should().Be("Incomplete game - expect a player to have won the game");
        }

        [Test]
        public void WhenBothPlayersHaveAWinningScore_ThenAnErrorIsThrown()
        {
            var ex = Assert.Throws<Exception>(() => new Game(ScoreValues.GameWon, ScoreValues.GameWon));
            ex.Message.Should().Be("It is impossible for both players to win the game");
        }

        [Test]
        [TestCase(ScoreValues.Love)]
        [TestCase(ScoreValues.Fifteen)]
        [TestCase(ScoreValues.Thirty)]
        [TestCase(ScoreValues.Forty)]
        public void WhenPlayer1Wins_ThenTheGameIsCreated(ScoreValues player2Score)
        {
            var game = new Game(ScoreValues.GameWon, player2Score);

            game.Player1Score.Should().Be(ScoreValues.GameWon);
            game.Player2Score.Should().Be(player2Score);
        }

        [Test]
        [TestCase(ScoreValues.Love)]
        [TestCase(ScoreValues.Fifteen)]
        [TestCase(ScoreValues.Thirty)]
        [TestCase(ScoreValues.Forty)]
        public void WhenPlayer2Wins_ThenTheGameIsCreated(ScoreValues player1Score)
        {
            var game = new Game(player1Score, ScoreValues.GameWon);

            game.Player1Score.Should().Be(player1Score);
            game.Player2Score.Should().Be(ScoreValues.GameWon);
        }
    }
}
