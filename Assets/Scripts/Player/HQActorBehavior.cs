using Misner.PalmRTS.Actor;
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

        public int Inventory_EmptyBoxCount { get; set; }
        public int Inventory_DrillProductCount { get; set; }

        #endregion
        
        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            OurTeam.AddClickEvent(Actor, ShowHQPanel);

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
					CreateConstructionBot = OnCreateConstructionBot,
					CreateTransitVehicle = OnCreateTransitVehicle,
                    CreateMiningDrill = OnCreateMiningDrill
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
				newConstructionBot.transform.localPosition = this.transform.localPosition + _actorSpawnOffset + Random.insideUnitSphere * 0.1f;
				
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

        #endregion

	}
}
