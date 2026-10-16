using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace OpenAI
{

    public class Speech2Text : MonoBehaviour
    {
        [SerializeField] private ChatGPTTester chatTest;
        [SerializeField] private InputActionReference bButton;

        private readonly string fileName = "output.wav";
        private readonly int duration = 10;
        private readonly int micro_ind = 1;

        private AudioClip clip;
        private OpenAIApi openai;

        private bool wasPressed = false;

        private async void SendMessage()
        {
            var apiKey = System.Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Debug.LogWarning("Set OPENAI_API_KEY to enable speech transcription.");
                return;
            }
            if (openai == null) openai = new OpenAIApi(apiKey);
            byte[] data = SaveWav.Save(fileName, clip);
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);
            Debug.Log(res.Text);

            chatTest.Execute(res.Text);
        }

        [SerializeField] private Text2Speech textToSpeech;
        // Update is called once per frame
        private void Update()
        {
            var isPressed = bButton.action.ReadValue<float>() > 0.1f;
            if (isPressed && !wasPressed)
            {
                // stop if the avatar is still talking
                textToSpeech.PauseTalking();

                Debug.Log("Start recording...");
                chatTest.ShowChatbox();
                //foreach (var d in Microphone.devices)
                //{
                //    Debug.Log(d.ToString());
                //}

                Debug.Log("Choose microphone: " + Microphone.devices[micro_ind]);
                clip = Microphone.Start(Microphone.devices[micro_ind], false, duration, 44100);
            }
            if (!isPressed && wasPressed)
            {
                Debug.Log("Stop recording...");

                Microphone.End(null);
                SendMessage();
            }
            wasPressed = isPressed;
        }
    }
}
