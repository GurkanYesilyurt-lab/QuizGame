using Models.GameModel;
using Screens.Leaderboard;
using Screens.PopupScreen;
using Screens.QuestionScreen;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PopupView popupView;


        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            BindModels();
            BindControllers();
            DeclareSignals();
            BindSignalsToControllers();
          
        }

        private void BindSignalsToControllers()
        {
            Container.BindSignal<ShowPopupSignal>()
                .ToMethod<PopupController>(x => x.ShowPopup).FromResolve();
        }

        private void BindModels()
        {
            Container.Bind<IGameModel>().To<GameModel>().AsSingle().WithArguments(popupView).NonLazy();
        }

        private void BindControllers()
        {
            Container.Bind<PopupController>().AsSingle().NonLazy();
            Container.Bind<LeaderboardController>().AsSingle().NonLazy();
            Container.Bind<QuestionController>().AsSingle().NonLazy();
        }


        private void DeclareSignals()
        {
            Container.DeclareSignal<ShowPopupSignal>();
            Container.DeclareSignal<ShowLeaderboardScreenSignal>();
            Container.DeclareSignal<LoadQuestionDataSignal>();
            Container.DeclareSignal<ShowQuestionScreenSignal>();
            Container.DeclareSignal<ShowMainScreenSignal>();
        }
    }
}