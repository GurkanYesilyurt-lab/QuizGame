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

        private Tween _loadingTween;

        private void Awake()
        {
            _signalBus.Subscribe<LoadQuestionDataSignal>(ShowLoadingScreen);
        }

        private void StartLoadingTimer(float time)
        {
            _loadingTween?.Kill();
            _loadingTween = DOVirtual.Float(0, 1, time, (delta) => { loadingCircle.fillAmount = delta; })
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

        private void ShowLoadingScreen()
        {
            StartLoadingTimer(3);
            _isQuestionsLoaded = false;
            loadingScreen.SetActive(true);
            _questionController.LoadAllQuestions(() => { _isQuestionsLoaded = true; });
        }
    }
}