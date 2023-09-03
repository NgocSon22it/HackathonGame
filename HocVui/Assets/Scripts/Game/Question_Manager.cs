using Firebase.Storage;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using System.Linq;
using UnityEngine.Video;
using UnityEngine.Networking;
using Assets.Scripts.Database.Entity;
using Photon.Realtime;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;

public class Question_Manager : MonoBehaviour
{
    [Header("Collection Info")]
    public TMP_InputField Collection_Name;
    public TMP_Text MessageName;
    public string VideoUrl, CollectionName;
    public int CollectionID;

    [Header("Upload Video")]
    public VideoPlayer VideoObj;
    public GameObject UploadVideoIcon;
    public Button VideoPlayBtn;
    public GameObject LoadingVideo;


    [Header("Upload Image")]
    public TMP_Text NameImage;
    public Button AddBtn;
    public GameObject UploadImageIcon;
    public GameObject LoadingImage;

    [Header("Question Information")]
    [SerializeField] TMP_Text Information_Question;
    [SerializeField] List<TMP_Text> Information_Answer;
    [SerializeField] List<Image> Information_IsCorrect;
    [SerializeField] List<Image> Information_IsCorrectColor;
    public RawImage rawImage;
    public Texture texture;
    public string LinkImage;

    [Header("Create Question")]
    [SerializeField] GameObject CreateQuestion_Panel;
    [SerializeField] Toggle CreateQuestion_ToggleUploadImage;
    [SerializeField] GameObject CreateQuestion_UploadImagePanel;
    [SerializeField] TMP_InputField QuestionText_Input;
    [SerializeField] List<TMP_InputField> ListAnswer_Input;
    [SerializeField] List<Toggle> CreateQuestion_Toggle;

    bool IsUploadImage;
    int CorrectAnswer;
    List<string> listAnswer;
    Question_Entity currentQuestion;
    string ImageName;


    [Header("Paging")]
    [SerializeField] TMP_Text CurrentPaging;
    int CurrentIndex;

    public static Question_Manager Instance;


    private void Awake()
    {
        Instance = this;
    }

    #region Setup question
    public void CreateQuestion()
    {
        UploadImageIcon.SetActive(true);
        if (IsValidQuestion())
        {

            listAnswer = new List<string>();

            foreach (var a in ListAnswer_Input)
            {
                listAnswer.Add(a.text);
            }
            rawImage.texture = texture;

            UploadImageIcon.SetActive(false);

            currentQuestion = new Question_Entity(QuestionText_Input.text, listAnswer, CorrectAnswer, rawImage.texture, LinkImage);

            References.ListQuestionCreate.Add(currentQuestion);

            CurrentIndex = References.ListQuestionCreate.Count - 1;

            Information_LoadCurrentQuestion(CurrentIndex);

            CreateQuestion_Close();

        }
       
    }

    public bool IsValidQuestion()
    {
        if (string.IsNullOrEmpty(QuestionText_Input.text))
        {
            //Message_Text.text = "Câu hỏi không được để trống!";
            Lobby_Manager.Instance.MessagePanel_On("Câu hỏi không được để trống!");

            return false;
        }

        foreach (var item in ListAnswer_Input)
        {
            if (string.IsNullOrEmpty(item.text))
            {
                //Message_Text.text = "Đáp án không được để trống!";
                Lobby_Manager.Instance.MessagePanel_On("Đáp án không được để trống!");

                return false;
            }
        }

        if (IsUploadImage && string.IsNullOrEmpty(ImageName))
        {
            //Message_Text.text = "Hãy chọn hình ảnh!";
            Lobby_Manager.Instance.MessagePanel_On("Hãy chọn hình ảnh!");
            return false;
        }

        return true;
    }

    public void Information_LoadCurrentQuestion(int Index)
    {
        Information_Question.text = (Index + 1) + ". " + References.ListQuestionCreate[Index].questionText;

        for (int i = 0; i < 4; i++)
        {
            Information_Answer[i].text = References.ListQuestionCreate[Index].answers[i];
        }

        foreach (var item in Information_IsCorrect)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in Information_IsCorrectColor)
        {
            item.color = Color.white;
        }

        rawImage.texture = References.ListQuestionCreate[Index].rawImage;

        if(rawImage.texture != null) UploadImageIcon.SetActive(false);
        else UploadImageIcon.SetActive(true);

