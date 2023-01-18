using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Screens.Leaderboard
{
    public class InfiniteScroll : MonoBehaviour, IDropHandler
    {
        private const int UPDATE_TIME_DIFF = 500;
        private const float SCROLL_SPEED = 50f;
        private const float SCROLL_DURATION = 0.25f;

        public enum Direction
        {
            Top = 0,
            Bottom = 1,
        }

        public delegate int HeightItem(int index);

        public event HeightItem OnHeight;
        public Action<int, GameObject> onFill = delegate { };
        public Action<Direction> onPull = delegate { };

        [Header("Item settings")] public GameObject prefab;

        [Header("Padding")] public int topPadding = 10;
        public int bottomPadding = 10;
        public int itemSpacing = 2;

        [Header("Labels")] public TMP_FontAsset labelsFont;
        public string topPullLabel = "Pull to refresh";
        public string topReleaseLabel = "Release to load";
        public string bottomPullLabel = "Pull to refresh";
        public string bottomReleaseLabel = "Release to load";

        [Header("Directions")] public bool isPullTop = true;
        public bool isPullBottom = true;

        [Header("Offsets")] public float pullValue = 1.5f;
        public float labelOffset = 85f;

        [HideInInspector] public TextMeshProUGUI topLabel;

        [HideInInspector] public TextMeshProUGUI bottomLabel;

        private ScrollRect _scroll;
        private RectTransform _content;
        private Rect _container;
        private RectTransform[] _rects;

        private GameObject[] _views;
        private bool _isCanLoadUp;
        private bool _isCanLoadDown;
        private bool _isCanLoadLeft;
        private bool _isCanLoadRight;
        private int _previousPosition = -1;
        private int _count;
        private Dictionary<int, int> _heights;
        private Dictionary<int, float> _positions;
        private DateTime _lastMoveTime;
        private float _previousScrollPosition;
        private int _saveStepPosition = -1;

        void Awake()
        {
            _container = GetComponent<RectTransform>().rect;
            _scroll = GetComponent<ScrollRect>();
            _scroll.onValueChanged.AddListener(OnScrollChange);
            _content = _scroll.viewport.transform.GetChild(0).GetComponent<RectTransform>();
            _heights = new Dictionary<int, int>();
            _positions = new Dictionary<int, float>();
            CreateLabels();
        }

        void Update()
        {
            UpdateVertical();
        }

        void UpdateVertical()
        {
            if (_count == 0)
            {
                return;
            }

            var topPosition = _content.anchoredPosition.y - itemSpacing;
            if (topPosition <= 0f && _rects[0].anchoredPosition.y < -topPadding - 10f)
            {
                InitData(_count);
                return;
            }

            if (topPosition < 0f)
            {
                return;
            }

            if (!_positions.ContainsKey(_previousPosition) || !_heights.ContainsKey(_previousPosition))
            {
                return;
            }

            float itemPosition = Mathf.Abs(_positions[_previousPosition]) + _heights[_previousPosition];
            int position = (topPosition > itemPosition) ? _previousPosition + 1 : _previousPosition - 1;
            int border = (int) (_positions[0] + _heights[0]);
            int step = (int) ((topPosition + topPosition / 1.25f) / border);
            if (step != _saveStepPosition)
            {
                _saveStepPosition = step;
            }
            else
            {
                return;
            }

            if (position < 0 || _previousPosition == position || _scroll.velocity.y == 0f)
            {
                return;
            }

            if (position > _previousPosition)
            {
                if (position - _previousPosition > 1)
                {
                    position = _previousPosition + 1;
                }

                int newPosition = position % _views.Length;
                newPosition--;
                if (newPosition < 0)
                {
                    newPosition = _views.Length - 1;
                }

                int index = position + _views.Length - 1;
                if (index < _count)
                {
                    Vector2 pos = _rects[newPosition].anchoredPosition;
                    pos.y = _positions[index];
                    _rects[newPosition].anchoredPosition = pos;
                    Vector2 size = _rects[newPosition].sizeDelta;
                    size.y = _heights[index];
                    _rects[newPosition].sizeDelta = size;
                    _views[newPosition].name = index.ToString();
                    onFill(index, _views[newPosition]);
                }
            }
            else
            {
                if (_previousPosition - position > 1)
                {
                    position = _previousPosition - 1;
                }

                int newIndex = position % _views.Length;
                Vector2 pos = _rects[newIndex].anchoredPosition;
                pos.y = _positions[position];
                _rects[newIndex].anchoredPosition = pos;
                Vector2 size = _rects[newIndex].sizeDelta;
                size.y = _heights[position];
                _rects[newIndex].sizeDelta = size;
                _views[newIndex].name = position.ToString();
                onFill(position, _views[newIndex]);
            }

            _previousPosition = position;
        }

        void OnScrollChange(Vector2 vector)
        {
            ScrollChangeVertical(vector);
        }

        void ScrollChangeVertical(Vector2 vector)
        {
            _isCanLoadUp = false;
            _isCanLoadDown = false;
            if (_views == null)
            {
                return;
            }

            float y = 0f;
            float z = 0f;
            bool isScrollable = (_scroll.verticalNormalizedPosition != 1f && _scroll.verticalNormalizedPosition != 0f);
            y = _content.anchoredPosition.y;
            if (isScrollable)
            {
                if (_scroll.verticalNormalizedPosition < 0f)
                {
                    z = y - _previousScrollPosition;
                }
                else
                {
                    _previousScrollPosition = y;
                }
            }
            else
            {
                z = y;
            }

            if (y < -labelOffset && isPullTop)
            {
                topLabel.gameObject.SetActive(true);
                topLabel.text = topPullLabel;
                if (y < -labelOffset * pullValue)
                {
                    topLabel.text = topReleaseLabel;
                    _isCanLoadUp = true;
                }
            }
            else
            {
                topLabel.gameObject.SetActive(false);
            }

            if (z > labelOffset && isPullBottom)
            {
                bottomLabel.gameObject.SetActive(true);
                bottomLabel.text = bottomPullLabel;
                if (z > labelOffset * pullValue)
                {
                    bottomLabel.text = bottomReleaseLabel;
                    _isCanLoadDown = true;
                }
            }
            else
            {
                bottomLabel.gameObject.SetActive(false);
            }
        }


        public void OnDrop(PointerEventData eventData)
        {
            DropVertical();
        }

        void DropVertical()
        {
            if (_isCanLoadUp)
            {
                onPull(Direction.Top);
            }
            else if (_isCanLoadDown)
            {
                onPull(Direction.Bottom);
            }

            _isCanLoadUp = false;
            _isCanLoadDown = false;
        }

        public void InitData(int count)
        {
            InitVertical(count);
        }

        void InitVertical(int count)
        {
            var height = CalcSizesPositions(count);
            CreateViews();
            _previousPosition = 0;
            _count = count;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, height);
            var pos = _content.anchoredPosition;
            var size = Vector2.zero;
            pos.y = 0f;
            _content.anchoredPosition = pos;
            for (int i = 0; i < _views.Length; i++)
            {
                _views[i].gameObject.SetActive(i < count);
                if (i + 1 > _count)
                {
                    continue;
                }

                pos = _rects[i].anchoredPosition;
                pos.y = _positions[i];
                pos.x = 0f;
                _rects[i].anchoredPosition = pos;
                size = _rects[i].sizeDelta;
                size.y = _heights[i];
                _rects[i].sizeDelta = size;
                _views[i].name = i.ToString();
                onFill(i, _views[i]);
            }
        }

        float CalcSizesPositions(int count)
        {
            return CalcSizesPositionsVertical(count);
        }

        float CalcSizesPositionsVertical(int count)
        {
            _heights.Clear();
            _positions.Clear();
            float result = 0f;
            for (int i = 0; i < count; i++)
            {
                _heights[i] = OnHeight(i);
                _positions[i] = -(topPadding + i * itemSpacing + result);
                result += _heights[i];
            }

            result += topPadding + bottomPadding + (count == 0 ? 0 : ((count - 1) * itemSpacing));
            return result;
        }

        public void ApplyDataTo(int count, int newCount, Direction direction)
        {
            ApplyDataToVertical(count, newCount, direction);
        }

        void ApplyDataToVertical(int count, int newCount, Direction direction)
        {
            _count = count;
            if (_count <= _views.Length)
            {
                InitData(count);
                return;
            }

            float height = CalcSizesPositions(count);
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, height);
            Vector2 pos = _content.anchoredPosition;
            if (direction == Direction.Top)
            {
                float y = 0f;
                for (int i = 0; i < newCount; i++)
                {
                    y += _heights[i] + itemSpacing;
                }

                pos.y = y;
                _previousPosition = newCount;
            }
            else
            {
                float h = 0f;
                for (int i = _heights.Count - 1; i >= _heights.Count - newCount; i--)
                {
                    h += _heights[i] + itemSpacing;
                }

                pos.y = height - h - _container.height;
            }

            _content.anchoredPosition = pos;
            float _topPosition = _content.anchoredPosition.y - itemSpacing;
            float itemPosition = Mathf.Abs(_positions[_previousPosition]) + _heights[_previousPosition];
            int position = (_topPosition > itemPosition) ? _previousPosition + 1 : _previousPosition - 1;
            if (position < 0)
            {
                _previousPosition = 0;
                position = 1;
            }

            for (int i = 0; i < _views.Length; i++)
            {
                int newIndex = position % _views.Length;
                if (newIndex < 0)
                {
                    continue;
                }

                _views[newIndex].gameObject.SetActive(true);
                _views[newIndex].name = position.ToString();
                onFill(position, _views[newIndex]);
                pos = _rects[newIndex].anchoredPosition;
                pos.y = _positions[position];
                _rects[newIndex].anchoredPosition = pos;
                Vector2 size = _rects[newIndex].sizeDelta;
                size.y = _heights[position];
                _rects[newIndex].sizeDelta = size;
                position++;
                if (position == _count)
                {
                    break;
                }
            }
        }

        void CreateViews()
        {
            CreateViewsVertical();
        }

        void CreateViewsVertical()
        {
            if (_views != null)
            {
                return;
            }

            GameObject clone;
            RectTransform rect;
            var height = 0;
            foreach (int item in _heights.Values)
            {
                height += item;
            }

            height = height / _heights.Count;
            int fillCount = Mathf.RoundToInt(_container.height / height) + 4;
            _views = new GameObject[fillCount];
            for (int i = 0; i < fillCount; i++)
            {
                clone = (GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity);
                clone.transform.SetParent(_content);
                clone.transform.localScale = Vector3.one;
                clone.transform.localPosition = Vector3.zero;
                rect = clone.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0.5f, 1f);
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = Vector2.one;
                rect.offsetMax = Vector2.zero;
                rect.offsetMin = Vector2.zero;
                _views[i] = clone;
            }

            _rects = new RectTransform[_views.Length];
            for (int i = 0; i < _views.Length; i++)
            {
                _rects[i] = _views[i].gameObject.GetComponent<RectTransform>();
            }
        }

        void CreateLabels()
        {
            CreateLabelsVertical();
        }

        void CreateLabelsVertical()
        {
            GameObject topText = new GameObject("TopLabel");
            topText.transform.SetParent(_scroll.viewport.transform);
            topLabel = topText.AddComponent<TextMeshProUGUI>();
            topLabel.font = labelsFont;
            topLabel.fontSize = 24;
            topLabel.transform.localScale = Vector3.one;
            topLabel.alignment = TextAlignmentOptions.Center;
            topLabel.text = topPullLabel;
            RectTransform rect = topLabel.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = Vector2.one;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = new Vector2(0f, -labelOffset);
            rect.anchoredPosition3D = Vector3.zero;
            topText.SetActive(false);
            GameObject bottomText = new GameObject("BottomLabel");
            bottomText.transform.SetParent(_scroll.viewport.transform);
            bottomLabel = bottomText.AddComponent<TextMeshProUGUI>();
            bottomLabel.font = labelsFont;
            bottomLabel.fontSize = 24;
            bottomLabel.transform.localScale = Vector3.one;
            bottomLabel.alignment = TextAlignmentOptions.Center;
            bottomLabel.text = bottomPullLabel;
            bottomLabel.transform.position = Vector3.zero;
            rect = bottomLabel.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(1f, 0f);
            rect.offsetMax = new Vector2(0f, labelOffset);
            rect.offsetMin = Vector2.zero;
            rect.anchoredPosition3D = Vector3.zero;
            bottomText.SetActive(false);
        }
    }
}