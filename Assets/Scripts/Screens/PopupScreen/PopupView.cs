using System;
using System.Collections;
using DG.Tweening;
using Installers;
using TMPro;
using UnityEngine;

namespace Screens.PopupScreen
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private RectTransform popupParent;
        [SerializeField] private TMP_Text mText;

        private Coroutine _moveCoroutine;

        public void ShowPopup(string msg, PopupSettings settings,
            Action onPopupDisappear = null)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            gameObject.SetActive(true);
            _moveCoroutine = StartCoroutine(Do());

            IEnumerator Do()
            {
                DOTween.Kill(popupParent);
                popupParent.DOAnchorPosY(400, 0);
                mText.text = msg;
                popupParent.DOAnchorPosY(-settings.posY, settings.showSpeed).SetEase(Ease.InOutBack);
                yield return new WaitForSeconds(settings.duration);
                popupParent.DOAnchorPosY(400, settings.hideSpeed).SetEase(Ease.InOutBack);
                yield return new WaitForSeconds(settings.hideSpeed);
                onPopupDisappear?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}