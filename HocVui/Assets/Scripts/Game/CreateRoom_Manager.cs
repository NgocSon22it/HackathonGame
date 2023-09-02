using Assets.Scripts.Database.DAO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Scripts.Game
{
    public class CreateRoom_Manager : MonoBehaviour
    {
        [Header("Setup")]
        public GameObject ItemPrefabs;
        public Transform Content;

        public static CreateRoom_Manager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Open()
        {
            var list = Collection_DAO.GetAllbyUserID(References.account.Id);

            foreach (var item in list)
            {
                Instantiate(ItemPrefabs, Content).GetComponent<ItemCollection>() .Setup(item);
            }
        }

        public void Selected()
        {
            ViewCollection.Instance.ListQuestion = Question_DAO.GetAllbyCollectionID(ViewCollection.Instance.Collection.ID);
            ViewCollection.Instance.ShowInfo();
        }

    }
}
