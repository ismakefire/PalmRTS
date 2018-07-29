using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.UI;
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

        private int _playerMoney = 1000;

        #endregion

        #region ITeam

        public void OnActorAdded(ActorBehavior actorBehavior)
        {
            _actors.Add(actorBehavior);
        }

        public void OnActorClicked(ActorBehavior actorBehavior)
        {
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("{0}.OnActorClicked(). A panel is up, we can fuck off.", this.ToString());
                return;
            }

            if (!_onClickActions.ContainsKey(actorBehavior))
            {
                Debug.LogFormat("{0}.OnActorClicked() does not contain event.", this.ToString());
                return;
            }

			//Debug.LogFormat("{0}.OnActorClicked() YAY YAY.", this.ToString());
            _onClickActions[actorBehavior]();
        }

        #endregion

        #region Public Interface

        public PlayerTeam()
        {
        }

        public string GetPlayerMoneyString()
        {
            string moneyString = string.Format("${0}", this._playerMoney);
            
            return moneyString;
        }

        public bool SpendMoney(int costInMoney)
        {
            Debug.LogFormat("<color=#ff00ff>{0}.SpendMoney(), (_playerMoney >= costInMoney) = {1}</color>", this.ToString(), (_playerMoney >= costInMoney));

            if (_playerMoney >= costInMoney)
            {
                _playerMoney -= costInMoney;
                UiHudPanel.Instance.MoneyText = string.Format("${0}", _playerMoney);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddClickEvent(ActorBehavior actor, Action clickAction)
        {
            _onClickActions[actor] = clickAction;
        }

        public List<Vector2Int> GenerateAvailableStructureTiles()
        {
            HashList<Vector2Int> result = new HashList<Vector2Int>();

            foreach (ActorBehavior actor in _actors)
            {
                // TODO: Change this to any building providing additional range.
                HQActorBehavior hq = actor.GetComponent<HQActorBehavior>();

                if (hq != null)
                {
                    Debug.LogFormat("<color=#ff00ff>{0}.GenerateAvailableStructureTiles(), has hq. hq.Actor.TilePosition = {1}</color>", this.ToString(), hq.Actor.TilePosition);

                    int gridDistance = 5;
                    int displacementSquaredBound = gridDistance * gridDistance + 2*2;

                    for (int ix = -gridDistance; ix <= gridDistance; ix++)
                    {
                        for (int iy = -gridDistance; iy <= gridDistance; iy++)
                        {
                            int displacementSquared = ix * ix + iy * iy;
                            
                            if (displacementSquared > 0 && displacementSquared <= displacementSquaredBound)
                            {
                                result.Add(hq.Actor.TilePosition + new Vector2Int(ix, iy));
                            }
                        }
                    }
                }
            }

            foreach (ActorBehavior actor in _actors)
            {
                HQActorBehavior hq = actor.GetComponent<HQActorBehavior>();
                DrillStructureBehavior drillStructure = actor.GetComponent<DrillStructureBehavior>();

                if (hq != null || drillStructure != null)
                {
                    result.Remove(actor.TilePosition);
                }
            }

            List<Vector2Int> resultOutput = new List<Vector2Int>();

            resultOutput.AddRange(result);

            return resultOutput;
        }

        #endregion
	}
}
