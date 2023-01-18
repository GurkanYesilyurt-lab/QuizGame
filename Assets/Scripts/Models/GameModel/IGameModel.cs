using System.Collections.Generic;
using Screens.PopupScreen;
using Screens.QuestionScreen;
using UnityEngine;

namespace Models.GameModel
{
    public interface IGameModel
    {
        string PlayerName { get; set; }
        PopupView Popup();
        List<Question> CurrentLevelQuestions { get; set; }
        
        int LevelScore { get; set; }
    }
}