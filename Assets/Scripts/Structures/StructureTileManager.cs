using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using UnityEngine;

namespace Misner.PalmRTS.Structure
{
	public class StructureTileManager
	{
        #region Variables

        private Dictionary<Vector2Int, ActorBehavior> _tileToActor = new Dictionary<Vector2Int, ActorBehavior>();

        #endregion

        #region Singleton

        private static StructureTileManager _instance = null;

        public static StructureTileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StructureTileManager();
                }

                return _instance;
            }
        }

        private StructureTileManager()
        {
        }

        #endregion

        #region Public Interface

        public bool Add(ActorBehavior actor)
        {
            Debug.LogFormat("<color=#ff00ff>{0}.Add(), actor.TilePosition = {1}</color>", this.ToString(), actor.TilePosition);

            Vector2Int tileKey = actor.TilePosition;

            if (_tileToActor.ContainsKey(tileKey))
            {
                return false;
            }

            _tileToActor[tileKey] = actor;

            return true;
        }

        public ActorBehavior GetActorAtTile(Vector2Int tileKey)
        {
            ActorBehavior result = null;

            if (_tileToActor.ContainsKey(tileKey))
            {
                result = _tileToActor[tileKey];
            }

            return result;
        }

        #endregion
	}
}
