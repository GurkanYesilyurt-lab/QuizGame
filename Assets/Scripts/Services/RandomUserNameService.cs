using System;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class RandomUserNameService
    {
       
        public void Test()
        {
            
            var requestDisposable = UnityWebRequest
                .Get($"{Consts.BASE_URI}api/users/players")
                .SendWebRequest()
                .AsAsyncOperationObservable()
                .Subscribe(result =>
                {
                    Debug.Log( result.webRequest.downloadHandler.text);
                   
                });
            // var parallel = Observable.WhenAll(
            //     Get("http://google.com/"),
            //     Get("http://bing.com/"),
            //     Get("http://unity3d.com/"));
            //
            // parallel.Subscribe(xs =>
            // {
            //     Debug.Log(xs[0]); // google
            //     Debug.Log(xs[1]); // bing
            //     Debug.Log(xs[2]); // unity
            // });
        }
    }
}