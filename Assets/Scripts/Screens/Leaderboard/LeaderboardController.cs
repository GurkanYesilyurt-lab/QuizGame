using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

namespace Screens.Leaderboard
{
    public class LeaderboardController
    {
        private LeaderboardService _leaderboardService;

        public LeaderboardController()
        {
            _leaderboardService = new LeaderboardService();
        }


        public void LoadLeaderboardData(int pageIndex, Action<LeaderboardData> callback)
        {
            if (pageIndex > 1) return;
            var observable = _leaderboardService.GetLeaderboardData(pageIndex);
            observable.Subscribe(result =>
            {
                var data = result.webRequest.downloadHandler.text;
                var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(data);
                callback?.Invoke(leaderboardData);
            });
        }
    }


    public class LeaderboardSingleData
    {
        public int rank { get; set; }
        public string nickname { get; set; }
        public int score { get; set; }
    }

    public class LeaderboardData
    {
        public int page { get; set; }
        public List<LeaderboardSingleData> data { get; set; }
    }
}