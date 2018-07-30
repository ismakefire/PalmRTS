﻿using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Transit;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
    public class TransportConnector : MonoBehaviour, ITransitActor
    {
        #region Private Variables

        private TransitOrderController _currentOrder = null;

        #endregion

        #region Properties

        public ActorBehavior Actor
        {
            get
            {
                return GetComponent<ActorBehavior>();
            }
        }

        public PlayerTeam OurTeam
        {
            get
            {
                PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);

                return playerTeam;
            }
        }

        private int _emptyBoxCount;
        public int EmptyBoxCount
        {
            get
            {
                return _emptyBoxCount;
            }
            set
            {
                _emptyBoxCount = value;

                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }
            }
        }

        private int _drillProductCount;
        public int DrillProductCount
        {
            get
            {
                return _drillProductCount;
            }
            set
            {
                _drillProductCount = value;

                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }
            }
        }

        private readonly List<TransitOrderController> _transitOrders = new List<TransitOrderController>();
        public IList<TransitOrderController> TransitOrders
        {
            get
            {
                List<TransitOrderController> orders = new List<TransitOrderController>();

                foreach (var item in _transitOrders)
                {
                    orders.Add(item);
                }

                return orders;
            }
        }

        public event Action InventoryChanged;
        public event Action TransitOrdersChanged;

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        protected void Start()
        {
            OurTeam.AddClickEvent(Actor, ShowTransitDepotPanel);

            EmptyBoxCount = 20;
            DrillProductCount = 2;

            _lastTick = Time.time;

            StructureTileManager.Instance.Add(Actor);
        }

        private float _lastTick;

        protected void Update()
        {
            if (_currentOrder != null && Time.time - _lastTick >= _currentOrder.Duration)
            {
                //Debug.LogFormat("<color=#ff00ff>{0}.Update(), _currentOrder.Duration = {1}, _currentOrder.Verb = {2}, _currentOrder.Object = {3}</color>", this.ToString(), _currentOrder.Duration, _currentOrder.Verb, _currentOrder.Object);

                NextOrder();
            }
        }

        #endregion

        #region Public Methods

        public void AddOrder(TransitOrderController transitOrder)
        {
            _transitOrders.Add(transitOrder);

            if (_currentOrder == null)
            {
                _currentOrder = transitOrder;
                transitOrder.IsPrimaryOrder = true;
                _lastTick = Time.time;
            }

            if (TransitOrdersChanged != null)
            {
                TransitOrdersChanged();
            }
        }

        public void RemoveOrder(TransitOrderController transitOrder)
        {
            if (_currentOrder == transitOrder)
            {
                int lastOrderIndex = _transitOrders.IndexOf(_currentOrder);

                _transitOrders.Remove(transitOrder);

                // Cycle the index back to the front.
                if (lastOrderIndex >= _transitOrders.Count)
                {
                    lastOrderIndex = 0;
                }

                if (lastOrderIndex < _transitOrders.Count)
                {
                    _currentOrder = _transitOrders[lastOrderIndex];
                    _currentOrder.IsPrimaryOrder = true;
                    _lastTick = Time.time;
                }
                else
                {
                    _currentOrder = null;
                }
            }
            else
            {
                _transitOrders.Remove(transitOrder);
            }

            _transitOrders.Remove(transitOrder);

            if (TransitOrdersChanged != null)
            {
                TransitOrdersChanged();
            }
        }

        #endregion

        #region Events

        protected void ShowTransitDepotPanel()
        {
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("<color=#ff0000>{0}.ShowTransitDepotPanel(), PanelManager.Instance.IsAnyChildActive = {1}</color>", this.ToString(), PanelManager.Instance.IsAnyChildActive);
            }
            else
            {
                //Debug.LogFormat("<color=#ff00ff>{0}.TODO()</color>", this.ToString());

                UiPlayerTransportConnectorPanel.Instance.ShowPanel(
                    new UiPlayerTransportConnectorPanel.PlayerConnectorActions(),
                    this
                );
            }
        }

        #endregion

        #region Private Methods

        private void NextOrder()
        {
            if (_currentOrder == null)
            {
                return;
            }

            int lastOrderIndex = _transitOrders.IndexOf(_currentOrder);
            _currentOrder.IsPrimaryOrder = false;
            _currentOrder.CompleteOrder();

            int nextOrderIndex = lastOrderIndex + 1;

            // Cycle the index back to the front.
            if (nextOrderIndex >= _transitOrders.Count)
            {
                nextOrderIndex = 0;
            }

            if (nextOrderIndex < _transitOrders.Count)
            {
                _currentOrder = _transitOrders[nextOrderIndex];
                _currentOrder.IsPrimaryOrder = true;
                _lastTick = Time.time;
            }
            else
            {
                _currentOrder = null;
            }
        }

        #endregion
    }
}
