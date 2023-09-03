using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ItemCollection : MonoBehaviour
    {
        public TMP_Text Name, Amount;
        Collection_Entity itemSelected;
        public void Setup(Collection_Entity item)
        {
            this.itemSelected = item;
            this.Name.text = item.Name;  
            this.Amount.text = item.AmountQuestion.ToString();
        }

        public void onClick()
        {
            ViewCollection.Instance.Collection = this.itemSelected;
        }
    }
}
