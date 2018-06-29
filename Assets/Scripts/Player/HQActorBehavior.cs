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
			UiPlayerHqPanel.Instance.ShowPanel(
                new UiPlayerHqPanel.PlayerHQActions() {
                    CreateConstructionBot = OnCreateConstructionBot,
                    CreateTransitVehicle = OnCreateTransitVehicle
                }
            );
		}

        protected void OnCreateConstructionBot()
        {
            Debug.LogFormat("{0}.OnCreateConstructionBot()", this.ToString());

            GameObject newConstructionBot = Instantiate(_constructionBotPrefab);
            newConstructionBot.transform.SetParent(this.transform.parent);
            newConstructionBot.transform.localPosition = this.transform.localPosition + _actorSpawnOffset + Random.insideUnitSphere * 0.1f;

            ActorBehavior actor = newConstructionBot.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }

        protected void OnCreateTransitVehicle()
        {
            Debug.LogFormat("{0}.OnCreateTransitVehicle()", this.ToString());
        }

        #endregion

	}
}
