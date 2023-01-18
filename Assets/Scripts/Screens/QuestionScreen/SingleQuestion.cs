using System.Collections.Generic;
using Newtonsoft.Json;

namespace Screens.QuestionScreen
{
    public class SingleQuestion
    {
      [JsonProperty("category")]  public string Category { get; set; }
      [JsonProperty("question")]  public string Question { get; set; }
      [JsonProperty("choices")]  public List<string> Choices { get; set; }
      [JsonProperty("answer")]  public string Answer { get; set; }
    }
}