namespace TennisMatch.Models
{
    public class Game
    {
        public ScoreValues Player1Score { get; }
        public ScoreValues Player2Score { get; }

        public Game(ScoreValues player1Score, ScoreValues player2Score) => (Player1Score, Player2Score) = (player1Score, player2Score);
    }
}
