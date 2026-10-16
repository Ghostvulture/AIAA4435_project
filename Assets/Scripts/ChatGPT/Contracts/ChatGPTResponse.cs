using System;
using System.Collections.Generic;
using Newtonsoft.Json;

//public class ChatGPTResponse
//{
//    [JsonProperty("id")]
//    public string Id { get; set; }

//    [JsonProperty("object")]
//    public string Object { get; set; }

//    [JsonProperty("created")]
//    public long Created { get; set; }

//    [JsonProperty("choices")]
//    public List<ChatGPTChoice> Choices { get; set; }

//    [JsonProperty("usage")]
//    public ChatGPTUsage Usage { get; set; }
//}

public class ChatGPTResponse
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("tasks")]
    public List<string> Tasks { get; set; }

    [JsonProperty("context")]
    public string Context { get; set; }

    [JsonProperty("landmark")]
    public string Landmark { get; set; }

    [JsonProperty("tourids")]
    public List<string> TourIDs { get; set; }
}

//public class ChatGPTMessage
//{
//    [JsonProperty("role")]
//    public string Role { get; set; }

//    [JsonProperty("content")]
//    public string Content { get; set; }
//}

//public class ChatGPTChoice
//{
//    [JsonProperty("index")]
//    public int Index { get; set; }

//    [JsonProperty("message")]
//    public ChatGPTMessage Message { get; set; }

//    [JsonProperty("finish_reason")]
//    public string FinishReason { get; set; }
//}

//public class ChatGPTUsage
//{
//    [JsonProperty("prompt_tokens")]
//    public int PromptTokens { get; set; }

//    [JsonProperty("completion_tokens")]
//    public int CompletionTokens { get; set; }

//    [JsonProperty("total_tokens")]
//    public int TotalTokens { get; set; }
//}
