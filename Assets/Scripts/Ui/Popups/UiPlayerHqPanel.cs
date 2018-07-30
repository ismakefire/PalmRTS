using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Transit;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class UiPlayerHqPanel : MonoBehaviour
	{
        #region Types

        public class PlayerHQActions
        {
            public IInventoryStructure Structure { get; set; }
            
            public Action CreateConstructionBot { get; set; }
            public Action CreateTransitVehicle { get; set; }
            public Action CreateMiningDrill { get; set; }
        }

        #endregion

        #region Variables

        private readonly PanelModel<PlayerHQActions> _panelModel = new PanelModel<PlayerHQActions>();
        private readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>();

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

        [SerializeField]
        private Transform _inventoryLayout;

        [SerializeField]
        private InventorySlot _inventorySlotPrefab;

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

            //Debug.LogFormat("{0}.ShowPanel()", this.ToString());

            if (_panelModel.Actions != null && _panelModel.Actions.Structure != null)
            {
				actions.Structure.InventoryChanged += OnInventoryChanged;
            }

            OnInventoryChanged();
        }

        public void HidePanel()
        {
            if (_panelModel.Actions != null && _panelModel.Actions.Structure != null)
            {
				_panelModel.Actions.Structure.InventoryChanged -= OnInventoryChanged;
            }

            _panelModel.Clear();
            this.gameObject.SetActive(false);

            //Debug.LogFormat("{0}.HidePanel()", this.ToString());

            ClearInventory();
        }

        #endregion

        #region UI Events

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

        #region Gameplay Events

        protected void OnInventoryChanged()
        {
            ClearInventory();

            Debug.LogFormat("<color=#ff00ff>{0}.OnInventoryChanged(), _panelModel.Actions.Structure.Inventory_EmptyBoxCount = {1}, _panelModel.Actions.Structure.Inventory_DrillProductCount = {2}</color>", this.ToString(), _panelModel.Actions.Structure.Inventory_EmptyBoxCount, _panelModel.Actions.Structure.Inventory_DrillProductCount);

            if (_panelModel.Actions.Structure.Inventory_EmptyBoxCount > 0)
            {
                AddItem("Empty Box", _panelModel.Actions.Structure.Inventory_EmptyBoxCount.ToString(), Color.gray);
            }

            for (int i = 0; i < _panelModel.Actions.Structure.Inventory_DrillProductCount; i++)
            {
                AddItem("Drill Product", "1", Color.red);
            }
        }

        #endregion

        #region Private Inventory Methods

        private void ClearInventory()
        {
            foreach (InventorySlot inventorySlot in _inventorySlots)
            {
                UnityEngine.Object.Destroy(inventorySlot.gameObject);
            }

            _inventorySlots.Clear();
        }

        private void AddItem(string itemName, string itemCount, Color itemColor)
        {
            InventorySlot inventorySlot = UnityEngine.Object.Instantiate<InventorySlot>(_inventorySlotPrefab);
            inventorySlot.transform.parent = _inventoryLayout;

            inventorySlot.ItemNameText = itemName;
            inventorySlot.ItemCountText = itemCount;
            inventorySlot.ItemIconImage.color = itemColor;

            _inventorySlots.Add(inventorySlot);
        }

        #endregion
	}
}
