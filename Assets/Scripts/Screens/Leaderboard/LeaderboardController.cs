using System;
using Newtonsoft.Json;
using Signals;
using UniRx;
using Zenject;

namespace Screens.Leaderboard
{
    public class LeaderboardController
    {
        [Inject] private SignalBus _signalBus;
        private readonly LeaderboardService _leaderboardService;

        public LeaderboardController()
        {
            _leaderboardService = new LeaderboardService();
        }


        public void LoadLeaderboardData(int pageIndex, Action<LeaderboardData> callback)
        {
            if (pageIndex > 1)
            {
                _signalBus.Fire(new ShowPopupSignal()
                {
                    message = "Has no more data!"
                });
                return;
            }

            var observable = _leaderboardService.GetLeaderboardData(pageIndex);
            observable.Subscribe(result =>
            {
                var data = result.webRequest.downloadHandler.text;
                var leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(data);
                callback?.Invoke(leaderboardData);
            });
        }
    }
}