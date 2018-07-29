using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Player;
using Misner.PalmRTS.Structure;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public class InsertIntoTransitOrder : ITransitOrder
    {
        #region Private Variables

        readonly TransitDepotStructureActor _transitDepot;

        #endregion

        #region Constructor

        public InsertIntoTransitOrder(TransitDepotStructureActor transitDepot)
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
                                if (_transitDepot.EmptyBoxCount > 0)
                                {
                                    int transactionAmount = 1;

                                    _transitDepot.EmptyBoxCount -= transactionAmount;
									drill.EmptyBoxCount += transactionAmount;
                                }
                                break;

                            case 2:
                                if (_transitDepot.DrillProductCount > 0)
                                {
                                    int transactionAmount = 1;

                                    _transitDepot.DrillProductCount -= transactionAmount;
									drill.FullBoxCount += transactionAmount;
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
