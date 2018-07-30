using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Util;
using Misner.Utility.Math;
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

        #region MonoBehaviour Singleton

        private static TerrainTileParentBehavior _instance = null;

        public static TerrainTileParentBehavior Instance
        {
            get
            {
                return _instance;
            }
        }

        // Use this for initialization
        protected void Awake()
        {
            _instance = this;
        }

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
                //Debug.LogFormat("{0}.Start(), existing tiles found! existingBehaviors.Length = {1}", this.ToString(), existingBehaviors.Length);
                
                foreach (TerrainTileBehavior childBehavior in existingBehaviors)
                {
                    _terrainTileModel.AddTile(childBehavior);
                }
            }

            int mapSize = 7;

            _terrainTileModel.Populate(x0: -mapSize, x1: mapSize, y0: -mapSize, y1: mapSize);
        }

        // Update is called once per frame
		protected void Update ()
		{
		}

        #endregion

        #region Public Methods

        public TerrainTileBehavior GetTile(Vector2Int gridIndex)
        {
            TerrainTileBehavior result = null;

            if (_terrainTileModel != null)
            {
                result = _terrainTileModel.GetTile(new IntVector2(gridIndex.x, gridIndex.y));
            }

            return result;
        }

        #endregion
	}
}
