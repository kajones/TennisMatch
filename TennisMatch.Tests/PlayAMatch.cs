using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TennisMatch.Models;
using Match = TennisMatch.Models.Match;

namespace TennisMatch.Tests
{
    [TestFixture]
    public class PlayAMatch
    {
        [Test]
        public void WhenAMatchIsCreated_ThenBothPlayersAreAssigned()
        {
            var player1 = new Player("Fred Perry");
            var player2 = new Player("Andy Murray");

            var gameResultGenerator = new GameResultGenerator(new RandomGenerator());

            var match = new Match(player1, player2, gameResultGenerator);

            match.Player1.Should().BeEquivalentTo(player1);
            match.Player2.Should().BeEquivalentTo(player2);
            match.Sets.Should().BeEmpty();
            match.Winner.Should().BeNull();
        }

        [Test]
        public void WhenPlayer1WinsTheFirstTwoSets_ThenTheyWinTheMatch()
        {
            var player1 = new Player("Roger Federer");
            var player2 = new Player("Andy Murray");

            var gameResultGenerator = GetGeneratorForSetsWon(new[] { true, true });

            var match = new Match(player1, player2, gameResultGenerator);

            match.Play();

            match.Winner.Should().Be(player1);

            match.GetResult().Should().Be("6-0 6-0");
        }

        [Test]
        public void WhenTheFirstTwoSetsAreSharedAndPlayer1WinsTheThird_ThenPlayer1WinsTheMatch()
        {
            var player1 = new Player("Roger Federer");
            var player2 = new Player("Andy Murray");

            var gameResultGenerator = GetGeneratorForSetsWon(new[] { true, false, true });

            var match = new Match(player1, player2, gameResultGenerator);

            match.Play();

            match.Winner.Should().Be(player1);

            match.GetResult().Should().Be("6-0 0-6 6-0");
        }

        [Test]
        public void WhenTheFirstTwoSetsAreSharedAndPlayer2WinsTheThird_ThenPlayer2WinsTheMatch()
        {
            var player1 = new Player("Roger Federer");
            var player2 = new Player("Andy Murray");

            var gameResultGenerator = GetGeneratorForSetsWon(new[] { true, false, false });

            var match = new Match(player1, player2, gameResultGenerator);

            match.Play();

            match.Winner.Should().Be(player2);

            match.GetResult().Should().Be("0-6 6-0 6-0");
        }

        [Test]
        public void WhenPlayer2WinsTheFirstTwoSets_ThenTheyWinTheMatch()
        {
            var player1 = new Player("Roger Federer");
            var player2 = new Player("Andy Murray");

            var gameResultGenerator = GetGeneratorForSetsWon(new[] { false, false });

            var match = new Match(player1, player2, gameResultGenerator);

            match.Play();

            match.Winner.Should().Be(player2);

            match.GetResult().Should().Be("6-0 6-0");
        }

        private IGameResultGenerator GetGeneratorForSetsWon(bool[] setWonByPlayer1)
        {
            var gameWherePlayer1Won = new Game(ScoreValues.GameWon, ScoreValues.Fifteen);
            var gameWherePlayer2Won = new Game(ScoreValues.Thirty, ScoreValues.GameWon);

            var gameResultGenerator = new Mock<IGameResultGenerator>(MockBehavior.Strict);

            var games = new List<Game>();
            foreach(var setWin in setWonByPlayer1)
            {
                if (setWin)
                {
                    games.AddRange(Enumerable.Repeat(gameWherePlayer1Won, 6));
                }
                else
                {
                    games.AddRange(Enumerable.Repeat(gameWherePlayer2Won, 6));
                }
            }

            var queue = new Queue<Game>(games);

            gameResultGenerator.Setup(call => call.GetResult()).Returns(() => queue.Dequeue());

            return gameResultGenerator.Object;
        }
    }
}
