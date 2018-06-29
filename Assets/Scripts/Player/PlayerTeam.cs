using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Team;
using Misner.Utility.Collections;
using UnityEngine;

namespace Misner.PalmRTS.Team
{
    public interface ITeam
    {
		void OnActorAdded(ActorBehavior actorBehavior);
        void OnActorClicked(ActorBehavior actorBehavior);
    }
}

namespace Misner.PalmRTS.Player
{
    public class PlayerTeam : ITeam
	{
        #region Variables

        private readonly HashList<ActorBehavior> _actors = new HashList<ActorBehavior>();
        private Dictionary<ActorBehavior, Action> _onClickActions = new Dictionary<ActorBehavior, Action>();

        #endregion

        #region ITeam

        public void OnActorAdded(ActorBehavior actorBehavior)
        {
            _actors.Add(actorBehavior);
        }

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

        public List<Vector2Int> GenerateAvailableStructureTiles()
        {
            List<Vector2Int> result = new List<Vector2Int>();

            foreach (ActorBehavior actor in _actors)
            {
                HQActorBehavior hq = actor.GetComponent<HQActorBehavior>();

                if (hq != null)
                {
                    Debug.LogFormat("<color=#ff00ff>{0}.GenerateAvailableStructureTiles(), has hq. hq.Actor.TilePosition = {1}</color>", this.ToString(), hq.Actor.TilePosition);

                    //
                }
            }

            return result;
        }

        #endregion
	}
}
