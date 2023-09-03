using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Scripts.Game
{
    public class ViewCollection : MonoBehaviour
    {
        [Header("Video")]
        public GameObject VideoObj;
        public VideoPlayer VideoPlayer;
        public Button VideoBtn;
        bool isVideo = true;

        [Header("Image")]
        public GameObject ImageObj;
        public RawImage rawImage;
        public Texture texture;


        [Header("Question")]
        public TMP_Text Question;
        public List<TMP_Text> ListAnswer;
        int index = 0;

        [Header("Collection")]
        public TMP_Text CollectionName;

        [Header("Paging")]
        public TMP_Text Paging;
        public GameObject NextBtn, PrevBtn;

        public Collection_Entity Collection;
        public List<Question_Entity> ListQuestion;

        public static ViewCollection Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowInfo()
        {
            NextBtn.SetActive(false); PrevBtn.SetActive(false);
            if (Collection != null)
            {
                CollectionName.text = Collection.Name;
                Paging.text = (index + 1).ToString() + "/" + (ListQuestion.Count).ToString();
                ShowVideo();
                ShowQuestion();
                NextBtn.SetActive(true); PrevBtn.SetActive(true);
            }

        }

        public void ShowVideo()
        {
            if (!string.IsNullOrEmpty(Collection.LinkVideo))
            {
                VideoBtn.interactable = true;
                isVideo = false;
                VideoPlayer.gameObject.SetActive(true);
                VideoPlayer.source = VideoSource.Url;
                VideoPlayer.url = Collection.LinkVideo;
                VideoPlayer.Prepare();
                VideoPlayer.Pause();
            }
            else
            {
                VideoBtn.interactable = false;
            }
        }

        public void ControlVideoBtn()
        {
            if (!isVideo)
            {
                isVideo = true;
                VideoObj.SetActive(true);
                ImageObj.SetActive(false);
                VideoPlayer.Pause();
                VideoBtn.GetComponentInChildren<TMP_Text>().text = "Phát";
            }
            else
            {
                if (VideoPlayer.isPlaying)
                {
                    VideoPlayer.Pause();
                    VideoBtn.GetComponentInChildren<TMP_Text>().text = "Phát";
                }
                else
                {
                    VideoPlayer.Play();
                    VideoBtn.GetComponentInChildren<TMP_Text>().text = "Dừng";
                }
            }
        }

        public void ControlImageBtn()
        {
            isVideo = false;
            VideoBtn.GetComponentInChildren<TMP_Text>().text = "Video";
            VideoObj.SetActive(false);
            ImageObj.SetActive(true);

            rawImage.texture = ListQuestion[index].rawImage;
        }


        public void ShowQuestion()
        {
            if (index <= ListQuestion.Count)
            {
                Question.text = ListQuestion[index].questionText;
                for (var i = 0; i < ListAnswer.Count; ++i)
                {
                    ListAnswer[i].text = ListQuestion[index].answers[i];
                }

                rawImage.gameObject.SetActive(false);

                if (!string.IsNullOrEmpty(ListQuestion[index].LinkImage))
                {
                    rawImage.gameObject.SetActive(true);
                    if (ListQuestion[index].rawImage == null) StartCoroutine(LoadImage(ListQuestion[index].LinkImage));
                    else rawImage.texture = ListQuestion[index].rawImage;
                }
            }
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
                ListQuestion[index].rawImage = ((DownloadHandlerTexture)request.downloadHandler).texture;
                rawImage.texture = ListQuestion[index].rawImage;
                // setting the loaded image to our object
            }
        }


        public void Next()
        {
            if (ListQuestion.Count > 0)
            {
                ++index;
                if (index >= ListQuestion.Count)
                {
                    index = 0;
                }
                Paging.text = (index + 1).ToString() + "/" + (ListQuestion.Count).ToString();
                rawImage.texture = ListQuestion[index].rawImage;
                ShowQuestion();
            }
        }


        public void Previous()
        {
            if (ListQuestion.Count > 0)
            {
                --index;
                if (index < 0)
                {
                    index = ListQuestion.Count - 1;
                }
                Paging.text = (index + 1).ToString() + "/" + (ListQuestion.Count).ToString();
                rawImage.texture = ListQuestion[index].rawImage;
                ShowQuestion();
            }
        }
    }
}
