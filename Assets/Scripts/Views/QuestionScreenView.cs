using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using DG.Tweening;
using Enums;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class QuestionScreenView : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private QuestionController _questionController;

        [SerializeField] private GameObject questionScreen;
        [SerializeField] private GameObject levelEndPanel;
        [SerializeField] private List<RectTransform> choiceButtonRectList;
        [SerializeField] private Button backBtn;
        [SerializeField] private Button levelEndBtn;
        [SerializeField] private TMP_Text questionTxt;
        [SerializeField] private TMP_Text timerTxt;
        [SerializeField] private TMP_Text scoreTxt;
        [SerializeField] private TMP_Text addScoreTxt;
        [SerializeField] private Color selectionColor;
        [SerializeField] private Color rightColor;
        [SerializeField] private Color wrongColor;
        [SerializeField] private Image timeBarImg;

        private List<Button> _choiceButtonList;
        private List<TMP_Text> _choiceTxtList;
        private List<Image> _choiceImageList;
        private Tweener _timerTween;
        private float _buttonBeginXPos;
        private int _questionIndex = 0;
        private Vector2 _addScoreTextDefaultPos;

        private void Awake()
        {
            _addScoreTextDefaultPos = addScoreTxt.rectTransform.anchoredPosition;
            _signalBus.Subscribe<ShowQuestionScreenSignal>(ShowFirstQuestion);
            _choiceButtonList = new List<Button>();
            _choiceTxtList = new List<TMP_Text>();
            _choiceImageList = new List<Image>();
            InitButtons();
        }

        private void InitButtons()
        {
            _buttonBeginXPos = -(GetComponent<RectTransform>().rect.width + choiceButtonRectList[0].rect.width);
            backBtn.onClick.AddListener(GoToMainMenu);
            levelEndBtn.onClick.AddListener(GoToMainMenu);
            foreach (var btn in choiceButtonRectList)
            {
                _choiceButtonList.Add(btn.GetComponent<Button>());
                _choiceTxtList.Add(btn.GetComponentInChildren<TMP_Text>());
                _choiceImageList.Add(btn.GetComponent<Image>());
            }

            for (int i = 0; i < _choiceButtonList.Count; i++)
            {
                var choiceType = (ChoiceType) i;
                _choiceButtonList[i].onClick.AddListener(() =>
                {
                    ChoiceSelected(choiceType);
                    _choiceImageList[(int) choiceType].color = selectionColor;
                });
            }
        }

        public void GoToMainMenu()
        {
            questionScreen.SetActive(false);
            levelEndPanel.SetActive(false);
        }

        private void ShowFirstQuestion()
        {
            scoreTxt.text = "Score: 0";
            questionScreen.SetActive(true);
            _questionIndex = 0;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            var question = _questionController.GetQuestion(_questionIndex);
            InitTimer();
            InitPanel(question.question, question.choices);
            SetChoiceButtonInteractableStatus(true);
            SetChoiceButtonsToDefaultColor();
            SetChoiceButtonsBeginPosition();
            MoveChoiceButtonsToScreen();
        }

        private void InitTimer()
        {
            timeBarImg.fillAmount = 0;
            var duration = _questionController.QuestionSettings.duration;
            _timerTween = DOVirtual.Float(0, duration, duration,
                    (value) =>
                    {
                        timeBarImg.fillAmount = value / duration;
                        timerTxt.text = $"00:{(int) (duration - value)}";
                    })
                .SetEase(Ease.Linear)
                .OnComplete(() => { SetChoiceButtonInteractableStatus(false); });
        }

        private void SetChoiceButtonsBeginPosition()
        {
            foreach (var btnRect in choiceButtonRectList)
            {
                DOTween.Kill(btnRect);
                btnRect.DOAnchorPosX(_buttonBeginXPos, 0);
            }
        }

        private void MoveChoiceButtonsToScreen()
        {
            for (int i = 0; i < choiceButtonRectList.Count; i++)
            {
                choiceButtonRectList[i].DOAnchorPosX(0, 1).SetEase(Ease.InOutBack).SetDelay(i * .05f);
            }
        }

        private void MoveButtonsOutOfScreen()
        {
            for (int i = 0; i < choiceButtonRectList.Count; i++)
            {
                choiceButtonRectList[i].DOAnchorPosX(-_buttonBeginXPos, 1).SetEase(Ease.InOutBack).SetDelay(i * .05f);
            }
        }

        public void InitPanel(string question, List<string> answers)
        {
            questionTxt.text = question;
            for (int i = 0; i < answers.Count; i++)
            {
                _choiceTxtList[i].text = answers[i].Replace(System.Environment.NewLine, "");
            }
        }

        private void SetChoiceButtonInteractableStatus(bool isActive)
        {
            foreach (var choiceButton in _choiceButtonList)
            {
                choiceButton.interactable = isActive;
            }
        }

        private void SetChoiceButtonsToDefaultColor()
        {
            foreach (var choiceButton in _choiceImageList)
            {
                choiceButton.color = Color.white;
            }
        }

        private async void SetScore(bool isTrue)
        {
            var incrementScore = isTrue
                ? _questionController.QuestionSettings.rightAnswerScore
                : _questionController.QuestionSettings.wrongAnswerScore;

            addScoreTxt.rectTransform.anchoredPosition = _addScoreTextDefaultPos;
            addScoreTxt.text = isTrue ? "+" + incrementScore : incrementScore.ToString();
            addScoreTxt.gameObject.SetActive(true);
            addScoreTxt.rectTransform.DOMoveY(scoreTxt.rectTransform.position.y, 1).OnComplete(() =>
            {
                addScoreTxt.gameObject.SetActive(false);
            });
            await Task.Delay(800);
            var finalScore = _questionController.Score + incrementScore;
            DOVirtual.Int(_questionController.Score, finalScore, .4f,
                (value) => { scoreTxt.text = $"Score: {value}"; });
            _questionController.SetScore(finalScore);
        }

        private void SetButtonColor(int index,bool isTrue)
        {
            _choiceImageList[index].color = isTrue ? rightColor : wrongColor;
        }


        private async void ChoiceSelected(ChoiceType choice)
        {
            SetChoiceButtonInteractableStatus(false);
            _timerTween.Kill();
            await Task.Delay(1000);
            var isTrue = _questionController.GetAnswer(_questionIndex, choice);
            SetScore(isTrue);
            SetButtonColor((int)choice,isTrue);
            _questionIndex++;
            MoveButtonsOutOfScreen();
            await Task.Delay(2000);
            if (_questionIndex>=10)
            {
                levelEndPanel.SetActive(true);
                questionScreen.SetActive(false);
                return;
            }
            ShowQuestion();
        }
    }
}