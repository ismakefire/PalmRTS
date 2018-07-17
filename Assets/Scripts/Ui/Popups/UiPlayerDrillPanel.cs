using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Player;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
	public class UiPlayerDrillPanel : MonoBehaviour
    {
        #region Types

        public class PlayerDrillActions
        {
        }

        #endregion

        #region Variables

        private readonly PanelModel<PlayerDrillActions> _panelModel = new PanelModel<PlayerDrillActions>();
        private readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>();

        private DrillStructureBehavior _drill = null;

        #endregion

        #region SerializeField

        [SerializeField]
        private Transform _inventoryLayout;

		[SerializeField]
		private InventorySlot _inventorySlotPrefab;

        [SerializeField]
        private ProgressBar _miningProgressBar;

        #endregion

        #region MonoBehaviour Singleton

        private static UiPlayerDrillPanel _instance = null;

        public static UiPlayerDrillPanel Instance
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

        protected void Update()
        {
            if (_drill != null)
            {
                _miningProgressBar.Progress = _drill.MiningProgress;
            }
        }

        #endregion

        #region Public Interface

        public void ShowPanel(PlayerDrillActions actions, DrillStructureBehavior drill)
        {
            _drill = drill;
            _drill.InventoryChanged += OnDrillInventoryChanged;
            
            _panelModel.ShowPanel(actions, HidePanel);
            this.gameObject.SetActive(true);

            OnDrillInventoryChanged();
        }

        public void HidePanel()
        {
            if (_drill != null)
            {
				_drill.InventoryChanged -= OnDrillInventoryChanged;
            }
            _drill = null;
            
			ClearInventory();

            _panelModel.Clear();
            this.gameObject.SetActive(false);
        }

        #endregion

        #region Drill Gameplay Events

        protected void OnDrillInventoryChanged()
        {
            ClearInventory();

            AddItem("Empty Box", _drill.EmptyBoxCount.ToString(), Color.gray);

            for (int i = 0; i < _drill.FullBoxCount; i++)
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
