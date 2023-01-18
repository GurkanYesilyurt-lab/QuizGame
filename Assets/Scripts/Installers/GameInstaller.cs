using System.Collections.Generic;
using Controllers;
using Models.GameModel;
using Popup;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Popup.Popup popup;


        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<IGameModel>().To<GameModel>().AsSingle().WithArguments(popup).NonLazy();
            
            Container.Bind<PopupMediator>().AsSingle().NonLazy();
            Container.Bind<LeaderboardController>().AsSingle().NonLazy();
            Container.Bind<QuestionController>().AsSingle().NonLazy();

            Container.DeclareSignal<ShowPopupSignal>();
            Container.DeclareSignal<OpenTutorialPanelSignal>();
            Container.DeclareSignal<LoadQuestionDataSignal>();
            Container.DeclareSignal<ShowQuestionScreenSignal>();
            
            Container.BindSignal<ShowPopupSignal>()
                .ToMethod<PopupMediator>(x => x.ShowPopup).FromResolve();

        }
    }
}