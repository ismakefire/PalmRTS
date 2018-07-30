using System.Collections;
using System.Collections.Generic;
using Misner.Utility.Math;
using UnityEngine;

namespace Misner.PalmRTS.Terrain
{
	public class TerrainTileBehavior : MonoBehaviour
	{
        #region Constants

        public const float GridWidth = 2f;

        #endregion

        #region SerializeField

        [SerializeField]
        private Material _darkerMaterial;

        [SerializeField]
        private Material _lighterMaterial;

        [SerializeField]
        private MeshRenderer _meshRenderer;

        #endregion

        #region Properties

        public IntVector2 GridPosition
        {
            get
            {
                return GridPositionFromWorld(transform.localPosition);
            }
        }

        public float EasyMiningLimit { get; set; }

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
            if (Random.Range(0f, 1f) > 0.5f)
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    _meshRenderer.material = _darkerMaterial;
                }
                else
                {
                    _meshRenderer.material = _lighterMaterial;
                }
            }

            float simpleBellCurve = 0.5f * (Random.Range(-1f, 1f) + Random.Range(-1f, 1f));

            EasyMiningLimit = 50f + 15f * simpleBellCurve;
        }

		// Update is called once per frame
		protected void Update ()
		{
		}

        #endregion
	}
}
