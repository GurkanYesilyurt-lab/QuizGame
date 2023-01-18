using System.Collections.Generic;
using Mopsicus.InfiniteScroll.Controllers;
using UnityEngine;

namespace Models.GameModel
{
    public interface IGameModel
    {
        string PlayerName { get; set; }
        Popup.Popup Popup();
        List<Question> CurrentLevelQuestions { get; set; }
    }
}