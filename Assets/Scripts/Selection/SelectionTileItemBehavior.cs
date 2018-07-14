using System;
using UnityEngine;

namespace Misner.PalmRTS.Selection
{
	public class SelectionTileItemBehavior : MonoBehaviour
	{
        #region Types

        public interface IStructureDeploymentHandle
        {
            void OnSelectionPerformed(Vector2Int tileLocation);
        }

        #endregion

        #region Properties

        public IStructureDeploymentHandle DeploymentHandle { get; set; }
        public Vector2Int TileLocation { get; set; }

        #endregion

        #region MonoBehaviour

        protected void OnMouseUp ()
		{
            if (DeploymentHandle != null)
            {
                DeploymentHandle.OnSelectionPerformed(TileLocation);
            }
		}

        #endregion
	}
}
