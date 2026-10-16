using Amazon;
using System.IO;
using UnityEngine;
using Amazon.Polly;
using Amazon.Runtime;
using Amazon.Polly.Model;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;

public class Text2Speech : MonoBehaviour {
    private AudioSource audioSource;
    private Animator animator;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        // Test the talk motion
        // TestSaySomething();
    }

    private async void TestSaySomething() {
        string audioPath;
#if UNITY_ANDROID && !UNITY_EDITOR
        audioPath = $"jar:file://{Application.persistentDataPath}/audio.mp3";
#elif (UNITY_IOS || UNITY_OSX) && !UNITY_EDITOR
        audioPath = $"file://{Application.persistentDataPath}/audio.mp3";
#else
        audioPath = $"{Application.persistentDataPath}/audio.mp3";
#endif
        using (var www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG)) {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            audioSource.Play();

            animator.SetBool("Talk", true);
            while (audioSource.isPlaying) await Task.Yield();
            animator.SetBool("Talk", false);
        }
    }

    public void PauseTalking()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            animator.SetBool("Talk", false);
        }
    }

    public async void MakeAudioRequest(string message) {
        var credentials = new BasicAWSCredentials(System.Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), System.Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
        var client = new AmazonPollyClient(credentials, RegionEndpoint.APNortheast1);

        var request = new SynthesizeSpeechRequest() {
            Text = message,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Ayanda,
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);
        Debug.Log("Response done.");

        WriteIntoFile(response.AudioStream);

        string audioPath;

#if UNITY_ANDROID && !UNITY_EDITOR
        audioPath = $"jar:file://{Application.persistentDataPath}/audio.mp3";
#elif (UNITY_IOS || UNITY_OSX) && !UNITY_EDITOR
        audioPath = $"file://{Application.persistentDataPath}/audio.mp3";
#else
        audioPath = $"{Application.persistentDataPath}/audio.mp3";
#endif

        using (var www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG)) {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            audioSource.Play();

            animator.SetBool("Talk", true);
            while (audioSource.isPlaying) await Task.Yield();
            animator.SetBool("Talk", false);
        }
    }

    private void WriteIntoFile(Stream stream) {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create)) {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
