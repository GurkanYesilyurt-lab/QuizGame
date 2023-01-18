using System.Collections.Generic;
using Screens.PopupScreen;
using Screens.QuestionScreen;

namespace Models.GameModel
{
    public class GameModel : IGameModel
    {
        public string PlayerName { get; set; }
        private readonly PopupView _popupView;
        public List<SingleQuestion> CurrentLevelQuestions { get; set; }
        public int LevelScore { get; set; }

        public GameModel(PopupView popupView)
        {
            _popupView = popupView;
            CurrentLevelQuestions = new List<SingleQuestion>();
        }
        
        public PopupView Popup()
        {
            return _popupView;
        }
    }
}