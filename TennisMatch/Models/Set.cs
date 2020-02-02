namespace TennisMatch.Models
{
    public class Set
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public Set(Player player1, Player player2) => (Player1, Player2) = (player1, player2);

        public Player Play()
        {
            return Player1;
        }
    }
}
