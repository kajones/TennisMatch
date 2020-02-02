using System;
using System.Collections.Generic;
using System.Linq;

namespace TennisMatch.Models
{
    public class Match
    {
        private readonly IGameResultGenerator gameResultGenerator;

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public IList<Set> Sets { get; set; }

        public Player Winner { get; private set; }

        public Match(Player player1, Player player2, IGameResultGenerator gameResultGenerator) 
        {
            Player1 = player1;
            Player2 = player2;

            Sets = new List<Set>();

            this.gameResultGenerator = gameResultGenerator;
        }

        public void Play()
        {
            while(Winner == null)
            {
                var set = new Set(Player1, Player2, gameResultGenerator);
                set.Play();

                Sets.Add(set);

                CheckForWinner();
            }
        }

        private void CheckForWinner()
        {
            var setsWon = Sets.GroupBy(s => s.Winner);
            var winner = setsWon.FirstOrDefault(g => g.Count() >= 2);
            if (winner == null) return;

            Winner = winner.Key;
        }
    }
}
