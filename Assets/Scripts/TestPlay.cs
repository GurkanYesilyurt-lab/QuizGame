using Signals;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class TestPlay: MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;


        public void Test()
        {
            _signalBus.Fire<LoadQuestionDataSignal>();
        }
    }
}