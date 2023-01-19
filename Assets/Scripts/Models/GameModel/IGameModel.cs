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
        List<SingleQuestion> CurrentLevelQuestions { get; set; }
        int LevelScore { get; set; }
    }
}