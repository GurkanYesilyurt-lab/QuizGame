using System;
using System.Collections.Generic;
using Models.GameModel;
using Newtonsoft.Json;
using Services;
using UniRx;
using Zenject;

namespace Mopsicus.InfiniteScroll.Controllers
{
    public class QuestionController
    {
        [Inject] private IGameModel _gameModel;
        private QuestionService _questionService;
        public QuestionController()
        {
            _questionService = new QuestionService();
        }
        
        public Question GetQuestion(int index)
        {
          return _gameModel.CurrentLevelQuestions[index];
        }
        
        public void LoadAllQuestions(Action callback)
        {
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