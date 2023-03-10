using System;
using Installers;
using Models.GameModel;
using Newtonsoft.Json;
using UniRx;
using Zenject;

namespace Screens.QuestionScreen
{
    public class QuestionController
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private QuestionSettings _questionSettings;

        public QuestionSettings QuestionSettings => _questionSettings;
        public int Score => _gameModel.LevelScore;

        private readonly QuestionService _questionService;


        public QuestionController()
        {
            _questionService = new QuestionService();
        }

        public SingleQuestion GetQuestion(int index)
        {
            return _gameModel.CurrentLevelQuestions[index];
        }

        public bool CheckAnswerIsTrue(int index, ChoiceType choice)
        {
            return _gameModel.CurrentLevelQuestions[index].Answer == choice.ToString();
        }

        public ChoiceType GetAnswer(int questionIndex)
        {
            var answer = _gameModel.CurrentLevelQuestions[questionIndex].Answer;
            var choice= Enum.Parse<ChoiceType>(answer);
            return choice;
        }

        public void SetScore(int score)
        {
            _gameModel.LevelScore = score;
        }

        public void LoadAllQuestions(Action callback)
        {
            _gameModel.LevelScore = 0;
            _questionService.GetQuestions().Subscribe(result =>
            {
                var questionData = JsonConvert.DeserializeObject<QuestionData>(result.webRequest.downloadHandler.text);
                _gameModel.CurrentLevelQuestions = questionData.Questions;
                callback?.Invoke();
            });
        }
    }
}