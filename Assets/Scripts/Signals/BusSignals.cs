using System;

namespace Signals
{
    public struct ShowPopupSignal
    {
        public string message;
        public Action callback;
    }
    
    public struct ClearLayer
    {
        public int layer;
    }
}