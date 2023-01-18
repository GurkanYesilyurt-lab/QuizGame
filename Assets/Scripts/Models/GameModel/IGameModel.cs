using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Models.GameModel
{
    public interface IGameModel
    {
        string PlayerName { get; set; }
        Popup.Popup Popup();
        List<Question> CurrentLevelQuestions { get; set; }
        
        int LevelScore { get; set; }
    }
}