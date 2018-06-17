using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
	public class HQActorBehavior : MonoBehaviour
	{
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
		}

        #endregion

        #region Events

		protected void ShowHQPanel()
		{
			UiPlayerHqPanel.Instance.ShowPanel();
		}

        #endregion

	}
}