        Information_IsCorrect[References.ListQuestionCreate[Index].correctAnswerIndex].gameObject.SetActive(true);

        Information_IsCorrectColor[References.ListQuestionCreate[Index].correctAnswerIndex].color = Color.yellow;

        CurrentPaging.text = (CurrentIndex + 1) + " / " + (References.ListQuestionCreate.Count);
    }

    public void CreateQuestion_SelectCorrectAnswer(int Index)
    {
        CorrectAnswer = Index;
    }

    public void CreateQuestion_Open()
    {
        CreateQuestion_Panel.SetActive(true);
    }

    public void CreateQuestion_Close()
    {
        CreateQuestion_Panel.SetActive(false);
    }

    public void ResetData_CreateQuestion()
    {
        CorrectAnswer = 0;
        CreateQuestion_Toggle[0].isOn = true;

        IsUploadImage = false;
        CreateQuestion_ToggleUploadImage.isOn = false;

        QuestionText_Input.text = "";
        rawImage.texture = null;
        texture = null;
        LinkImage = string.Empty;
        NameImage.text = "Bạn chưa chọn hình";
        UploadImageIcon.SetActive(true);

        foreach (var item in ListAnswer_Input)
        {
            item.text = "";
        }
    }
    public void Next()
    {
        if (References.ListQuestionCreate.Count > 0)
        {
            CurrentIndex++;
            if (CurrentIndex >= References.ListQuestionCreate.Count)
            {
                CurrentIndex = 0;
            }

            Information_LoadCurrentQuestion(CurrentIndex);
        }
    }


    public void Previous()
    {
        if (References.ListQuestionCreate.Count > 0)
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
            {
                CurrentIndex = References.ListQuestionCreate.Count - 1;
            }

            Information_LoadCurrentQuestion(CurrentIndex);
        }
    }

    public void CreateQuestion_IsUploadImage(bool value)
    {
        CreateQuestion_UploadImagePanel.SetActive(value);
        IsUploadImage = value;
    }

    public void Information_DeleteQuestion()
    {
        if (References.ListQuestionCreate.Count > 0)
        {
            Question_Entity a = References.ListQuestionCreate[CurrentIndex];
            References.ListQuestionCreate.Remove(a);

            if (References.ListQuestionCreate.Count > 0)
            {
                Next();
            }
            else
            {
                Information_Question.text = "";

                for (int i = 0; i < 4; i++)
                {
                    Information_Answer[i].text = "";
                }

                foreach (var item in Information_IsCorrect)
                {
                    item.gameObject.SetActive(false);
                }

                foreach (var item in Information_IsCorrectColor)
                {
                    item.color = Color.white;
                }

                CurrentPaging.text = "0 / 0";
            }
        }
    }

    public void CreateCollection()
    {
        if (References.ListQuestionCreate.Count > 0)
        {
            Collection_DAO.SaveCollection(References.account.Id, CollectionName, References.ListQuestionCreate.Count, VideoUrl);
            var ID = Collection_DAO.GetCollectionID(References.account.Id, CollectionName);

            Question_DAO.SaveQuestion(ID, References.ListQuestionCreate);

            Debug.Log("Tạo Bộ câu hỏi thành công");

            SceneManager.LoadScene(Scenes.Lobby);
        }
        else
        {
            Debug.Log("Bạn cần tạo câu hỏi trước khi lưu");
            //Message_Text.text = "Bạn cần tạo câu hỏi trước khi lưu";
            Lobby_Manager.Instance.MessagePanel_On("Bạn cần tạo câu hỏi trước khi lưu");
            //Message_Panel.SetActive(true);
        }
    }

    public bool IsValidCollection()
    {
        var name = Collection_Name.text;
        if (string.IsNullOrEmpty(name))
        {
            MessageName.text = "Hãy nhập tên bộ câu hỏi!";
            //Lobby_Manager.Instance.MessagePanel_On("Hãy nhập tên bộ câu hỏi!");
            return false;
        }

        var ID = Collection_DAO.GetCollectionID(References.account.Id, name);
        if (ID != -1)
        {
            MessageName.text = "Tên này đã tồn tại!";
            //Lobby_Manager.Instance.MessagePanel_On("Tên này đã tồn tại!");

            return false;
        }
        return true;
    }

    #endregion

    #region Upload Video
    public void SelectVideoUpload()
    {
        /*//Use Unity's cross-platform file picker
        var FilePath = EditorUtility.OpenFilePanel("Select a Video File", "", "mp4");

        if (!string.IsNullOrEmpty(FilePath))
        {
            Debug.Log(FilePath);

            var nameFile = FilePath.Split("/");
            var FileName = nameFile[nameFile.Count() - 1];
            UploadVideo(FilePath, FileName);
            LoadingVideo.GetComponent<PanelLoader>().StartLoader();
        }*/
    }

    public void DeleteVideo()
    {
        VideoObj.url = string.Empty;
        VideoObj.gameObject.SetActive(true);
        UploadVideoIcon.SetActive(false);
        VideoPlayBtn.gameObject.SetActive(false);
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            // Initialize Firebase Storage
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        });
    }

    public void UploadVideo(string FilePath, string FileName)
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Create a reference to the image in the storage bucket
        StorageReference imageRef = storageRef.Child("video").Child(References.account.Username).Child(FileName);

        // Upload the image
        imageRef.PutFileAsync(FilePath).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Video upload failed.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Video upload successful.");
                DownloadVideo(FileName);
            }
        });
    }

    public void DownloadVideo(string FileName)
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Reference to the remote video in the storage bucket
        StorageReference videoRef = storageRef.Child("video").Child(References.account.Username).Child(FileName);

        // Download the video to local storage
        videoRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {

                string videoUrl = task.Result.ToString();
                Debug.Log("Success to get video download URL." + videoUrl);
                VideoObj.source = VideoSource.Url;
                VideoObj.url = videoUrl;
                VideoObj.gameObject.SetActive(true);
                VideoObj.Prepare();
                VideoObj.Pause();
                
                VideoPlayBtn.gameObject.SetActive(true);
                UploadVideoIcon.SetActive(false);
                LoadingVideo.GetComponent<PanelLoader>().StopLoader();
            }
            else
            {
                Debug.LogError("Failed to get video download URL.");
            }
        });
    }

    public void ControlVideo()
    {
        if (VideoObj.isPlaying)
        {
            VideoObj.Pause();
            VideoPlayBtn.GetComponentInChildren<TMP_Text>().text = "Phát";
        }
        else
        {
            VideoObj.Play();
            VideoPlayBtn.GetComponentInChildren<TMP_Text>().text = "Dừng";
        }
    }

    public void StoreVideoInfo()
    {
        VideoObj.Pause();

        if (IsValidCollection())
        {
            CollectionName = Collection_Name.text;
            VideoUrl = VideoObj.url;
            MessageName.text = "";
            Collection_UI.Instances.fadeLeft();
        }
    }

    #endregion

    #region Upload Image

    public void SelectImageUpload()
    {
        // Use Unity's cross-platform file picker
      /* var FilePath = EditorUtility.OpenFilePanel("Select a Video File", "", "png,jpg,jpeg,gif,svg");

        if (!string.IsNullOrEmpty(FilePath))
        {
            Debug.Log(FilePath);

            var nameFile = FilePath.Split("/");
            var FileName = nameFile[nameFile.Count() - 1];
            ImageName = FileName;

            Debug.Log(ImageName);

            UploadImage(FilePath, FileName);
            LoadingImage.GetComponent<PanelLoader>().StartLoader();
        }*/
    }
    public void UploadImage(string FilePath, string FileName)
    {
        AddBtn.interactable = false;
        NameImage.text = FileName;
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Create a reference to the image in the storage bucket
        StorageReference imageRef = storageRef.Child("images").Child(References.account.Username).Child(FileName);
        // Upload the image
        imageRef.PutFileAsync(FilePath).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Image upload failed.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Image upload successful.");
                DownloadImage(FileName);
                AddBtn.interactable = true;
            }
        });
    }

    public void DownloadImage(string FileName)
    {
        //initialize storage reference
        var storage = FirebaseStorage.DefaultInstance;
        var storageReference = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        //get reference of image
        StorageReference image = storageReference.Child("images").Child(References.account.Username).Child(FileName);

        //Get the download link of file
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {

                Debug.Log("Image DownloadImage successful.");
                LinkImage = Convert.ToString(task.Result);
                StartCoroutine(LoadImage(LinkImage)); //Fetch file from the link
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    IEnumerator LoadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl); //Create a request
        yield return request.SendWebRequest(); //Wait for the request to complete
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("down success ");
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            LoadingImage.GetComponent<PanelLoader>().StopLoader();
            // setting the loaded image to our object
        }
    }
    #endregion
}
