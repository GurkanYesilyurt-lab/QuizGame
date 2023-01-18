using DG.Tweening;
using Screens.QuestionScreen;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Screens.LoadingScreen
{
    public class LoadingScreenView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private QuestionController _questionController;

        [SerializeField] private Image loadingCircle;
        [SerializeField] private GameObject loadingScreen;

        private bool _isQuestionsLoaded;

        private void Awake()
        {
            _signalBus.Subscribe<LoadQuestionDataSignal>(ShowLoadingScreen);
        }

        public void StarLoadingTimer(float time)
        {
            DOVirtual.Float(0, 1, time, (delta) => { loadingCircle.fillAmount = delta; })
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_isQuestionsLoaded)
                    {
                        _signalBus.Fire<ShowQuestionScreenSignal>();
                        loadingScreen.SetActive(false);
                    }
                    else
                    {
                        _signalBus.Fire(new ShowPopupSignal()
                        {
                            message = "Check your connection"
                        });
                    }
                });
        }

        public void ShowLoadingScreen()
        {
            StarLoadingTimer(3);
            _isQuestionsLoaded = false;
            loadingScreen.SetActive(true);

            _questionController.LoadAllQuestions(() => { _isQuestionsLoaded = true; });
        }
    }
}