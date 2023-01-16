using Signals;
using UnityEngine;
using Zenject;

namespace Login
{
    public class Login : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;


        public void Fire()
        {
            _signalBus.Fire(new ShowPopupSignal() { message = "Basma buraya",callback = () =>
            {
                Debug.Log("Geri Gittim");
            }});
        }
    }
}