using System;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Resource;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Transit;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
    public class HQActorBehavior : MonoBehaviour, IInventoryStructure
    {
        #region Variables

        private readonly ResourceCollection _currentResources = new ResourceCollection();

        #endregion

        #region SerializeField

        [SerializeField]
        private Vector3 _actorSpawnOffset;

        [SerializeField]
        private GameObject _constructionBotPrefab;

        [SerializeField]
        private GameObject _drillStructurePrefab;

        [SerializeField]
        private GameObject _depotStructurePrefab;

        [SerializeField]
        private GameObject _machineFactoryStructurePrefab;

        [SerializeField]
        private GameObject _connectorStructurePrefab;

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

        #endregion

        #region IInventoryStructure

        public ResourceCollection Resources
        {
            get
            {
                return _currentResources;
            }
        }

        public int Inventory_EmptyBoxCount
        {
            get
            {
                return _currentResources.Get(EResourceItem.MetalBox);
            }
            set
            {
                _currentResources.Set(EResourceItem.MetalBox, value);
            }
        }

        public int Inventory_DrillProductCount
        {
            get
            {
                return _currentResources.Get(EResourceItem.SolidRock);
            }
            set
            {
                _currentResources.Set(EResourceItem.SolidRock, value);
            }
        }

        public event Action InventoryChanged;

        #endregion
        
        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
        {
            _currentResources.Changed += OnInventoryChanged;
            OurTeam.AddClickEvent(Actor, ShowHQPanel);

            Inventory_EmptyBoxCount = 20;
            Inventory_DrillProductCount = 2;

            if (InventoryChanged != null)
            {
                InventoryChanged();
            }

            StructureTileManager.Instance.Add(Actor);
		}

        #endregion

        #region Events

		protected void ShowHQPanel()
		{
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("<color=#ff0000>{0}.ShowHQPanel(), PanelManager.Instance.IsAnyChildActive = {1}</color>", this.ToString(), PanelManager.Instance.IsAnyChildActive);
            }
            else
            {
				UiPlayerHqPanel.Instance.ShowPanel(
					new UiPlayerHqPanel.PlayerHQActions() {
                    Structure = this,
					CreateConstructionBot = OnCreateConstructionBot,
					CreateTransitVehicle = OnCreateTransitVehicle,
                    CreateMiningDrill = OnCreateMiningDrill,
                    BuyMetalBox = OnBuyMetalBox,
                    SellMetalBox = OnSellMetalBox
				}
				);
            }
		}

        protected void OnCreateConstructionBot()
        {
            if (OurTeam.SpendMoney(40))
            {
				Debug.LogFormat("{0}.OnCreateConstructionBot()", this.ToString());
				
				GameObject newConstructionBot = Instantiate(_constructionBotPrefab);
				newConstructionBot.transform.SetParent(this.transform.parent);
				newConstructionBot.transform.localPosition = this.transform.localPosition + _actorSpawnOffset + UnityEngine.Random.insideUnitSphere * 0.1f;
				
				ActorBehavior actor = newConstructionBot.GetComponent<ActorBehavior>();
				ActorModelManager.Instance.Add(actor);
				
				ConstructionBotActorBehavior constructionBot = newConstructionBot.GetComponent<ConstructionBotActorBehavior>();
				constructionBot.DrillStructurePrefab = _drillStructurePrefab;
                constructionBot.DepotStructurePrefab = _depotStructurePrefab;
                constructionBot.MachineFactoryStructurePrefab = _machineFactoryStructurePrefab;
                constructionBot.ConnectorStructurePrefab = _connectorStructurePrefab;
            }
        }

        protected void OnCreateTransitVehicle()
        {
            bool purchaseWorked = OurTeam.SpendMoney(10);
            
            Debug.LogFormat("{0}.OnCreateTransitVehicle(), purchaseWorked = {1}", this.ToString(), purchaseWorked);
        }

		protected void OnCreateMiningDrill()
        {
            bool purchaseWorked = OurTeam.SpendMoney(100);

            Debug.LogFormat("{0}.OnCreateMiningDrill(), purchaseWorked = {1}", this.ToString(), purchaseWorked);
		}

        protected void OnBuyMetalBox()
        {
            if (OurTeam.SpendMoney(10))
            {
                Inventory_EmptyBoxCount += 1;

				if (InventoryChanged != null)
				{
					InventoryChanged();
				}
            }
        }

        protected void OnSellMetalBox()
        {
            if (Inventory_EmptyBoxCount >= 1)
            {
                Inventory_EmptyBoxCount -= 1;
                OurTeam.AwardMoney(4);

                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }
            }
        }

        protected void OnInventoryChanged()
        {
            if (InventoryChanged != null)
            {
                InventoryChanged();
            }
        }

        #endregion

	}
}
