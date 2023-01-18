using System.Collections.Generic;
using Mopsicus.InfiniteScroll.Controllers;

namespace Models.GameModel
{
    public class GameModel : IGameModel
    {
        public string PlayerName { get; set; }
        private readonly Popup.Popup _popup;
        public List<Question> CurrentLevelQuestions { get; set; }
        
        public GameModel(Popup.Popup popup)
        {
            _popup = popup;
            CurrentLevelQuestions = new List<Question>();
        }
        
        public Popup.Popup Popup()
        {
            return _popup;
        }
    }
}