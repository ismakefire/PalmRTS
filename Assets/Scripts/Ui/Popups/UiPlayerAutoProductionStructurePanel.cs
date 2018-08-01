using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Player;
using Misner.PalmRTS.Resource;
using Misner.PalmRTS.Transit;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
    public class UiPlayerAutoProductionStructurePanel : MonoBehaviour
    {
        #region Types

        public class PlayerStructureActions
        {
        }

        #endregion

        #region Variables

        private readonly PanelModel<PlayerStructureActions> _panelModel = new PanelModel<PlayerStructureActions>();
        private readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>();

        private AutoProductionStructureActor _structure = null;

        #endregion

        #region SerializeField

        [SerializeField]
        private Transform _inventoryLayout;

        [SerializeField]
        private InventorySlot _inventorySlotPrefab;

        [SerializeField]
        private ProgressBar _progressBar;

        #endregion

        #region MonoBehaviour Singleton

        private static UiPlayerAutoProductionStructurePanel _instance = null;

        public static UiPlayerAutoProductionStructurePanel Instance
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
            HidePanel();
        }

        protected void Update()
        {
            if (_structure != null)
            {
                _progressBar.Progress = _structure.ProductionProgress;
            }
        }

        #endregion

        #region Public Interface

        public void ShowPanel(PlayerStructureActions actions, AutoProductionStructureActor structure)
        {
            _structure = structure;
            _structure.InventoryChanged += OnStructureInventoryChanged;

            _panelModel.ShowPanel(actions, HidePanel);
            this.gameObject.SetActive(true);

            OnStructureInventoryChanged();
        }

        public void HidePanel()
        {
            if (_structure != null)
            {
                _structure.InventoryChanged -= OnStructureInventoryChanged;
            }
            _structure = null;

            ClearInventory();

            _panelModel.Clear();
            this.gameObject.SetActive(false);
        }

        #endregion

        #region Structure Gameplay Events

        protected void OnStructureInventoryChanged()
        {
            ClearInventory();

			foreach (EResourceItem itemKey in ResourceItemUtil.GetAll())
			{
				int amount = _structure.Resources.Get(itemKey);
				
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
