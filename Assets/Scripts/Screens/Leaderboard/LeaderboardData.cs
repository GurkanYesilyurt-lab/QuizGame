using System.Collections.Generic;
using Newtonsoft.Json;

namespace Screens.Leaderboard
{
    public class LeaderboardData
    {
        [JsonProperty("page")] public int Page { get; set; }
        [JsonProperty("data")] public List<LeaderboardSingleData> Data { get; set; }
    }
}