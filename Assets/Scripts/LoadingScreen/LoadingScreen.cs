using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image loadingCircle;

        [Button]
        public void StarLoadingTimer(float time)
        {
            DOVirtual.Float(0, 1, time, (delta) =>
            {
                loadingCircle.fillAmount = delta;
            }).SetEase(Ease.Linear);
        }
    }
}