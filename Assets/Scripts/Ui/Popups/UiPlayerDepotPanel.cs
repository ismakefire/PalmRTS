using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Player;
using Misner.PalmRTS.Transit;
using UnityEngine;
using UnityEngine.UI;

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
        private readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>();

        private TransportConnector _transportConnector = null;

        #endregion

        #region SerializeField

        [SerializeField]
        private Transform _inventoryLayout;

        [SerializeField]
        private InventorySlot _inventorySlotPrefab;

        [SerializeField]
        private Transform _noOrdersText;

        [SerializeField]
        private TransitOrderCard _transitOrderCardPrefab;

        [SerializeField]
        private Transform _createNewOrderTransform;

		[SerializeField]
		private Button _createNewOrderButton;

        [SerializeField]
        private Transform _ordersParent;

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
            _createNewOrderButton.onClick.AddListener(OnCreateNewOrderButtonClicked);

            HidePanel();
        }

        protected void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                HidePanel();
            }
        }

        #endregion

        #region Public Interface

        public void ShowPanel(PlayerDepotActions actions, TransportConnector transportConnector)
        {
            _transportConnector = transportConnector;
            _transportConnector.InventoryChanged += OnDepotInventoryChanged;
            _transportConnector.TransitOrdersChanged += OnTransitOrdersChanged;
            
            _panelModel.ShowPanel(actions, HidePanel);
            this.gameObject.SetActive(true);

            OnDepotInventoryChanged();
            OnTransitOrdersChanged();

            _noOrdersText.gameObject.SetActive(false);
        }

        public void HidePanel()
        {
            if (_transportConnector != null)
            {
                _transportConnector.InventoryChanged -= OnDepotInventoryChanged;
                _transportConnector.TransitOrdersChanged -= OnTransitOrdersChanged;
				_transportConnector = null;
            }

            _panelModel.Clear();
            this.gameObject.SetActive(false);

			ClearInventory();

            RemoveAllOrderButtons();
        }

        #endregion

        #region Depot Gameplay Events

        protected void OnDepotInventoryChanged()
        {
            ClearInventory();

            if (_transportConnector.Inventory_EmptyBoxCount > 0)
            {
                AddItem("Empty Box", _transportConnector.Inventory_EmptyBoxCount.ToString(), Color.gray);
            }

            for (int i = 0; i < _transportConnector.Inventory_DrillProductCount; i++)
            {
                AddItem("Drill Product", "1", Color.red);
            }
        }

        protected void OnTransitOrdersChanged()
        {
            RemoveAllOrderButtons();

            if (_transportConnector.TransitOrders == null || _transportConnector.TransitOrders.Count <= 0)
            {
                Debug.LogFormat("<color=#ff00ff>{0}.OnTransitOrdersChanged(), need some orders brah!</color>", this.ToString());
            }
            else
            {
                foreach (TransitOrderController item in _transportConnector.TransitOrders)
                {
                    OrdersCardsHandler orderHandler = AddOrdersButton(item);

                    orderHandler.Setup(item.Verb, item.Object, item.Subject);
                }
            }
        }

        #endregion

        #region Event Methods

        protected void OnCreateNewOrderButtonClicked()
        {
            _transportConnector.AddOrder(new TransitOrderController(_transportConnector, 0));
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

        #region Private Orders Methods

        private List<OrdersCardsHandler> _ordersCardsHandlers = new List<OrdersCardsHandler>();

        private OrdersCardsHandler AddOrdersButton(TransitOrderController transitOrder)
        {
            // Create card
			TransitOrderCard newTransitOrderCard = UnityEngine.Object.Instantiate<TransitOrderCard>(_transitOrderCardPrefab);

            // Add to UI
			_createNewOrderTransform.parent = null;
            newTransitOrderCard.transform.parent = _ordersParent;
			_createNewOrderTransform.parent = _ordersParent;

            // Setup for use
            OrdersCardsHandler handler = new OrdersCardsHandler(this, transitOrder, newTransitOrderCard, RemoveOrder);
            _ordersCardsHandlers.Add(handler);

            return handler;
        }

        /// <summary>
        /// Only removes the buttons, no model changes.
        /// </summary>
        private void RemoveAllOrderButtons()
        {
            foreach (OrdersCardsHandler handler in _ordersCardsHandlers)
            {
                handler.Clear();
            }

            _ordersCardsHandlers.Clear();
        }

        /// <summary>
        /// Removes the button and the model behind it.
        /// </summary>
        /// <param name="handler">Handler.</param>
        private void RemoveOrder(OrdersCardsHandler handler)
        {
            _transportConnector.RemoveOrder(handler.TransitOrder);
        }

        #endregion
	}
}
