using System.Collections.Generic;
using UnityEngine;

namespace Models.GameModel
{
    public class GameModel : IGameModel
    {
        public string PlayerName { get; set; }

        private readonly List<RectTransform> _layers;
        private readonly Popup.Popup _popup;
        

        
        
        
        public RectTransform GetLayerParent(int index)
        {
            return _layers[index];
        }
        public GameModel(List<RectTransform> layers,Popup.Popup popup)
        {
            _layers = layers;
            _popup = popup;
        }
        
        public Popup.Popup Popup()
        {
            return _popup;
        }
    }
}