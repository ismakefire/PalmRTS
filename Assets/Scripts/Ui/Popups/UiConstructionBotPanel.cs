using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class UiConstructionBotPanel : MonoBehaviour
    {
        #region Types

        public class PlayerDeploymentActions
        {
            public Action DeployDrill { get; set; }
        }

        #endregion

        #region Variables

        private PlayerDeploymentActions _actions = null;

        #endregion

        #region Singleton

        private static UiConstructionBotPanel _instance = null;

        public static UiConstructionBotPanel Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region SerializeField

        [SerializeField]
        private Button _deployDrillButton;

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

            _deployDrillButton.onClick.AddListener(OnDeployDrillButtonClicked);
        }

        // Update is called once per frame
        protected void Start()
        {
            HidePanel();
        }

        #endregion

        #region Public Interface

        private float _panelShowTime = -1f;

        public void ShowPanel(PlayerDeploymentActions actions = null)
        {
			_panelShowTime = Time.time;
            _actions = actions;
            this.gameObject.SetActive(true);

            Debug.LogFormat("{0}.ShowPanel()", this.ToString());
        }

        public void HidePanel()
        {
            _actions = null;
            this.gameObject.SetActive(false);

            Debug.LogFormat("{0}.HidePanel()", this.ToString());
        }

        #endregion

        #region Events

        protected void OnDeployDrillButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployDrillButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            Debug.LogFormat("{0}.OnDeployDrillButtonClicked(), we're all good!", this.ToString());

            if (_actions != null && _actions.DeployDrill != null)
            {
                _actions.DeployDrill();
            }

            HidePanel();
        }

        #endregion
	}
}
