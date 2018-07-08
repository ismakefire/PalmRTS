using System;
using UnityEngine;

namespace Misner.PalmRTS.Selection
{
	public class SelectionTileItemBehavior : MonoBehaviour
	{
        #region Properties

        public Action OnItemClicked { get; set; }

        #endregion

        #region MonoBehaviour

        protected void OnMouseUp ()
		{
            Debug.LogFormat("<color=#ff00ff>{0}.OnMouseUp()</color>", this.ToString());

            if (OnItemClicked != null)
            {
                OnItemClicked();
            }
            else
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnMouseUp(), No action setup.</color>", this.ToString());
            }
		}

        #endregion
	}
}
