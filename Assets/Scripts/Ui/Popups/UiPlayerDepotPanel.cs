using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
	public class UiPlayerDepotPanel : MonoBehaviour
	{
        #region Types

        public class PlayerDepotActions
        {
        }

        #endregion

        #region Variables

        private readonly PanelModel<PlayerDepotActions> _panelModel = new PanelModel<PlayerDepotActions>();

        #endregion
        
        #region MonoBehaviour Singleton

        private static UiPlayerDepotPanel _instance = null;

        public static UiPlayerDepotPanel Instance
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

        #region MonoBehaviour

        // Update is called once per frame
        protected void Start()
        {
            //_createConstructionBotButton.onClick.AddListener(OnCreateConstructionBotButtonClicked);
            //_createTransitVehicleButton.onClick.AddListener(OnCreateTransitVehicleButtonClicked);
            //_createMiningDrillButton.onClick.AddListener(OnCreateMiningDrillButtonClicked);

            HidePanel();
        }

        #endregion

        #region Public Interface

        public void ShowPanel(PlayerDepotActions actions)
        {
            _panelModel.ShowPanel(actions, HidePanel);
            this.gameObject.SetActive(true);
        }

        public void HidePanel()
        {
            _panelModel.Clear();
            this.gameObject.SetActive(false);
        }

        #endregion
	}
}
