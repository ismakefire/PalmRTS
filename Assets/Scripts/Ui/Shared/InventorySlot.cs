using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class InventorySlot : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private Text _itemCount;

        [SerializeField]
        private Text _itemName;

        [SerializeField]
        private Image _itemIconImage;

        #endregion

        #region Properties

        public string ItemCountText
        {
            get
            {
                return _itemCount.text;
            }
            set
            {
                _itemCount.text = value;
            }
        }

        public string ItemNameText
        {
            get
            {
                return _itemName.text;
            }
            set
            {
                _itemName.text = value;
            }
        }

        public Image ItemIconImage
        {
            get
            {
                return _itemIconImage;
            }
        }

        #endregion
	}
}
