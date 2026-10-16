using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenAI;

public class ChatGPTTester : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] private Button askButton;
    [SerializeField] private InputField inputField;
    //[SerializeField] private TextMeshProUGUI chatGPTAnswer;

    [SerializeField] private string prompt;

    [SerializeField] private ScrollRect scroll;

    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;

    public bool Voice = false;
    [SerializeField] private Text2Speech textToSpeech;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject DynamicCanvas;
    [SerializeField] private RectTransform InfoDisplay;
    [SerializeField] private RectTransform Info;

    [SerializeField] private GameObject VirtualMirror;


    private float height;

    private List<ChatMessage> messages = new List<ChatMessage>();

    public CameraController cameraController;

    public GameObject Model;

    public Minimap minimap;

    public GameObject userAvatar;

    private void Start()
    {
        //ShowChatbox();
        HideChatbox();
        caseid = 0;
    }

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AppendMessage("I really interested in chinese art.", "user");
            AppendMessage("That's great! Chinese art has a rich history and is known for its unique styles and techniques.\n In our virtual museum, we have several Chinese paintings that you might find interesting. Here are a few of them:\r\n\r\n1. \"Section of Goddess of Luo River\": This painting depicts a beautiful landscape with mountains, rivers, and trees. It showcases the traditional Chinese ink painting style.\r\n\r\n2. \"Travelers among Mountains and Streams\": This painting portrays a group of travelers navigating through a mountainous landscape. It is a classic example of Chinese landscape painting.\r\n\r\nThese are just a few examples of the Chinese art we have in our museum. If you would like to explore more, I can guide you to these paintings in the virtual space.", "assistant");
            //DisplayInfo("To determine the three most popular paintings in the museum, I would need access to the popularity data of each painting. Unfortunately, the popularity information is not provided in the given data. However, I can provide you with a list of the top three most famous paintings in general:\r\n\r\n1. \"Mona Lisa\" by Leonardo da Vinci\r\n2. \"The Last Supper\" by Leonardo da Vinci\r\n3. \"The Scream\" by Edvard Munch\r\n\r\nThese paintings are widely recognized and highly regarded in the art world.", "");
            //DisplayInfo("Welcome to the virtual museum! This museum is home to a diverse collection of paintings from various artists and periods. The museum aims to provide visitors with an immersive and educational experience.\r\n\r\nThe museum features a wide range of artworks, including famous masterpieces such as the \"Mona Lisa\" by Leonardo da Vinci");
        }
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    HightlightDetails("painting 015");
        //}

        //// X button: chat button
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    // Trigger the click event of the tour button
        //    ChatboxButton.GetComponent<Button>().onClick.Invoke();
        //}

        //// A button: tour button
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    // Trigger the click event of the tour button
        //    TourButton.GetComponent<Button>().onClick.Invoke();
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    var text = new List<string> { "painting 013", "painting 000", "painting 001" };
        //    VirtualMirroring(text);
        //}
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Execute(case1[caseid]);
            caseid += 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Execute(case2[caseid]);
            caseid += 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Execute(case3[caseid]);
            caseid += 1;
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //var text = new List<string> { "painting 016", "painting 017", "painting 018", "painting 019"};
            //VirtualMirroring(text);
            Execute(case4[caseid]);
            caseid += 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            DisplayInfo("Recommendation for the tour:\r\n- Section of Goddess of Luo River\r\n- Travelers among Mountains and Streams\r\n- A Man and His Horse in the Wind\r\n- Forest Grotto at Juqu\r\n- Shen Zhou self portrait at age 80\r\n", "");
        }
    }
    private int caseid = 0;
    private string[] case1 = new string[] {
        "Please help me plan a tour for this museum in 30 minutes",
        "Take me to visit Impression, Sunrise",
        "Take me to visit The Scream",
        "Take me to visit Mona Lisa",
        "Summarize the tour and suggest the next painting"
    };
    private string[] case2 = new string[] {
        "Take me to visit the painting named The Birth of Venus",
        "Please introduce this painting to me",
        "What are the interesting details in this painting",
        "Who is the people in the middle of this painting",
        "is there any other paintings of the similar style to this painting in this museum?"
    };
    private string[] case3 = new string[] {
        "I really like Chinese paintings",
        "GIve me some recommendations for Chinese paintings in this museum",
        "Introduce the second painting you just recommended Travelers among Mountains and Streams",
        "Take me to visit this painting Travelers among Mountains and Streams",
        "Take me to the closet chinese painting Forest Grotto at Juqu"
    };
    private string[] case4 = new string[] {
        //"What is the weather today?",
        //"I really like abstract paintings",
        //"How many french paintings in this museum?",
        //"List some French paintings",
        "Take me to see the painting Composition No.10",
        "Introduce this painting"
    };

    public void Execute(string input = "")
    {
        input = case3[caseid];
        caseid += 1;

        Debug.Log("Send the message");

        DeleteChildren(InfoDisplay);

        if (string.IsNullOrEmpty(input))
        {
            prompt = inputField.text;
        }
        else
        {
            prompt = input;
        }

        if (prompt == null || prompt.Length <= 0 || prompt == "you")
        {
            AppendMessage("Record Nothing! Please input your question again!", "assistant");
        }
        else
        {
            AppendMessage(prompt, "user");

            askButton.enabled = false;
            inputField.text = "";
            inputField.enabled = false;

            // Transform the main camera position into the local transform of the scene gameobject
            string currentPosition = Model.transform.InverseTransformPoint(Camera.main.transform.position).ToString();
            string landmark = cameraController.GetCurrentLandmark();
            List<string> tourHistory = cameraController.GetTourHistory();
            StartCoroutine(ChatGPTClient.Instance.Ask(prompt, currentPosition, landmark, tourHistory, (r) => ProcessResponse(r)));
        }
    }

    [SerializeField] private GameObject Chatbox;
    [SerializeField] private GameObject ChatboxButton;
    [SerializeField] private GameObject TourButton;
    private bool isChatboxShow = false;
    public void HideChatbox()
    {
        if (Chatbox != null)
        {
            Chatbox.SetActive(false);

            //ChatboxButton.SetActive(true);
            //Vector3 currentPosition = ChatboxButton.transform.position;
            //currentPosition.y = TourButton.transform.position.y;
            //ChatboxButton.transform.position = currentPosition;

            isChatboxShow = false;
        }
    }

    public void ShowChatbox()
    {
        if (Chatbox != null)
        {
            ResetCanvasPos();
            Chatbox.SetActive(true);

            //ChatboxButton.SetActive(false);
            //Vector3 currentPosition = ChatboxButton.transform.position;
            //currentPosition.y = Chatbox.transform.position.y + Chatbox.GetComponent<RectTransform>().sizeDelta.y;
            //ChatboxButton.transform.position = currentPosition;

            isChatboxShow = true;
        }
    }

    public void ChatboxButtonClicked()
    {
        if (isChatboxShow)
        {
            HideChatbox();
        }
        else
        {
            ShowChatbox();
        }
    }

    public void ProcessResponse(ChatGPTResponse response)
    {
        //var chatGPTContent = response.Choices.FirstOrDefault()?.Message?.Content;
        var chatGPTContent = response.Content;
        var ResponseTasks = response.Tasks;
        var ResponseContext = response.Context;
        var ResponseLandmark = response.Landmark;
        var ResponseTourIDs = response.TourIDs;

        if (!string.IsNullOrEmpty(chatGPTContent))
        {
            AppendMessage(chatGPTContent, "assistant");
            Debug.Log("Response in voice...");
            HideChatbox();

            if (ResponseTasks.Contains("navigation"))
            {
                DeleteAllChildren(VirtualMirror);
                cameraController.ProcessChatGPTResponse(chatGPTContent, Voice, textToSpeech);
            }
            else
            {
                if (Voice) textToSpeech.MakeAudioRequest(chatGPTContent);

                minimap.HideMinimap();

                if (ResponseTasks.Contains("information enhancement"))
                {
                    DisplayInfo(ResponseContext, ResponseLandmark);
                    VirtualMirroring(ResponseTourIDs);
                    //var text = new List<string> { "painting 000", "painting 001" };
                    //VirtualMirroring(text);
                    if (caseid == 2)
                    {
                        var text = new List<string> { "painting 008", "painting 009", "painting 010", "painting 011", "painting 012" };
                        //var text = new List<string> { "painting 000" };
                        VirtualMirroring(text);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No text was generated from this prompt.");
        }

        askButton.enabled = true;
        inputField.enabled = true;
    }

    private void AppendMessage(string prompt, string role)
    {
        scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        var item = Instantiate(role == "user" ? sent : received, scroll.content);
        item.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = cameraController.ResponseJsonOrNot(prompt); ;
        item.anchoredPosition = new Vector2(0, -height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        height += item.sizeDelta.y;
        scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        scroll.verticalNormalizedPosition = 0;
    }

    private void DisplayInfo(string prompt, string landmark)
    {
        string landmarkID = cameraController.GetCurrentLandmark();

        var item = Instantiate(Info, InfoDisplay);
        if (landmark.Length > 0)
        {
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = landmark;
            HightlightDetails(landmarkID);
        }
        else
        {
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = "More Information";
        }
        item.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = prompt;
        LayoutRebuilder.ForceRebuildLayoutImmediate(item);

        // position the information at the center of the info display canvas
        float topY = InfoDisplay.rect.height / 2 - item.rect.height / 2;
        item.anchoredPosition = new Vector2(0f, -topY);

        ResetHighlightCanvasPos();
    }

    public float distanceFromCamera = 30.0f;
    public float angleFromCamera = 0.0f;
    private void ResetHighlightCanvasPos()
    {
        // reset the position of the canvas of info display
        Vector3 rightOffset = Quaternion.Euler(0, angleFromCamera, 0) * userAvatar.transform.forward;
        Vector3 newPosition = mainCamera.transform.position + (rightOffset.normalized * distanceFromCamera);
        DynamicCanvas.transform.position = newPosition;
        DynamicCanvas.transform.rotation = Quaternion.LookRotation(DynamicCanvas.transform.position - mainCamera.transform.position);
    }

    private void ResetCanvasPos()
    {
        // reset the position of the canvas of chatbox
        Vector3 newPosition = mainCamera.transform.position + userAvatar.transform.forward * distanceFromCamera;
        Canvas.transform.position = newPosition;
        Canvas.transform.rotation = Quaternion.LookRotation(Canvas.transform.position - mainCamera.transform.position);
    }

    [SerializeField] private GameObject highlightPaintings;
    //[SerializeField] private RectTransform HighlightDisplay;
    //[SerializeField] private RectTransform HighlightRect;
    private void HightlightDetails(string landmark)
    {
        string highlightNameToActivate = landmark.Replace("painting", "Highlight");
        Transform highlightTransform = highlightPaintings.transform.Find(highlightNameToActivate);

        if (highlightTransform != null)
        {
            GameObject specificHighlight = highlightTransform.gameObject;
            specificHighlight.SetActive(true);
            //if (caseid == 3 || caseid == 4) specificHighlight.SetActive(true);
            //if (caseid == 3)
            //{
            //    highlightTransform.GetChild(1).gameObject.SetActive(true);
            //    highlightTransform.GetChild(2).gameObject.SetActive(true);
            //}
            //if (caseid == 4)
            //{
            //    highlightTransform.GetChild(1).gameObject.SetActive(false);
            //    highlightTransform.GetChild(2).gameObject.SetActive(false);
            //}
        }
        else
        {
            Debug.LogError("Highlight not found: " + highlightNameToActivate);
        }
    }

    private void DeleteChildren(RectTransform parentRectTransform)
    {
        // Check if the parent RectTransform is not null
        if (parentRectTransform != null)
        {
            // Loop through all children of the parent RectTransform
            for (int i = parentRectTransform.childCount - 1; i >= 0; i--)
            {
                // Destroy the child GameObject
                Destroy(parentRectTransform.GetChild(i).gameObject);
            }
        }
    }

    private void DeleteAllChildren(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // VitualMirror: Clone the corresponding object in front of the viewer
    private void VirtualMirroring(List<string> TourIDs)
    {
        DeleteAllChildren(VirtualMirror);

        // Step 2. Update position due to the painting numbers
        for (int i = 0; i < TourIDs.Count; i++)
        {
            string item = TourIDs[i];
            float offset = 1.5f * (float)i - (float)Math.Floor((double)TourIDs.Count / 2) - 0.5f;
            MirrorSingleItem(item, offset);
        }
    }

    private void MirrorSingleItem(string item, float offset)
    {

        string paintingObjectID = item.Replace("painting ", "placeholder.");
        GameObject objectToClone = GameObject.Find(paintingObjectID);

        if (objectToClone != null)
        {
            GameObject clonedObject = Instantiate(objectToClone);
            clonedObject.transform.parent = VirtualMirror.transform;

            // Position the cloned object in bottom front of the main camera
            //Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * 3f;
            Vector3 offsetVec = userAvatar.transform.right * offset; // Move left by 1.5 units
            Vector3 newPosition = mainCamera.transform.position + userAvatar.transform.forward * 3f + offsetVec;
            newPosition.y -= 1.5f;
            clonedObject.transform.position = newPosition;
            Quaternion newRotation = Quaternion.LookRotation(clonedObject.transform.position - mainCamera.transform.position);
            newRotation.eulerAngles = new Vector3(-90f, newRotation.eulerAngles.y + 90f, newRotation.eulerAngles.z);
            clonedObject.transform.rotation = newRotation;
            clonedObject.transform.localScale /= 3f;
        }
        else
        {
            Debug.LogWarning(paintingObjectID + " not found.");
        }
    }
}
