﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class UiPlayerHqPanel : MonoBehaviour
	{
        #region Types

        public class PlayerHQActions
        {
            public Action CreateConstructionBot { get; set; }
            public Action CreateTransitVehicle { get; set; }
        }

        #endregion

        #region Variables

        private PlayerHQActions _actions = null;

        #endregion

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

        #region SerializeField

        [SerializeField]
        private Button _createConstructionBotButton;

        [SerializeField]
        private Button _createTransitVehicleButton;

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

            _createConstructionBotButton.onClick.AddListener(OnCreateConstructionBotButtonClicked);
            _createTransitVehicleButton.onClick.AddListener(OnCreateTransitVehicleButtonClicked);
        }

        // Update is called once per frame
		protected void Start()
		{
            HidePanel();
		}

        #endregion

        #region Public Interface

        public void ShowPanel(PlayerHQActions actions)
        {
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

        protected void OnCreateConstructionBotButtonClicked()
        {
            Debug.LogFormat("{0}.OnCreateConstructionBotButtonClicked(), we're all good!", this.ToString());

            if (_actions != null && _actions.CreateConstructionBot != null)
            {
                _actions.CreateConstructionBot();
            }

            HidePanel();
        }

        protected void OnCreateTransitVehicleButtonClicked()
        {
            Debug.LogFormat("{0}.OnCreateTransitVehicleButtonClicked(), we're all good!", this.ToString());

            if (_actions != null && _actions.CreateTransitVehicle != null)
            {
                _actions.CreateTransitVehicle();
            }

            HidePanel();
        }

        #endregion
	}
}
