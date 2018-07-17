using System;
using System.Collections;
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
            public Action CreateMiningDrill { get; set; }
        }

        #endregion

        #region Variables

        private readonly PanelModel<PlayerHQActions> _panelModel = new PanelModel<PlayerHQActions>();

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

        // Use this for initialization
        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        #endregion

        #region SerializeField

        [SerializeField]
        private Button _createConstructionBotButton;

		[SerializeField]
		private Button _createTransitVehicleButton;

        [SerializeField]
        private Button _createMiningDrillButton;

        #endregion

        #region MonoBehaviour

        // Update is called once per frame
        protected void Start()
        {
			_createConstructionBotButton.onClick.AddListener(OnCreateConstructionBotButtonClicked);
			_createTransitVehicleButton.onClick.AddListener(OnCreateTransitVehicleButtonClicked);
			_createMiningDrillButton.onClick.AddListener(OnCreateMiningDrillButtonClicked);

            HidePanel();
		}

        #endregion

        #region Public Interface

        public void ShowPanel(PlayerHQActions actions)
        {
            _panelModel.ShowPanel(actions, HidePanel);
            this.gameObject.SetActive(true);

            Debug.LogFormat("{0}.ShowPanel()", this.ToString());
        }

        public void HidePanel()
        {
            _panelModel.Clear();
            this.gameObject.SetActive(false);

            Debug.LogFormat("{0}.HidePanel()", this.ToString());
        }

        #endregion

        #region Events

        protected void OnCreateConstructionBotButtonClicked()
        {
            _panelModel.PlayPanelAction(_panelModel.Actions.CreateConstructionBot);
        }

        protected void OnCreateTransitVehicleButtonClicked()
        {
            _panelModel.PlayPanelAction(_panelModel.Actions.CreateTransitVehicle);
        }

        protected void OnCreateMiningDrillButtonClicked()
        {
            _panelModel.PlayPanelAction(_panelModel.Actions.CreateMiningDrill);
        }

        #endregion
	}
}
