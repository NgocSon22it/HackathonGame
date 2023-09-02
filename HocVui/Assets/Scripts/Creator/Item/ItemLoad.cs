using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Creator.Item
{
    public class ItemLoad : MonoBehaviour
    {
        public Image Image, Background;
        public TMP_Text Name;
        public Button Obj;
        public string ID, path;
        public TypeLayout type;

        public void Setup(string ID, string name, string path, TypeLayout type)
        {
            this.type = type;
            this.path = path;
            this.ID = ID;
            Name.text = name;

            Image.sprite = Resources.Load<Sprite>(path + "-item");

            Obj.onClick.AddListener(onClick);
        }

        public void onClick()
        {
            switch (type)
            {
                case TypeLayout.Hair:
                    Creator.Instance.LoadHair(ID, path);
                    break;
                case TypeLayout.Eye:
                    Creator.Instance.LoadEye(ID, path);

                    break;
                case TypeLayout.Mouth:
                    Creator.Instance.LoadMouth(ID, path);

                    break;
                case TypeLayout.Skin:
                    Creator.Instance.LoadSkin(ID, path);
                    break;

            }
        }
    }
}
