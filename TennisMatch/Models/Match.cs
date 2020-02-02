using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public string GetResult()
        {
            var player1First = Winner == Player1;

            var resultBuilder = new StringBuilder();
            foreach(var set in Sets)
            {
                if (player1First)
                    resultBuilder.Append($"{set.Player1Games}-{set.Player2Games} ");
                else
                    resultBuilder.Append($"{set.Player2Games}-{set.Player1Games} ");
            }
            return resultBuilder.ToString().Trim();
        }
    }
}
