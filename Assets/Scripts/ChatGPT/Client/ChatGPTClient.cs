using DilmerGames.Core.Singletons;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;

public class ChatGPTClient : Singleton<ChatGPTClient>
{
	[SerializeField]
	private ChatGPTSettings chatGPTSettings;

	public IEnumerator Ask(string prompt, string position, string landmark,List<string> history, System.Action<ChatGPTResponse> callBack)
    {
		var url = chatGPTSettings.debug ? $"{chatGPTSettings.apiURL}?debug=true" : chatGPTSettings.apiURL;

		using(UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
			byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(
				JsonConvert.SerializeObject(new ChatGPTRequest
				{
					Question = prompt,
					Position = position,
					Landmark = landmark,
					History = history
                    //Model = chatGPTSettings.apiModel,
                    //Messages = new ChatGPTMessage[]
                    //               {
                    //	new ChatGPTMessage
                    //                   {
                    //		Role = "user",
                    //		Content = prompt
                    //                   }
                    //               }
                }));

			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.disposeDownloadHandlerOnDispose = true;
			request.disposeUploadHandlerOnDispose = true;
			request.disposeCertificateHandlerOnDispose = true;

			request.SetRequestHeader("Content-Type", "application/json");
			//request.SetRequestHeader("Authorization", $"Bearer { chatGPTSettings.apiKey }");
			//request.SetRequestHeader("OpenAI-Organization", chatGPTSettings.apiOrganization);

			//var requestStartTime = DateTime.Now;

			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
			{
				Debug.Log(request.error);
			}
			else
			{
				string responseInfo = request.downloadHandler.text;
				var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseInfo);
				callBack(response);
			}
		}
	}
}
