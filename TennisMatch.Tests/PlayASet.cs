using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TennisMatch.Models;

namespace TennisMatch.Tests
{
    [TestFixture]
    public class PlayASet
    {
        [Test]
        public void WhenASetIsCreated_ThenBothPlayersAreAssigned()
        {
            var player1 = new Player("Fred Perry");
            var player2 = new Player("Andy Murray");

            var gameResultGenerator = new GameResultGenerator(new RandomGenerator());

            var set = new Set(player1, player2, gameResultGenerator);

            set.Player1.Should().BeEquivalentTo(player1);
            set.Player2.Should().BeEquivalentTo(player2);

            set.Games.Should().BeEmpty();
            set.Player1Games.Should().Be(0);
            set.Player2Games.Should().Be(0);
        }

        [Test]
        public void WhenPlayer1WinsTheFirstSixGamesInARow_ThenPlayer1WinsTheSet()
        {
            var player1 = new Player("Serena Williams");
            var player2 = new Player("Naomi Osaka");

            var gameWherePlayer1Wins = new Game(ScoreValues.GameWon, ScoreValues.Love);

            var gameResultGenerator = new Mock<IGameResultGenerator>(MockBehavior.Strict);
            gameResultGenerator.SetupSequence(call => call.GetResult())
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer1Wins);

            var set = new Set(player1, player2, gameResultGenerator.Object);
            set.Play();

            set.Winner.Should().Be(player1);

            set.Player1Games.Should().Be(6);
            set.Player2Games.Should().Be(0);
        }

        [Test]
        public void WhenPlayer2WinsTheFirstSixGamesInARow_ThenPlayer2WinsTheSet()
        {
            var player1 = new Player("Serena Williams");
            var player2 = new Player("Naomi Osaka");

            var gameWherePlayer2Wins = new Game(ScoreValues.Love, ScoreValues.GameWon);

            var gameResultGenerator = new Mock<IGameResultGenerator>(MockBehavior.Strict);
            gameResultGenerator.SetupSequence(call => call.GetResult())
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer2Wins);

            var set = new Set(player1, player2, gameResultGenerator.Object);
            set.Play();

            set.Winner.Should().Be(player2);

            set.Player1Games.Should().Be(0);
            set.Player2Games.Should().Be(6);
        }

        [Test]
        public void WhenThePlayersAlternateGameWinsUntilTheEndOfTheSet_ThenThePlayerWithATwoGameMarginWins()
        {
            var player1 = new Player("Serena Williams");
            var player2 = new Player("Naomi Osaka");

            var gameWherePlayer1Wins = new Game(ScoreValues.GameWon, ScoreValues.Love);
            var gameWherePlayer2Wins = new Game(ScoreValues.Love, ScoreValues.GameWon);

            var gameResultGenerator = new Mock<IGameResultGenerator>(MockBehavior.Strict);
            gameResultGenerator.SetupSequence(call => call.GetResult())
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer2Wins)
                .Returns(gameWherePlayer1Wins)
                .Returns(gameWherePlayer1Wins);

            var set = new Set(player1, player2, gameResultGenerator.Object);
            set.Play();

            set.Winner.Should().Be(player1);

            set.Player1Games.Should().Be(8);
            set.Player2Games.Should().Be(6);
        }
            

        [Test]
        public void CheckGameResults()
        {
            var testScenarios = GetScenarios();

            foreach(var testScenario in testScenarios)
            {
                var player1 = new Player("Player 1");
                var player2 = new Player("Player 2");

                var gameResultGenerator = new Mock<IGameResultGenerator>(MockBehavior.Strict);
                var gameQueue = new Queue<Game>(testScenario.Games);
                gameResultGenerator.Setup(call => call.GetResult())
                    .Returns(() => gameQueue.Dequeue());

                var set = new Set(player1, player2, gameResultGenerator.Object);
                set.Play();

                set.Player1Games.Should().Be(testScenario.ExpectedPlayer1Games);
                set.Player2Games.Should().Be(testScenario.ExpectedPlayer2Games);

                if (testScenario.ExpectedPlayer1Games > testScenario.ExpectedPlayer2Games)
                    set.Winner.Should().Be(player1);
                else
                    set.Winner.Should().Be(player2);

                gameQueue.Should().BeEmpty($"On test: {testScenario.ExpectedPlayer1Games} {testScenario.ExpectedPlayer2Games}");
            }
        }

        private IList<SetResultTest> GetScenarios()
        {
            var player1EasyWin = new Game(ScoreValues.GameWon, ScoreValues.Love);
            var player1HardWin = new Game(ScoreValues.GameWon, ScoreValues.Forty);
            var player2EasyWin = new Game(ScoreValues.Love, ScoreValues.GameWon);
            var player2HardWin = new Game(ScoreValues.Forty, ScoreValues.GameWon);

            return new List<SetResultTest>
            {
                new SetResultTestBuilder(6, 1)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .Build(),
                new SetResultTestBuilder(6, 2)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .Build(),
                new SetResultTestBuilder(6, 3)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .Build(),
                new SetResultTestBuilder(6, 4)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .Build(),
                new SetResultTestBuilder(7, 5)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1EasyWin)
                    .AddGame(player1HardWin)
                    .Build(),
                new SetResultTestBuilder(8, 6)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1EasyWin)
                    .AddGame(player1HardWin)
                    .Build(),
                new SetResultTestBuilder(1, 6)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .Build(),
                new SetResultTestBuilder(2, 6)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .Build(),
                new SetResultTestBuilder(3, 6)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .Build(),
                new SetResultTestBuilder(4, 6)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .Build(),
                new SetResultTestBuilder(5, 7)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .Build(),
                new SetResultTestBuilder(6, 8)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player1HardWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .AddGame(player2EasyWin)
                    .Build(),
            };
        }
    }
}
