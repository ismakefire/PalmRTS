using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Financial;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Transit;
using Misner.PalmRTS.UI;
using Misner.Utility.Collections;
using UnityEngine;

namespace Misner.PalmRTS.Team
{
    public interface ITeam
    {
        void OnActorAdded(ActorBehavior actorBehavior);
        void OnActorRemoved(ActorBehavior actorBehavior);
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
        private readonly DebtModel _debtModel;

        private int _playerMoney = 500;

        #endregion

        #region ITeam

        public void OnActorAdded(ActorBehavior actorBehavior)
        {
            _actors.Add(actorBehavior);
        }

        public void OnActorRemoved(ActorBehavior actorBehavior)
        {
            _actors.Remove(actorBehavior);
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
            _debtModel = DebtModel.Create(amount: _playerMoney * 9, pps: 0.00001);
            _debtModel.BalanceChange += OnBalanceChange;
            _debtModel.DebtChanged += OnDebtChanged;
        }

        protected bool OnBalanceChange(int change)
        {
            bool canAffordInterest = false;

            if (_playerMoney + change > 0)
            {
                _playerMoney += change;
                canAffordInterest = true;

                UiHudPanel.Instance.MoneyText = GetPlayerMoneyString();
            }

            return canAffordInterest;
        }

        protected void OnDebtChanged()
        {
            UiHudPanel.Instance.DebtText = GetPlayerDebtString();
        }

        public string GetPlayerMoneyString()
        {
            string moneyString = string.Format("${0}", this._playerMoney);
            
            return moneyString;
        }

        public string GetPlayerDebtString()
        {
            string debtString = string.Format("${0}", _debtModel.DebtBalance);

            return debtString;
        }

        public bool SpendMoney(int costInMoney)
        {
            Debug.LogFormat("<color=#ff00ff>{0}.SpendMoney(), (_playerMoney >= costInMoney) = {1}</color>", this.ToString(), (_playerMoney >= costInMoney));

            if (_playerMoney >= costInMoney)
            {
                _playerMoney -= costInMoney;
                UiHudPanel.Instance.MoneyText = GetPlayerMoneyString();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void AwardMoney(int moneyAwarded)
        {
            _playerMoney += moneyAwarded;
            UiHudPanel.Instance.MoneyText = GetPlayerMoneyString();

            Debug.LogFormat("<color=#00ff00>{0}.SpendMoney(), moneyAwarded = {1}, _playerMoney = {2}</color>", this.ToString(), moneyAwarded, _playerMoney);
        }

        public void AddClickEvent(ActorBehavior actor, Action clickAction)
        {
            _onClickActions[actor] = clickAction;
        }

        public ActorBehavior GetActorByTile(Vector2Int tileLocation)
        {
            ActorBehavior resultActor = null;
            
            foreach (ActorBehavior actorBehavior in _actors)
            {
                if (actorBehavior.TilePosition == tileLocation)
                {
                    resultActor = actorBehavior;
                }
            }

            return resultActor;
        }

        public List<Vector2Int> GenerateRecyclableStructureTiles()
        {
            List<Vector2Int> resultOutput = new List<Vector2Int>();

            foreach (ActorBehavior actorBehavior in _actors)
            {
                if (actorBehavior.GetComponent<HQActorBehavior>())
                {
                    continue;
                }

                if (actorBehavior.GetComponent<IInventoryStructure>() != null)
                {
                    resultOutput.Add(actorBehavior.TilePosition);
                }
            }

            return resultOutput;
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
                if (
                    actor.GetComponent<HQActorBehavior>() != null ||
                    actor.GetComponent<DrillStructureBehavior>() != null ||
                    actor.GetComponent<TransitDepotStructureActor>() != null ||
                    actor.GetComponent<MachineFactoryStructureActoryBehavior>() != null ||
                    actor.GetComponent<TransportConnector>() != null ||
                    actor.GetComponent<AutoProductionStructureActor>() != null
                )
                {
                    result.Remove(actor.TilePosition);
                }
            }

            List<Vector2Int> resultOutput = new List<Vector2Int>();

            resultOutput.AddRange(result);

            return resultOutput;
        }

        public HQActorBehavior GetFirstHQ()
        {
            HQActorBehavior hq = null;

            foreach (ActorBehavior actor in _actors)
            {
                hq = actor.GetComponent<HQActorBehavior>();

                if (hq != null)
                {
                    break;
                }
            }

            return hq;
        }

        #endregion
	}
}
