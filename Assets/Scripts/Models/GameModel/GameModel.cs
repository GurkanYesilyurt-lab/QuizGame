namespace Models.GameModel
{
    public class GameModel : IGameModel
    {
        public string PlayerName { get; set; }
        private readonly Popup.Popup _popup;
        
        
        
        public GameModel(Popup.Popup popup)
        {
            _popup = popup;
        }
        
        public Popup.Popup Popup()
        {
            return _popup;
        }
    }
}