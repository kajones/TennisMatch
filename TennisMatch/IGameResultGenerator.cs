using TennisMatch.Models;

namespace TennisMatch
{
    public interface IGameResultGenerator
    {
        Game GetResult();
    }
}
