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

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            _terrainTileModel = new TerrainTileModel(this);
            
            TerrainTileBehavior[] existingBehaviors = transform.GetComponentsInChildren<TerrainTileBehavior>();

            if (!existingBehaviors.IsNullOrEmpty())
            {
                Debug.LogFormat("{0}.Start(), existing tiles found! existingBehaviors.Length = {1}", this.ToString(), existingBehaviors.Length);
                
                foreach (TerrainTileBehavior childBehavior in existingBehaviors)
                {
                    _terrainTileModel.AddTile(childBehavior);
                }
            }
        }

        // Update is called once per frame
		protected void Update ()
		{
		}

        #endregion
	}
}
