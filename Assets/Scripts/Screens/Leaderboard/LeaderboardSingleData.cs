using Newtonsoft.Json;

namespace Screens.Leaderboard
{
    public class LeaderboardSingleData
    {
        [JsonProperty("rank")] public int Rank { get; set; }
        [JsonProperty("nickname")] public string Nickname { get; set; }
        [JsonProperty("score")] public int Score { get; set; }
    }
}