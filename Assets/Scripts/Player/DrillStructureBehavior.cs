using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Resource;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Terrain;
using Misner.PalmRTS.Transit;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
    public class DrillStructureBehavior : MonoBehaviour, IInventoryStructure
    {
        #region Variables

        private readonly float _miningRateUps = 10.0f / 30f;

        private float _groundDrilledCount = 0f;
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

        #endregion

        #region IInventoryStructure

        public ResourceCollection Resources
        {
            get
            {
                // TODO
                return null;
            }
        }

        public int Inventory_EmptyBoxCount
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

        public int Inventory_DrillProductCount
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
			TerrainTileBehavior tile = TerrainTileParentBehavior.Instance.GetTile(Actor.TilePosition);
			//Debug.LogFormat("<color=#ff00ff>{0}.Update(), tile.transform.position = {1}, transform.position = {2}, Actor.TilePosition = {3}</color>", this.ToString(), tile.transform.position, transform.position, Actor.TilePosition);


            float miningRateCoef = 1.0f;

            for (float layerDepth = 1; layerDepth < 10; layerDepth++)
            {
                if (_groundDrilledCount >= tile.EasyMiningLimit * layerDepth)
                {
                    miningRateCoef *= 0.1f;
                }
                else
                {
                    break;
                }
            }


            if (_emptyBoxCount >= 1)
            {
				_miningProgress += Time.deltaTime * _miningRateUps * miningRateCoef;
				
				transform.localPosition = new Vector3(transform.localPosition.x, 0.5f - 0.01f*(_miningProgress + _groundDrilledCount), transform.localPosition.z);
				tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, transform.localPosition.y - 0.5f, tile.transform.localPosition.z);
				
				if (_miningProgress >= 1f)
				{
					--_emptyBoxCount;
					++_fullBoxCount;
					
					++_groundDrilledCount;
					_miningProgress = 0f;
					
					if (InventoryChanged != null)
					{
						InventoryChanged();
					}
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
                UiPlayerDrillPanel.Instance.ShowPanel(new UiPlayerDrillPanel.PlayerDrillActions(), this);
            }
        }

        #endregion
	}
}
