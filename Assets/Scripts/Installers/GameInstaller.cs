using System.Collections.Generic;
using Models.GameModel;
using Popup;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private List<RectTransform> layers;
        [SerializeField] private Popup.Popup popup;


        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<IGameModel>().To<GameModel>().AsSingle().WithArguments(layers,popup).NonLazy();
            
            Container.Bind<PopupMediator>().AsSingle().NonLazy();
            
            Container.DeclareSignal<ShowPopupSignal>();
            
            Container.BindSignal<ShowPopupSignal>()
                .ToMethod<PopupMediator>(x => x.ShowPopup).FromResolve();
        }
    }
}