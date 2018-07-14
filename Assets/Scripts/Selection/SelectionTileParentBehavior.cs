using System;
using UnityEngine;

namespace Misner.PalmRTS.Selection
{
	public class SelectionTileParentBehavior : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private SelectionTileItemBehavior _selectionObjectPrefab;

        #endregion

        #region MonoBehaviour Singleton

        private static SelectionTileParentBehavior _instance = null;

        public static SelectionTileParentBehavior Instance
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

        #region Public Interface

        public SelectionTileItemBehavior CreateObjectAt(Vector2Int tileLocation, SelectionTileItemBehavior.IStructureDeploymentHandle deploymentHandle)
        {
            SelectionTileItemBehavior newSelectionObject = UnityEngine.Object.Instantiate<SelectionTileItemBehavior>(_selectionObjectPrefab);

            newSelectionObject.transform.SetParent(this.transform);
            newSelectionObject.transform.localPosition = new Vector3(tileLocation.x, 0, tileLocation.y);
            newSelectionObject.transform.localScale = Vector3.one;

            newSelectionObject.DeploymentHandle = deploymentHandle;
            newSelectionObject.TileLocation = tileLocation;

            return newSelectionObject;
        }

        public void DestroyObject(SelectionTileItemBehavior selectionTile)
        {
            selectionTile.DeploymentHandle = null;

            UnityEngine.Object.DestroyObject(selectionTile.gameObject);
        }

        #endregion
	}
}
