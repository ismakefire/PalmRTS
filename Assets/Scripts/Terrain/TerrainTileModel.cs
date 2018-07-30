using System;
using System.Collections.Generic;
using Misner.PalmRTS.Util;
using Misner.Utility.Math;
using UnityEngine;

namespace Misner.PalmRTS.Terrain
{
	public class TerrainTileModel
	{
        #region Variables

        private readonly TerrainTileParentBehavior _parentBehavior;
        private readonly TerrainTileBehavior _defaultTilePrefab;

        private readonly Dictionary<IntVector2, TerrainTileBehavior> _gridToTile = new Dictionary<IntVector2, TerrainTileBehavior>();

        #endregion

        #region Public Methods

        public TerrainTileModel(TerrainTileParentBehavior terrainTileParentBehavior, TerrainTileBehavior defaultTilePrefab)
		{
    		_parentBehavior = terrainTileParentBehavior;
            _defaultTilePrefab = defaultTilePrefab;
		}

        public void AddTile(TerrainTileBehavior tileBehavior)
        {
            if (_gridToTile.ContainsKey(tileBehavior.GridPosition))
            {
                Debug.LogErrorFormat("{0}.AddTile(), failed to add tile. Tile already exists at position. tileBehavior.GridPosition = {1}", this.ToString(), tileBehavior.GridPosition.ToString());
                return;
            }

			// Reparent
			tileBehavior.transform.SetParent(_parentBehavior.transform);

            // Add to grid mapping.
            _gridToTile[tileBehavior.GridPosition] = tileBehavior;
        }

		public void Populate(int x0, int x1, int y0, int y1)
        {
            for (int indexX = x0; indexX <= x1; indexX++)
            {
                for (int indexY = y0; indexY <= y1; indexY++)
                {
                    IntVector2 gridIndex = new IntVector2(indexX, indexY);

                    if (!_gridToTile.ContainsKey(gridIndex))
                    {
                        PopulateInternal(gridIndex);
                    }
                }
            }
        }

        public TerrainTileBehavior GetTile(IntVector2 gridIndex)
        {
            TerrainTileBehavior result = null;

			if (_gridToTile.ContainsKey(gridIndex))
			{
                result = _gridToTile[gridIndex];
			}

            return result;
        }

        #endregion

        #region Private Methods

        private void PopulateInternal(IntVector2 gridIndex)
        {
            TerrainTileBehavior tileBehavior = UnityEngine.Object.Instantiate<TerrainTileBehavior>(_defaultTilePrefab);
            tileBehavior.name = string.Format("{0} {1}", tileBehavior.name.SubStringFromEnd("(Clone)".Length), gridIndex.ToString());

            // Reparent and position
            tileBehavior.transform.SetParent(_parentBehavior.transform);
            tileBehavior.transform.localPosition = TerrainTileBehavior.WorldPositionFromGrid(gridIndex) + Vector3.up * (0.04f * UnityEngine.Random.Range(-1, 1));

            AddTile(tileBehavior);
        }

        #endregion
    }
}
