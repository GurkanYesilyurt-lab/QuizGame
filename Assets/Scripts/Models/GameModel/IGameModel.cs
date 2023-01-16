using UnityEngine;

namespace Models.GameModel
{
    public interface IGameModel
    {
        string PlayerName { get; set; }
        RectTransform GetLayerParent(int index);
        Popup.Popup Popup();
    }
}