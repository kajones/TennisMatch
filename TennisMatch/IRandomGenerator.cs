namespace TennisMatch
{
    public interface IRandomGenerator
    {
        bool DoesPlayer1WinGame();
        int WhichScore(int numberOfScoresAvailable);
    }
}
