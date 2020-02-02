using FluentAssertions;
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

            var set = new Set(player1, player2);

            set.Player1.Should().BeEquivalentTo(player1);
            set.Player2.Should().BeEquivalentTo(player2);
        }
    }
}
