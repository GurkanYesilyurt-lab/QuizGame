using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Enums;
using Models.GameModel;
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
        [Inject] private IGameModel _gameModel;

        [SerializeField] private GameObject questionScreen;
        [SerializeField] private List<RectTransform> choiceButtonRectList;
        [SerializeField] private Button backBtn;
        [SerializeField] private TMP_Text questionTxt;
        [SerializeField] private TMP_Text timerTxt;
        [SerializeField] private TMP_Text scoreTxt;
        [SerializeField] private Color selectionColor;
        [SerializeField] private Color rightColor;
        [SerializeField] private Color wrongColor;

        private List<Button> _choiceButtons;
        private List<TMP_Text> _choiceTxtList;

        private float _buttonBeginXPos;
        private int _questionIndex = 0;

        private void Awake()
        {
            _signalBus.Subscribe<ShowQuestionScreenSignal>(QuestionScreenBeginGame);
            _choiceButtons = new List<Button>();
            _choiceTxtList = new List<TMP_Text>();
            InitButtons();
            // SetChoiceButtonBeginPositions();
            // MoveChoiceButtonsToScreen();
        }

        private void QuestionScreenBeginGame()
        {
            questionScreen.SetActive(true);
            _questionIndex = 0;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            var question = _gameModel.CurrentLevelQuestions[_questionIndex];
            InitPanel(question.question, question.choices);
            SetChoiceButtonBeginPositions();
            MoveChoiceButtonsToScreen();
        }

        private void InitButtons()
        {
            _buttonBeginXPos = -(GetComponent<RectTransform>().rect.width + choiceButtonRectList[0].rect.width);
            foreach (var btn in choiceButtonRectList)
            {
                _choiceButtons.Add(btn.GetComponent<Button>());
                _choiceTxtList.Add(btn.GetComponentInChildren<TMP_Text>());
            }

            for (int i = 0; i < _choiceButtons.Count; i++)
            {
                var choiceType = (ChoiceType) i;
                _choiceButtons[i].onClick.AddListener(() => { ChoiceSelected(choiceType); });
            }
        }

        private void SetChoiceButtonBeginPositions()
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

        private async void ChoiceSelected(ChoiceType choice)
        {
            //Check Answer

            await Task.Delay(1000);
            _questionIndex++;
            MoveButtonsOutOfScreen();
            await Task.Delay(2000);
            ShowQuestion();
        }
    }
}