using Installers;
using Models.GameModel;
using Signals;
using Zenject;

namespace Popup
{
    public class PopupMediator
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private PopupSettings popupSettings;

        public void ShowPopup(ShowPopupSignal container)
        {
            _gameModel.Popup().ShowPopup(container.message,popupSettings,container.callback);
        }
    }
}