using System.Collections.Generic;
using Newtonsoft.Json;

namespace Screens.QuestionScreen
{
    public class QuestionData
    {
       [JsonProperty("questions")] public List<SingleQuestion> Questions { get; set; }

    }
}