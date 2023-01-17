using System.Collections.Generic;
using Models.GameModel;
using Mopsicus.InfiniteScroll.Controllers;
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
            Container.Bind<RandomUserNameService>().AsSingle().NonLazy();
            Container.Bind<LeaderboardController>().AsSingle().NonLazy();
            
            
            Container.DeclareSignal<ShowPopupSignal>();
            Container.DeclareSignal<OpenTutorialPanelSignal>();
            
            Container.BindSignal<ShowPopupSignal>()
                .ToMethod<PopupMediator>(x => x.ShowPopup).FromResolve();

        }
    }
}