using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
	public class PanelManager : MonoBehaviour
	{
        #region MonoBehaviour Singleton

        private static PanelManager _instance = null;

        public static PanelManager Instance
        {
            get
            {
                return _instance;
            }
        }

        // Use this for initialization
        protected void Awake()
        {
            _instance = this;
        }

        #endregion

        #region Public Interface

        public bool IsAnyChildActive
        {
            get
            {
                for (int index = 0; index < transform.childCount; index++)
                {
                    Transform childTransform = transform.GetChild(index);

                    if (childTransform.gameObject.activeSelf)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion
	}
}
