using System;
using System.Collections.Generic;
using Enums;
using Installers;
using Models.GameModel;
using Newtonsoft.Json;
using Services;
using UniRx;
using Zenject;

namespace Controllers
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

        public Question GetQuestion(int index)
        {
            return _gameModel.CurrentLevelQuestions[index];
        }

        public bool GetAnswer(int index, ChoiceType choice)
        {
            return _gameModel.CurrentLevelQuestions[index].answer == choice.ToString();
        }

        public void SetScore(int score)
        {
            _gameModel.LevelScore = score;
        }

        public void OutOfTimeSetScore()
        {
            _gameModel.LevelScore += _questionSettings.outOfTimeScore;
        }

        public void LoadAllQuestions(Action callback)
        {
            _gameModel.LevelScore = 0;
            _questionService.GetQuestions().Subscribe(result =>
            {
                var questionData = JsonConvert.DeserializeObject<QuestionData>(result.webRequest.downloadHandler.text);
                _gameModel.CurrentLevelQuestions = questionData.questions;
                callback?.Invoke();
            });
        }
    }

    public class Question
    {
        public string category { get; set; }
        public string question { get; set; }
        public List<string> choices { get; set; }
        public string answer { get; set; }
    }

    public class QuestionData
    {
        public List<Question> questions { get; set; }
    }
}