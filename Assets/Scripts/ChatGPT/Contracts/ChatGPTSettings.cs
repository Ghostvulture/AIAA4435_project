using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTSettings", menuName = "ChatGPT/ChatGPTSettings")]

public class ChatGPTSettings : ScriptableObject
{
	public string apiURL;
    //https://api.openai.com/v1/chat/completions

	public string apiKey;

	public string apiModel;

	public string apiOrganization;

	public bool debug;

	public string[] reminders;
}