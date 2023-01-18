using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Screens.MainScreen
{
    public class MainScreenView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private GameObject mainScreenPanel;
        [SerializeField] private Button playBtn;
        [SerializeField] private Button leaderboardBtn;

        private void Start()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _signalBus.Fire<ShowPopupSignal>(new ShowPopupSignal()
                {
                    message = "There is no internet connection!"
                });
            }
        }

        private void Awake()
        {
            _signalBus.Subscribe<ShowMainScreenSignal>(() => { SetVisibility(true); });
            _signalBus.Subscribe<LoadQuestionDataSignal>(() => { SetVisibility(false); });
            _signalBus.Subscribe<ShowLeaderboardScreenSignal>(() => { SetVisibility(false); });
            
            playBtn.onClick.AddListener(Play);
            leaderboardBtn.onClick.AddListener(ShowLeaderboard);
        }

        private void SetVisibility(bool isActive)
        {
            mainScreenPanel.gameObject.SetActive(isActive);
        }

        private void Play()
        {
            _signalBus.Fire<LoadQuestionDataSignal>();
        }

        private void ShowLeaderboard()
        {
            _signalBus.Fire<ShowLeaderboardScreenSignal>();
        }
    }
}