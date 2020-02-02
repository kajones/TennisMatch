using System.Collections.Generic;

namespace TennisMatch.Models
{
    public class Set
    {
        private readonly IGameResultGenerator gameResultGenerator;

        public Player Player1 { get; }
        public Player Player2 { get; }

        public IList<Game> Games { get; }

        public Player Winner { get; private set; }
        public int Player1Games { get; private set; }
        public int Player2Games { get; private set; }

        public Set(Player player1, Player player2, IGameResultGenerator gameResultGenerator)
        {
            Player1 = player1;
            Player2 = player2;
            Games = new List<Game>();

            this.gameResultGenerator = gameResultGenerator;
        }

        public void Play()
        {
            while (Winner == null)
            {
                var game = gameResultGenerator.GetResult();
                Games.Add(game);

                if (game.Player1IsWinner) Player1Games++;
                if (game.Player2IsWinner) Player2Games++;

                CheckForWinner();
            }
        }

        private void CheckForWinner()
        {
            if (HasWon(Player1Games, Player2Games))
            {
                Winner = Player1;
                return;
            }

            if (HasWon(Player2Games, Player1Games))
                Winner = Player2;
        }

        private bool HasWon(int playerAGamesWon, int playerBGamesWon)
        {
            if (playerAGamesWon >= 6 && (playerAGamesWon - playerBGamesWon) >= 2)
                return true;

            return false;
        }
    }
}
