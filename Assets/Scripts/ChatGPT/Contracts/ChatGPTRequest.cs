using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class ChatGPTRequest
{
    //[JsonProperty(PropertyName = "model")]
    //public string Model { get; set; }

    //[JsonProperty(PropertyName = "messages")]
    //public ChatGPTMessage[] Messages { get; set; }

    [JsonProperty(PropertyName = "question")]
    public string Question { get; set; }

    [JsonProperty(PropertyName = "position")]
    public string Position { get; set; }

    [JsonProperty(PropertyName = "landmark")]
    public string Landmark { get; set; }

    [JsonProperty(PropertyName = "history")]
    public List<string> History { get; set; }
}
