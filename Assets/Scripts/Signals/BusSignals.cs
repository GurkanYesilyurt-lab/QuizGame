using System;

namespace Signals
{
    public struct ShowPopupSignal
    {
        public string message;
        public Action callback;
    }

    public struct OpenTutorialPanelSignal
    {
        
    }
    
    public struct LoadQuestionDataSignal
    {
        
    }
    
    public struct ShowQuestionScreenSignal
    {
        
    }
}