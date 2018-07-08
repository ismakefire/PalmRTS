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

        public void CreateObjectAt(Vector2Int tileLocation, Action<Vector2Int> onSelectionPerformed)
        {
            SelectionTileItemBehavior newSelectionObject = UnityEngine.Object.Instantiate<SelectionTileItemBehavior>(_selectionObjectPrefab);

            newSelectionObject.transform.SetParent(this.transform);
            newSelectionObject.transform.localPosition = new Vector3(tileLocation.x, 0, tileLocation.y);
            newSelectionObject.transform.localScale = Vector3.one;

            newSelectionObject.OnItemClicked += () =>
            {
                Debug.LogFormat("<color=#00ff00>{0}.CreateObjectAt() -> delegate, tileLocation = {1}</color>", this.ToString(), tileLocation);
            };

            Debug.LogFormat("<color=#ff00ff>{0}.CreateObjectAt(), tileLocation = {1}</color>", this.ToString(), tileLocation);
        }

        #endregion
	}
}
