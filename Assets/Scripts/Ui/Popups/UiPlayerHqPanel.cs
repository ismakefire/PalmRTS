using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Player;
using Misner.PalmRTS.Resource;
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

            public Action BuyMetalBox { get; set; }
            public Action SellMetalBox { get; set; }
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

        [SerializeField]
        private Button _buyMetalBoxButton;

        [SerializeField]
        private Text _buyMetalBoxText;

        [SerializeField]
        private Button _sellMetalBoxButton;

        [SerializeField]
        private Text _sellMetalBoxText;

        #endregion

        #region MonoBehaviour

        // Update is called once per frame
        protected void Start()
        {
            _createConstructionBotButton.onClick.AddListener(OnCreateConstructionBotButtonClicked);
            _createTransitVehicleButton.onClick.AddListener(OnCreateTransitVehicleButtonClicked);
            _createMiningDrillButton.onClick.AddListener(OnCreateMiningDrillButtonClicked);

            _buyMetalBoxButton.onClick.AddListener(OnBuyMetalBoxButtonClicked);
            _sellMetalBoxButton.onClick.AddListener(OnSellMetalBoxButtonClicked);

            _buyMetalBoxText.text = "-$10";
            _sellMetalBoxText.text = "+$4";

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

        protected void OnBuyMetalBoxButtonClicked()
        {
            _panelModel.PlayPanelAction(_panelModel.Actions.BuyMetalBox, false);
        }

        protected void OnSellMetalBoxButtonClicked()
        {
            _panelModel.PlayPanelAction(_panelModel.Actions.SellMetalBox, false);
        }

        #endregion

        #region Gameplay Events

        protected void OnInventoryChanged()
        {
            IInventoryStructure structure = _panelModel.Actions.Structure;

            ClearInventory();

            foreach (EResourceItem itemKey in ResourceItemUtil.GetAll())
            {
                int amount = structure.Resources.Get(itemKey);

                if (amount > 0)
                {
                    switch (itemKey)
                    {
                        case EResourceItem.SolidRock:
                            for (int i = 0; i < amount; i++)
                            {
                                AddItem("Drilled Rock", "1", Color.red);
                            }
                            break;

                        case EResourceItem.CrushedRock:
                            for (int i = 0; i < amount; i++)
                            {
                                AddItem("Crushed Rock", "1", new Color(1f, 0.5f, 0f));
                            }
                            break;

                        case EResourceItem.MetalPlate:
                            AddItem("Metal Plate", amount.ToString(), new Color(0.5f, 0.5f, 1f));
                            break;

                        case EResourceItem.MetalBox:
                            AddItem("Empty Box", amount.ToString(), Color.gray);
                            break;

                        default:
                            break;
                    }
                }
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
            inventorySlot.transform.SetParent(_inventoryLayout);

            inventorySlot.ItemNameText = itemName;
            inventorySlot.ItemCountText = itemCount;
            inventorySlot.ItemIconImage.color = itemColor;

            _inventorySlots.Add(inventorySlot);
        }

        #endregion
    }
}
