using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
	public class UiPlayerHqPanel : MonoBehaviour
	{
        #region Singleton

        private static UiPlayerHqPanel _instance = null;

        public static UiPlayerHqPanel Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Debug.LogErrorFormat("{0}.Awake(), why are there two of these?", this.ToString());
            }
        }

        // Update is called once per frame
		protected void Start()
		{
            HidePanel();
		}

        #endregion

        #region Public Interface

        public void ShowPanel()
        {
            this.gameObject.SetActive(true);

            Debug.LogFormat("{0}.ShowPanel()", this.ToString());
        }

        public void HidePanel()
        {
            this.gameObject.SetActive(false);

            Debug.LogFormat("{0}.HidePanel()", this.ToString());
        }

        #endregion
	}
}
