using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Util;
using UnityEngine;

namespace Misner.PalmRTS.Terrain
{
	public class TerrainTileParentBehavior : MonoBehaviour
	{
        #region Variables

        private TerrainTileModel _terrainTileModel = null;

        #endregion

        #region SerializeField

		[SerializeField]
        private TerrainTileBehavior _defaultTilePrefab;

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            if (_defaultTilePrefab == null)
            {
                Debug.LogErrorFormat("{0}.Start(), _defaultTilePrefab must be defined! Please check your prefab by searching '{1}' and making sure 'Default Tile Prefab' is defined.", this.ToString(), "t:TerrainTileParentBehavior");
                return;
            }

            _terrainTileModel = new TerrainTileModel(this, _defaultTilePrefab);
            
            TerrainTileBehavior[] existingBehaviors = transform.GetComponentsInChildren<TerrainTileBehavior>();

            if (!existingBehaviors.IsNullOrEmpty())
            {
                Debug.LogFormat("{0}.Start(), existing tiles found! existingBehaviors.Length = {1}", this.ToString(), existingBehaviors.Length);
                
                foreach (TerrainTileBehavior childBehavior in existingBehaviors)
                {
                    _terrainTileModel.AddTile(childBehavior);
                }
            }

            _terrainTileModel.Populate(x0: -1, x1: 1, y0: -1, y1: 1);
        }

        // Update is called once per frame
		protected void Update ()
		{
		}

        #endregion
	}
}
