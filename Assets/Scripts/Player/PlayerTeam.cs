using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    public class PlayerTeam : ITeam
	{
        #region ITeam

        public void OnActorClicked(ActorBehavior actorBehavior)
        {
            Debug.LogFormat("{0}.OnActorClicked()", this.ToString());

            UiPlayerHqPanel.Instance.ShowPanel();
        }

        #endregion
	}
}
