﻿using Assets.Scripts.Database.DAO;
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
    public class Playing_Manager : MonoBehaviour
    {
        [Header("Show Video")]
        //public VideoPlayer VideoObj;
        public GameObject PanelVideo;

        [Header("Oclock")]
        public GameObject Oclock;

        [Header("BXH")]
        public GameObject BXH;

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

        }

        public void StartGame()
        {
            Init(9);
            ShowMessage();

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
            ShowResult();
        }

        public void ShowResult()
        {

            if (References.SelectedAnswer == KeyAnswer)
            {
                Player_AllUI.Instance.StartPopupResult(true, References.TimeAnswer * 10);
            }
            else
            {
                Player_AllUI.Instance.StartPopupResult(false, References.TimeAnswer * 0);
            }
        }

        public void ShowListResult()
        {
            BXH.GetComponent<Panel_setting>().fadeIn();
        }

        public void EndListResult()
        {
            if (indexQuestion < listQuestion.Count)
            {
                BXH.GetComponent<Panel_setting>().fadeOut();
                NextQuestion();
            }
        }

        public void NextQuestion()
        {
            ++indexQuestion;
            ControlQuestion();
        }

        public void ShowBXH()
        {
            ManagerPlayingUI.Instance.ShowPanelBXH();
        }

        public void EndBXH()
        {
            Debug.Log("End Collection");
        }

        #endregion
    }
}
