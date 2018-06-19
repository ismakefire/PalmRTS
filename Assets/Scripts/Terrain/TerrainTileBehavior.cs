using System.Collections;
using System.Collections.Generic;
using Misner.Utility.Math;
using UnityEngine;

namespace Misner.PalmRTS.Terrain
{
	public class TerrainTileBehavior : MonoBehaviour
	{
        #region Constants

        public const float GridWidth = 8f;

        #endregion
        
        #region Properties

        public IntVector2 GridPosition
        {
            get
            {
                return GridPositionFromWorld(transform.localPosition);
            }
        }

        #endregion

        #region Static Methods

        public static IntVector2 GridPositionFromWorld(Vector3 worldPosition)
        {
            IntVector2 gridPosition = new IntVector2(
                x: Mathf.RoundToInt(worldPosition.x / GridWidth),
                y: Mathf.RoundToInt(worldPosition.z / GridWidth)
            );
            
            return gridPosition;
        }

        public static Vector3 WorldPositionFromGrid(IntVector2 gridPosition)
        {
            Vector3 worldPosition = new Vector3(
                x: GridWidth * (float)gridPosition.x,
                y: 0f,
                z: GridWidth * (float)gridPosition.y
            );

            return worldPosition;
        }

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
		}

		// Update is called once per frame
		protected void Update ()
		{
		}

        #endregion
	}
}
