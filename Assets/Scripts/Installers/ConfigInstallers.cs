using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    
    [CreateAssetMenu(menuName = "Config/Game Settings")]

    public class ConfigInstallers : ScriptableObjectInstaller<ConfigInstallers>
    {
        public PopupSettings popupSettings;

        
        
        public override void InstallBindings()
        {
            Container.BindInstance(popupSettings);
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
    
    
   
}