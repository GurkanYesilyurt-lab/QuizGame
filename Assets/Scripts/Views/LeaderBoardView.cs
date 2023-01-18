using System.Collections.Generic;
using Controllers;
using DG.Tweening;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class LeaderBoardView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private LeaderboardController _leaderboardController;

        [SerializeField] private GameObject leaderboardPanel;
        [SerializeField] private InfiniteScroll scroll;
        [SerializeField] private Image progressImage;

        private List<LeaderboardSingleData> _leaderboardList;
        private int _initializedDataCount = 0;
        private int _lastLoadedPage = 0;

        private void Awake()
        {
            _leaderboardList = new List<LeaderboardSingleData>();
            _signalBus.Subscribe<ShowPopupSignal>(OnShowLeaderboardPanelAction);
            scroll.OnFill += OnFillItem;
            scroll.OnHeight += OnHeightItem;
            scroll.OnPull += OnPull;
        }

        private void OnShowLeaderboardPanelAction()
        {
            _lastLoadedPage = 0;
            leaderboardPanel.gameObject.SetActive(true);
            LoadData(true);
        }

        private void LoadData(bool isFirstInit)
        {
            progressImage.gameObject.SetActive(true);
            progressImage.fillAmount = 0;
            var progressTween = progressImage.DOFillAmount(1, 3).OnComplete(() =>
            {
                progressImage.fillAmount = 1;
                progressImage.gameObject.SetActive(false);
            });

            _leaderboardController.LoadLeaderboardData(_lastLoadedPage, (data) =>
            {
                progressTween?.Kill(true);
                _leaderboardList.AddRange(data.data);
                _lastLoadedPage = data.page;
                if (isFirstInit)
                {
                    _initializedDataCount = data.data.Count;
                    scroll.InitData(_initializedDataCount);
                }
                else
                {
                    _initializedDataCount += data.data.Count;
                    scroll.ApplyDataTo(_initializedDataCount, data.data.Count, InfiniteScroll.Direction.Bottom);
                }
            });
        }

        public void CloseLeaderboardPanel()
        {
            leaderboardPanel.gameObject.SetActive(false);
        }

        private void OnPull(InfiniteScroll.Direction obj)
        {
            _lastLoadedPage++;
            LoadData(false);
            Debug.Log("pull");
        }

        void OnFillItem(int index, GameObject item)
        {
            var singleItem = item.GetComponent<LeaderboardSingleItemView>();
            var singleData = _leaderboardList[index];
            singleItem.Init(singleData.rank.ToString(), singleData.nickname, singleData.score.ToString());
        }

        int OnHeightItem(int index)
        {
            return 150;
        }
    }
}