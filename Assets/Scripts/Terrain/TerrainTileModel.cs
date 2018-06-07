
using UnityEngine;

namespace Misner.PalmRTS.Terrain
{
	public class TerrainTileModel
	{
        #region Variables

        private readonly TerrainTileParentBehavior _parentBehavior;

        #endregion

        #region Public Methods

		public TerrainTileModel(TerrainTileParentBehavior terrainTileParentBehavior)
		{
    		_parentBehavior = terrainTileParentBehavior;

            // TODO
		}

        public void AddTile(TerrainTileBehavior tileBehavior)
        {
            // Reparent, if we need to.
            if (tileBehavior.transform.parent != _parentBehavior.transform)
            {
                Debug.LogFormat("{0}.AddTile(), reparenting tile.", this.ToString());
                tileBehavior.transform.SetParent(_parentBehavior.transform);
            }

            // TODO
        }

        #endregion
    }
}
