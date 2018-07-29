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

        readonly TransitDepotStructureActor _transitDepot;

        #endregion

        #region Constructor

        public TakeFromTransitOrder(TransitDepotStructureActor transitDepot)
        {
            _transitDepot = transitDepot;
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
                ActorBehavior result = StructureTileManager.Instance.GetActorAtTile(_transitDepot.Actor.TilePosition + offset.Value);

                if (result == null)
                {
                    Debug.LogFormat("<color=#ff00ff>{0}.CompleteTransaction(), no valid target.</color>", this.ToString());
                }
                else
                {
                    DrillStructureBehavior drill = result.gameObject.GetComponent<DrillStructureBehavior>();

                    if (drill == null)
                    {
                        Debug.LogFormat("<color=#ff00ff>{0}.CompleteTransaction(), it's not a drill.</color>", this.ToString());
                    }
                    else
                    {
                        switch (Object)
                        {
                            case 1:
                                if (drill.EmptyBoxCount > 0)
                                {
                                    int transactionAmount = 1;

                                    drill.EmptyBoxCount -= transactionAmount;
                                    _transitDepot.EmptyBoxCount += transactionAmount;
                                }
                                break;

                            case 2:
                                if (drill.FullBoxCount > 0)
                                {
                                    int transactionAmount = 1;

                                    drill.FullBoxCount -= transactionAmount;
                                    _transitDepot.DrillProductCount += transactionAmount;
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
