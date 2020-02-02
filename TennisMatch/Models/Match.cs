using System.Collections.Generic;

namespace TennisMatch.Models
{
    public class Match
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public IList<Set> Sets { get; set; }

        public Match(Player player1, Player player2) 
        {
            Player1 = player1;
            Player2 = player2;

            Sets = new List<Set>();
        }
    }
}
