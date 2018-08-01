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

        private readonly ResourceCollection _currentResources = new ResourceCollection();

        private float _groundDrilledCount = 0f;

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
                return _currentResources;
            }
        }

        public int Inventory_EmptyBoxCount
        {
            get
            {
                return _currentResources.Get(EResourceItem.MetalBox);
            }
            set
            {
                _currentResources.Set(EResourceItem.MetalBox, value);
            }
        }

        public int Inventory_DrillProductCount
        {
            get
            {
                return _currentResources.Get(EResourceItem.SolidRock);
            }
            set
            {
                _currentResources.Set(EResourceItem.SolidRock, value);
            }
        }

        public event Action InventoryChanged;

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
        {
			_currentResources.Changed += OnInventoryChanged;
            OurTeam.AddClickEvent(Actor, ShowDrillPanel);

            _currentResources.Add(EResourceItem.MetalBox, 15);

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


            if (Resources.Has(EResourceItem.MetalBox))
            {
				_miningProgress += Time.deltaTime * _miningRateUps * miningRateCoef;
				
				transform.localPosition = new Vector3(transform.localPosition.x, 0.5f - 0.01f*(_miningProgress + _groundDrilledCount), transform.localPosition.z);
				tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, transform.localPosition.y - 0.5f, tile.transform.localPosition.z);
				
				if (_miningProgress >= 1f)
				{
                    if (Resources.Remove(EResourceItem.MetalBox, 1))
                    {
                        Resources.Add(EResourceItem.SolidRock, 1);

                        ++_groundDrilledCount;
                        _miningProgress = 0f;
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

        protected void OnInventoryChanged()
        {
            if (InventoryChanged != null)
            {
                InventoryChanged();
            }
        }

        #endregion
	}
}
