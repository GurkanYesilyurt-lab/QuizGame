using System;

namespace Signals
{
    public struct ShowPopupSignal
    {
        public string message;
        public Action callback;
    }

    public struct ShowMainScreenSignal
    {
    }

    
    public struct ShowLeaderboardScreenSignal
    {
    }

    public struct LoadQuestionDataSignal
    {
    }
    
    public struct ShowQuestionScreenSignal
    {
    }
}