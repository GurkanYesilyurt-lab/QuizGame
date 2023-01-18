using System;
using Models.GameModel;
using Signals;
using UnityEngine;
using Zenject;

namespace Login
{
    public class FirstLogin : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private IGameModel _gameModel;

        private void Awake()
        {
            
        }

        public void Fire()
        {
            // _signalBus.Fire(new ShowPopupSignal() { message = "Basma buraya",callback = () =>
            // {
            //     Debug.Log("Geri Gittim");
            // }});
            _signalBus.Fire(new LoadQuestionDataSignal() { });
        }
    }
}