using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Team;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    public class PlayerTeam : ITeam
	{
        #region Variables

        private Dictionary<ActorBehavior, Action> _onClickActions = new Dictionary<ActorBehavior, Action>();

        #endregion

        #region ITeam

        public void OnActorClicked(ActorBehavior actorBehavior)
        {
            if (!_onClickActions.ContainsKey(actorBehavior))
            {
                Debug.LogFormat("{0}.OnActorClicked() does not contain event.", this.ToString());
                return;
            }

			Debug.LogFormat("{0}.OnActorClicked() YAY YAY.", this.ToString());
            _onClickActions[actorBehavior]();
        }

        #endregion

        #region Public Interface

        public void AddClickEvent(ActorBehavior actor, Action clickAction)
        {
            _onClickActions[actor] = clickAction;
        }

        #endregion
	}
}
