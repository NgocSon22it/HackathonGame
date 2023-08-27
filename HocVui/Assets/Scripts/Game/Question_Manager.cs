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

public class Question_Manager : MonoBehaviour
{
    [Header("Question Information")]
    [SerializeField] TMP_Text Information_Question;
    [SerializeField] List<TMP_Text> Information_Answer;
    [SerializeField] List<Image> Information_IsCorrect;
    [SerializeField] List<Image> Information_IsCorrectColor;

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



    [Header("Error Message")]
    [SerializeField] GameObject Message_Panel;
    [SerializeField] TMP_Text Message_Text;

    [Header("Paging")]
    [SerializeField] TMP_Text CurrentPaging;
    int CurrentIndex;

    public static Question_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateQuestion()
    {
        if (IsValidQuestion())
        {

            listAnswer = new List<string>();

            foreach (var a in ListAnswer_Input)
            {
                listAnswer.Add(a.text);
            }

            currentQuestion = new Question_Entity(QuestionText_Input.text, listAnswer, CorrectAnswer);

            References.ListQuestionCreate.Add(currentQuestion);

            CurrentIndex = References.ListQuestionCreate.Count - 1;

            Information_LoadCurrentQuestion(CurrentIndex);

            CreateQuestion_Close();
        }
        else
        {
            Message_Panel.SetActive(true);
        }
    }

    public bool IsValidQuestion()
    {
        if (string.IsNullOrEmpty(QuestionText_Input.text))
        {
            Message_Text.text = "Câu hỏi không được để trống!";
            return false;
        }

        foreach (var item in ListAnswer_Input)
        {
            if (string.IsNullOrEmpty(item.text))
            {
                Message_Text.text = "Đáp án không được để trống!";
                return false;
            }
        }

        if (IsUploadImage && string.IsNullOrEmpty(ImageName))
        {
            Message_Text.text = "Hãy chọn hình ảnh!";
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


    public void Message_Close()
    {
        Message_Panel.SetActive(false);
    }

    public void ResetData_CreateQuestion()
    {
        CorrectAnswer = 0;
        CreateQuestion_Toggle[0].isOn = true;

        IsUploadImage = false;
        CreateQuestion_ToggleUploadImage.isOn = false;

        QuestionText_Input.text = "";

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
        References.SelectCollection = new Collection_Entity("Son", References.ListQuestionCreate, "Video.mp4"); 
        Debug.Log("Tạo Bộ câu hỏi thành công");
        Debug.Log(References.SelectCollection.ListQuestion.Count);

    }

    public void SelectVideoUpload()
    {
        // Use Unity's cross-platform file picker
        string selectedFilePath = EditorUtility.OpenFilePanel("Select a Video File", "", "mp4");

        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            Debug.Log(selectedFilePath);

            UploadVideo(selectedFilePath);
        }
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

    public void UploadVideo(string path)
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Create a reference to the image in the storage bucket
        StorageReference imageRef = storageRef.Child("video").Child("abc.mp4");

        // Upload the image
        imageRef.PutFileAsync(path).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Video upload failed.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Video upload successful.");
            }
        });
    }

    public void DownloadVideo()
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Reference to the remote video in the storage bucket
        StorageReference videoRef = storageRef.Child("video").Child("abc.mp4");

        // Download the video to local storage
        videoRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                string videoUrl = task.Result.ToString();

                Debug.Log("Success to get video download URL." + videoUrl);
            }
            else
            {
                Debug.LogError("Failed to get video download URL.");
            }
        });
    }

}
