using Assets.Scripts.Database.DAO;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.Game
{
    public class Playing_Manager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [Header("Show Video")]
        //public VideoPlayer VideoObj;
        public GameObject PanelVideo;

        [Header("Oclock")]
        public GameObject Oclock;

        [Header("BXH")]
        public GameObject BXH;

        [Header("Control Button")]
        public GameObject ControlBtn;
        public GameObject NextBtn;

        public static Playing_Manager Instance;

        Collection_Entity collection;

        List<Question_Entity> listQuestion;

        private int indexQuestion = 1;
        private int time;
        private int KeyAnswer;
        public bool isFinish = false;

        private void Awake()
        {
            Instance = this;
        }

        public void Init(int CollectionID)
        {
            collection = Collection_DAO.GetbyID(CollectionID);

            listQuestion = Question_DAO.GetAllbyCollectionID(CollectionID);

            ControlBtn.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                ControlBtn.SetActive(true);
            }
        }

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RaiseEvent(EventCode.Play_StartEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
        }

        #region Show message
        public void ShowMessage()
        {
            ManagerPlayingUI.Instance.StartMessageEvent();
        }

        public void EndMessage()
        {
            if (collection.LinkVideo != null)
            {
                ShowVideo(collection.LinkVideo);
            }
        }
        #endregion

        #region Control Video 

        public void ShowVideo(string LinkVideo)
        {
            PanelVideo.GetComponent<PanelVideo>().show(LinkVideo);
        }

        public void CloseVideo()
        {
            Debug.Log("CloseVideo()");
            ControlQuestion();
        }
        #endregion

        #region Control question

        public void ControlQuestion()
        {
            isFinish = (indexQuestion >= listQuestion.Count);
            if (indexQuestion <= listQuestion.Count)
            {
                ManagerPlayingUI.Instance.QuestionUI.GetComponent<QuestionPanel>()
                    .SetupQuestion(indexQuestion, listQuestion[indexQuestion - 1]);
                this.time = listQuestion[indexQuestion - 1].Time;
                this.KeyAnswer = listQuestion[indexQuestion - 1].correctAnswerIndex;
                Debug.Log("correctAnswerIndex " + listQuestion[indexQuestion - 1].correctAnswerIndex);
                Debug.Log("Time " + listQuestion[indexQuestion - 1].Time);
                ShowQuestion();
            }
            else
            {
                Debug.Log("End ControlQuestion");
            }
        }

        public void ShowQuestion()
        {
            ManagerPlayingUI.Instance.StartQuestionEvent();
        }

        public void EndQuestion()
        {
            ShowTimeAnswer();
        }

        public void ShowTimeAnswer()
        {
            Debug.Log("Waiting countdown time");
            GameManager.Instance.ResetRound();
            Oclock.GetComponent<Oclock>().run(this.time);

        }
        public void EndTimeAnswer()
        {
            Debug.Log("End Time Answer");
            if (PhotonNetwork.IsMasterClient == false)
            {
                ShowResult();
            }
            else
            {
                ShowListResult();
            }
        }

        public void ShowResult()
        {
            if (PhotonNetwork.IsMasterClient == false)
            {
                if (References.SelectedAnswer == KeyAnswer)
                {
                    GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
                        .PlayerAllUIInstance.GetComponent<Player_AllUI>().
                        StartPopupResult(true, References.TimeAnswer * 10);
                }
                else
                {
                    GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
                        .PlayerAllUIInstance.GetComponent<Player_AllUI>().
                        StartPopupResult(false, References.TimeAnswer * 0);
                }
            }
        }

        public void ShowListResult()
        {
            GameManager.Instance.Ranking_Sort();
            BXH.GetComponent<Panel_setting>().fadeIn();

            if (PhotonNetwork.IsMasterClient == true)
            {
                StartCoroutine(Waiting5f());
            }
        }

        IEnumerator Waiting5f()
        {
            yield return new WaitForSeconds(5f);
            NextBtn.SetActive(true);
        }

        //public void EndListResult()
        //{
        //    if (indexQuestion < listQuestion.Count)
        //    {
        //        BXH.GetComponent<Panel_setting>().fadeOut();
        //        NextQuestion();
        //    }
        //}

        public void NextQuestion()
        {
            if (indexQuestion < listQuestion.Count)
            {
                NextBtn.SetActive(false);
                BXH.GetComponent<Panel_setting>().fadeOut();
                ++indexQuestion;
                ControlQuestion();
            }
        }

        public void ShowBXH()
        {
            ManagerPlayingUI.Instance.ShowPanelBXH();
        }

        public void EndBXH()
        {
            Debug.Log("End Collection");
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == EventCode.Play_StartEventCode)
            {
                Init(9);
                ShowMessage();
                Debug.Log("Ok nha");
            }

        }

        #endregion
    }
}
