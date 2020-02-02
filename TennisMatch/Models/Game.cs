using System;

namespace TennisMatch.Models
{
    public class Game
    {
        public ScoreValues Player1Score { get; }
        public ScoreValues Player2Score { get; }

        public bool Player1IsWinner { get; }
        public bool Player2IsWinner { get; }

        public Game(ScoreValues player1Score, ScoreValues player2Score)
        {
            if (player1Score == ScoreValues.GameWon && player2Score == ScoreValues.GameWon)
                throw new Exception("It is impossible for both players to win the game");
            if (player1Score != ScoreValues.GameWon && player2Score != ScoreValues.GameWon)
                throw new Exception("Incomplete game - expect a player to have won the game");

            Player1Score = player1Score;
            Player2Score = player2Score;

            Player1IsWinner = Player1Score == ScoreValues.GameWon;
            Player2IsWinner = Player2Score == ScoreValues.GameWon;
        }
    }
}
