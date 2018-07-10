using System;
using UnityEngine;

namespace Misner.PalmRTS.Selection
{
	public class SelectionTileItemBehavior : MonoBehaviour
	{
        #region Properties

        public Player.ConstructionBotActorBehavior.DrillDeploymentHandle DeploymentHandle { get; set; }
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
