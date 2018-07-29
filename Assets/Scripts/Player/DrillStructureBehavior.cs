using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
	public class DrillStructureBehavior : MonoBehaviour
    {
        #region Variables

        private readonly float _miningRateUps = 10.0f / 30f; 

        private int _emptyBoxCount = 15;
        private int _fullBoxCount = 0;

        private float _miningProgress = 0f;

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

        public float MiningProgress
        {
            get
            {
                return _miningProgress;
            }
        }

        public int EmptyBoxCount
        {
            get
            {
                return _emptyBoxCount;
            }
            set
            {
                _emptyBoxCount = value;

                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }
            }
        }

        public int FullBoxCount
        {
            get
            {
                return _fullBoxCount;
            }
            set
            {
                _fullBoxCount = value;

                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }
            }
        }

        public event Action InventoryChanged;

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
        {
            OurTeam.AddClickEvent(Actor, ShowDrillPanel);

            StructureTileManager.Instance.Add(Actor);
		}

        protected void Update ()
        {
            _miningProgress += Time.deltaTime * _miningRateUps;

            if (_miningProgress >= 1f)
            {
                if (_emptyBoxCount > 0)
                {
                    --_emptyBoxCount;
                    ++_fullBoxCount;

                    _miningProgress = 0f;

                    if (InventoryChanged != null)
                    {
                        InventoryChanged();
                    }
                }
                else
                {
                    _miningProgress = 1f;
                }
            }
        }

        #endregion

        #region Events

        protected void ShowDrillPanel()
        {
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("<color=#ff0000>{0}.ShowDrillPanel(), PanelManager.Instance.IsAnyChildActive = {1}</color>", this.ToString(), PanelManager.Instance.IsAnyChildActive);
            }
            else
            {
                Debug.LogFormat("<color=#ff00ff>{0}.TODO()</color>", this.ToString());

                UiPlayerDrillPanel.Instance.ShowPanel(new UiPlayerDrillPanel.PlayerDrillActions(), this);

                //UiPlayerHqPanel.Instance.ShowPanel(
                //    new UiPlayerHqPanel.PlayerHQActions()
                //    {
                //        CreateConstructionBot = OnCreateConstructionBot,
                //        CreateTransitVehicle = OnCreateTransitVehicle,
                //        CreateMiningDrill = OnCreateMiningDrill
                //    }
                //);
            }
        }

        #endregion
	}
}
