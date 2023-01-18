using System;
using UniRx;
using UnityEngine.Networking;

namespace Screens.QuestionScreen
{
    public class QuestionService
    {
        public IObservable<UnityWebRequestAsyncOperation> GetQuestions()
        {
            var _url = $"https://magegamessite.web.app/case1/questions.json";

            var requestDisposable = UnityWebRequest
                .Get(_url)
                .SendWebRequest()
                .AsAsyncOperationObservable();
            return requestDisposable;
        }
    }
}