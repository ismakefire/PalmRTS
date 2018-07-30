using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Player;
using Misner.PalmRTS.Structure;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public class TakeFromTransitOrder : ITransitOrder
    {
        #region Private Variables

        readonly ITransitActor _transitActor;

        #endregion

        #region Constructor

        public TakeFromTransitOrder(ITransitActor transitActor)
        {
            _transitActor = transitActor;
        }

        #endregion

        #region ITransitOrder

        public int? Object { get; set; }

        public int? Subject { get; set; }

        public float? Duration
        {
            get
            {
                return null;
            }
        }

        public void CompleteTransaction()
        {
            Vector2Int? offset = this.GetOffsetFromSubject();

            if (offset == null)
            {
                Debug.LogFormat("<color=#ff00ff>{0}.CompleteTransaction(), no valid direction.</color>", this.ToString());
            }
            else
            {
                ActorBehavior result = StructureTileManager.Instance.GetActorAtTile(_transitActor.Actor.TilePosition + offset.Value);

                if (result == null)
                {
                    Debug.LogFormat("<color=#ff00ff>{0}.CompleteTransaction(), no valid target.</color>", this.ToString());
                }
                else
                {
                    IInventoryStructure inventoryStructure = result.gameObject.GetComponent<IInventoryStructure>();
                    //Debug.LogFormat("<color=#ff00ff>{0}.CompleteTransaction(), inventoryStructure = {1}</color>", this.ToString(), inventoryStructure.ToString());

                    if (inventoryStructure == null)
                    {
                        Debug.LogFormat("<color=#ff00ff>{0}.CompleteTransaction(), there isn't an inventory structure there.</color>", this.ToString());
                    }
                    else
                    {
                        switch (Object)
                        {
                            case 1:
                                if (inventoryStructure.Inventory_EmptyBoxCount > 0)
                                {
                                    int transactionAmount = 1;

                                    inventoryStructure.Inventory_EmptyBoxCount -= transactionAmount;
                                    _transitActor.Inventory_EmptyBoxCount += transactionAmount;
                                }
                                break;

                            case 2:
                                if (inventoryStructure.Inventory_DrillProductCount > 0)
                                {
                                    int transactionAmount = 1;

                                    inventoryStructure.Inventory_DrillProductCount -= transactionAmount;
                                    _transitActor.Inventory_DrillProductCount += transactionAmount;
                                }
                                break;
                            
                            default:
                                break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
