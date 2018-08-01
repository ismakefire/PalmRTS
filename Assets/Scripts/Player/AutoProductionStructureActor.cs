using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Resource;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Transit;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
    public class AutoProductionStructureActor : MonoBehaviour, IInventoryStructure
    {
        #region Variables

        private readonly ResourceCollection _currentResources = new ResourceCollection();

        private float _productionProgress = 0f;

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

        public float ProductionProgress
        {
            get
            {
                return _productionProgress;
            }
        }

        public int ConsumedResourceAmount
        {
            get
            {
                return _currentResources.Get(_consumedResource);
            }
            set
            {
                _currentResources.Set(_consumedResource, value);
            }
        }

        public int ProducedResourceAmount
        {
            get
            {
                return _currentResources.Get(_producedResource);
            }
            set
            {
                _currentResources.Set(_producedResource, value);
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
                return _currentResources.Get(EResourceItem.SolidRock);
            }
            set
            {
                _currentResources.Set(EResourceItem.SolidRock, value);

                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }
            }
        }

        public event Action InventoryChanged;

        #endregion

        #region SerializeField

        [SerializeField]
        private EResourceItem _consumedResource;

        [SerializeField]
        private EResourceItem _producedResource;

        [SerializeField]
        private float _productionRateUPS;

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        protected void Start ()
        {
            OurTeam.AddClickEvent(Actor, ShowPanel);

            StructureTileManager.Instance.Add(Actor);

            foreach (EResourceItem item in ResourceItemUtil.GetAll())
            {
                _currentResources.Set(item, 3);
            }
        }

        protected void Update()
        {
            float miningRateCoef = 1.0f;



            if (ConsumedResourceAmount >= 1)
            {
                _productionProgress += Time.deltaTime * _productionRateUPS * miningRateCoef;

                if (_productionProgress >= 1f)
                {
                    --ConsumedResourceAmount;
                    ++ProducedResourceAmount;

                    _productionProgress = 0f;

                    if (InventoryChanged != null)
                    {
                        InventoryChanged();
                    }
                }
            }
        }

        #endregion

        #region UI Events

        protected void ShowPanel()
        {
			UiPlayerAutoProductionStructurePanel.Instance.ShowPanel(
                new UiPlayerAutoProductionStructurePanel.PlayerStructureActions(),
				this
			);
        }

        #endregion

	}
}
