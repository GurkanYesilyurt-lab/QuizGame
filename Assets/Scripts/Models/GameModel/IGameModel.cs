using UnityEngine;

namespace Models.GameModel
{
    public interface IGameModel
    {
        string PlayerName { get; set; }
        Popup.Popup Popup();
    }
}