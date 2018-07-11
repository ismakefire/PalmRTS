using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
	public class HQActorBehavior : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private Vector3 _actorSpawnOffset;

        [SerializeField]
        private GameObject _constructionBotPrefab;

        [SerializeField]
        private GameObject _drillStructurePrefab;

        #endregion

        #region Properties

		public ActorBehavior Actor
		{
			get
			{
				return GetComponent<ActorBehavior>();
			}
		}

        #endregion
        
        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);

            playerTeam.AddClickEvent(Actor, ShowHQPanel);

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
			PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);

            if (playerTeam.SpendMoney(40))
            {
				Debug.LogFormat("{0}.OnCreateConstructionBot()", this.ToString());
				
				GameObject newConstructionBot = Instantiate(_constructionBotPrefab);
				newConstructionBot.transform.SetParent(this.transform.parent);
				newConstructionBot.transform.localPosition = this.transform.localPosition + _actorSpawnOffset + Random.insideUnitSphere * 0.1f;
				
				ActorBehavior actor = newConstructionBot.GetComponent<ActorBehavior>();
				ActorModelManager.Instance.Add(actor);
				
				ConstructionBotActorBehavior constructionBot = newConstructionBot.GetComponent<ConstructionBotActorBehavior>();
				constructionBot.DrillStructurePrefab = _drillStructurePrefab;
            }
        }

        protected void OnCreateTransitVehicle()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);
            bool purchaseWorked = playerTeam.SpendMoney(10);
            
            Debug.LogFormat("{0}.OnCreateTransitVehicle(), purchaseWorked = {1}", this.ToString(), purchaseWorked);
        }

		protected void OnCreateMiningDrill()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);
            bool purchaseWorked = playerTeam.SpendMoney(100);

            Debug.LogFormat("{0}.OnCreateMiningDrill(), purchaseWorked = {1}", this.ToString(), purchaseWorked);
		}

        #endregion

	}
}
