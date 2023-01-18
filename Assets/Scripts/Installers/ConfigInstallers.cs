using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    
    [CreateAssetMenu(menuName = "Config/Game Settings")]
    public class ConfigInstallers : ScriptableObjectInstaller<ConfigInstallers>
    {
        public PopupSettings popupSettings;
        public QuestionSettings questionSettings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(popupSettings);
            Container.BindInstance(questionSettings);
        }
    }
    
    
    [Serializable]
    public class PopupSettings
    {
        public float duration;
        public float showSpeed;
        public float hideSpeed;
        [Range(0,1000)] public float posY;
    }

    [Serializable]
    public class QuestionSettings
    {
        public float duration;
        public int rightAnswerScore;
        public int wrongAnswerScore;
        public int outOfTimeScore;
    }
    
    
   
}