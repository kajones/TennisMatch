using FluentAssertions;
using NUnit.Framework;
using TennisMatch.Models;

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

            var match = new Match(player1, player2);

            match.Player1.Should().BeEquivalentTo(player1);
            match.Player2.Should().BeEquivalentTo(player2);
            match.Sets.Should().BeEmpty();
        }
    }
}
