using Assets.Scripts.Common;
using Assets.Scripts.Creator.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Login;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Creator
{
    public class Creator : MonoBehaviour
    {
        public Button Skinbtn, Hairbtn, Eyebtn, Mouthbtn, Savebtn;
        public GameObject Content, ItemPrefabs, MessageError;

        public SpriteRenderer HairSpr, EyeSpr, MouthSpr, ShirtSpr, LeftHandSpr, RightHandSpr, LeftFootSpr, RightFootSpr;
        public string HairID, EyeID, MouthID, SkinID;


        public TMP_InputField Name;
        List<Skin_Entity> listSkin;
        List<Eye_Enity> listEye;
        List<Mouth_Entity> listMouth;
        List<Hair_Entity> listHair;

        public static Creator Instance;

        private void Awake()
        {
            Instance = this;

            listSkin = Skin_DAO.GetAll();
            listEye = Eye_DAO.GetAll();
            listMouth = Mouth_DAO.GetAll();
            listHair = Hair_DAO.GetAll();

            Skinbtn.onClick.AddListener(GetSkin);
            Hairbtn.onClick.AddListener(GetHair);
            Eyebtn.onClick.AddListener(GetEye);
            Mouthbtn.onClick.AddListener(GetMouth);
            Savebtn.onClick.AddListener(SaveLayout);
        }

        private void Start()
        {
            GetSkin();

            LoadHair(listHair[0].ID, listHair[0].Link);
            LoadEye(listEye[0].ID, listEye[0].Link);
            LoadMouth(listMouth[0].ID, listMouth[0].Link);
            LoadSkin(listSkin[0].ID, listSkin[0].Link);

            if (References.account != null)
            {
                Name.text = References.account.Name;
                Debug.Log(References.account.HairID + " - " +  listHair.Find(obj => obj.ID == References.account.HairID).Link);
                Debug.Log(References.account.EyeID + " - " + listEye.Find(obj => obj.ID == References.account.EyeID).Link);
                Debug.Log(References.account.MouthID + " - " + listMouth.Find(obj => obj.ID == References.account.MouthID).Link);
                Debug.Log(References.account.SkinID + " - " + listSkin.Find(obj => obj.ID == References.account.SkinID).Link);
                
                LoadHair(References.account.HairID, listHair.Find(obj => obj.ID == References.account.HairID).Link);
                LoadEye(References.account.EyeID, listEye.Find(obj => obj.ID == References.account.EyeID).Link);
                LoadMouth(References.account.MouthID, listMouth.Find(obj => obj.ID == References.account.MouthID).Link);
                LoadSkin(References.account.SkinID, listSkin.Find(obj => obj.ID == References.account.SkinID).Link);
            }
        }

        #region Setup Content
        public void GetSkin()
        {
            DestroyContent();
            Transform parentTransform = Content.transform;

            foreach (var item in listSkin)
            {
                var child = Instantiate(ItemPrefabs, parentTransform);
                child.GetComponent<ItemLoad>().Setup(item.ID, item.Name, item.Link, TypeLayout.Skin);
                child.transform.parent = parentTransform;
            }
        }

        public void GetEye()
        {
            DestroyContent();
            Transform parentTransform = Content.transform;

            foreach (var item in listEye)
            {
                var child = Instantiate(ItemPrefabs, parentTransform);
                child.GetComponent<ItemLoad>().Setup(item.ID, item.Name, item.Link, TypeLayout.Eye);
                child.transform.parent = parentTransform;
            }
        }

        public void GetHair()
        {
            DestroyContent();
            Transform parentTransform = Content.transform;

            foreach (var item in listHair)
            {
                var child = Instantiate(ItemPrefabs, parentTransform);
                child.GetComponent<ItemLoad>().Setup(item.ID, item.Name, item.Link, TypeLayout.Hair);
                child.transform.parent = parentTransform;
            }
        }

        public void GetMouth()
        {
            DestroyContent();
            Transform parentTransform = Content.transform;

            foreach (var item in listMouth)
            {
                var child = Instantiate(ItemPrefabs, parentTransform);
                child.GetComponent<ItemLoad>().Setup(item.ID, item.Name, item.Link, TypeLayout.Mouth);
                child.transform.parent = parentTransform;
            }
        }

        public void DestroyContent()
        {
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion


        #region Load layout

        public void LoadHair(string ID, string path)
        {
            HairSpr.sprite = Resources.Load<Sprite>(path);
            HairID = ID;
        }

        public void LoadEye(string ID, string path)
        {
            EyeSpr.sprite = Resources.Load<Sprite>(path);
            EyeID = ID;
        }

        public void LoadMouth(string ID, string path)
        {
            MouthSpr.sprite = Resources.Load<Sprite>(path);
            MouthID = ID;
        }

        public void LoadSkin(string ID, string path)
        {
            ShirtSpr.sprite = Resources.Load<Sprite>(path + "_Shirt");
            LeftFootSpr.sprite = Resources.Load<Sprite>(path + "_LeftFoot");
            LeftHandSpr.sprite = Resources.Load<Sprite>(path + "_LeftHand");
            RightFootSpr.sprite = Resources.Load<Sprite>(path + "_RightFoot");
            RightHandSpr.sprite = Resources.Load<Sprite>(path + "_RightHand");
            SkinID = ID;
        }

        #endregion

        #region Save Layout

        public void SaveLayout()
        {
            //References.account = new Account_Entity();
            //References.account.Username = "thienthien";
            if (string.IsNullOrEmpty(Name.text.Trim()))
            {
                MessageError.GetComponent<UI_Login>().ShowMessage(Message.NameNull);
            }
            else if (Account_DAO.CheckName(Name.text, References.account.Username))
            {
                Account_DAO.UpdateLayout(References.account.Username, Name.text, HairID, EyeID, MouthID, SkinID);
                SceneManager.LoadScene(Scenes.Lobby);
            }
            else
            {
                MessageError.GetComponent<UI_Login>().ShowMessage(Message.NameExist);
            }
        }
        #endregion

        public void BacktoHome()
        {
            if(!References.account.IsFirst)
            {
                SceneManager.LoadScene(Scenes.Lobby);
            }
            else
            {
                References.account = null;
                SceneManager.LoadScene(Scenes.Login);
            }
        }
    }
}
