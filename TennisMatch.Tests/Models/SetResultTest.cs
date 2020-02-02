using System.Collections.Generic;
using TennisMatch.Models;

namespace TennisMatch.Tests.Models
{
    public class SetResultTest
    {
        public int ExpectedPlayer1Games { get; }
        public int ExpectedPlayer2Games { get; }

        public List<Game> Games { get; }

        public SetResultTest(int player1, int player2)
        {
            ExpectedPlayer1Games = player1;
            ExpectedPlayer2Games = player2;

            Games = new List<Game>();
        }
    }

    public class SetResultTestBuilder
    {
        private SetResultTest setResultTest;

        public SetResultTestBuilder(int player1, int player2)
        {
            setResultTest = new SetResultTest(player1, player2);
        }

        public SetResultTestBuilder AddGame(Game game)
        {
            setResultTest.Games.Add(game);
            return this;
        }

        public SetResultTest Build()
        {
            return setResultTest;
        }
    }
}
