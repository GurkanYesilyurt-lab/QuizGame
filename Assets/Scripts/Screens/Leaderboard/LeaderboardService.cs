using System;
using UniRx;
using UnityEngine.Networking;

namespace Screens.Leaderboard
{
    public class LeaderboardService
    {
        public IObservable<UnityWebRequestAsyncOperation> GetLeaderboardData(int index = 0)
        {
            var _url = $"https://magegamessite.web.app/case1/leaderboard_page_{index}.json";

            var requestDisposable = UnityWebRequest
                .Get(_url)
                .SendWebRequest()
                .AsAsyncOperationObservable();
            return requestDisposable;
        }
    }
}