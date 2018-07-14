using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Selection;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Terrain;
using Misner.PalmRTS.UI;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
	public class ConstructionBotActorBehavior : MonoBehaviour
    {
        #region Properties

        public ActorBehavior Actor
        {
            get
            {
                return GetComponent<ActorBehavior>();
            }
        }

        public Rigidbody Body
        {
            get
            {
                return GetComponent<Rigidbody>();
            }
        }

        public GameObject DrillStructurePrefab { get; set; }

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        protected void Start()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            playerTeam.AddClickEvent(Actor, ShowDeploymentPanel);
        }

		protected void Update ()
		{
            if (transform.localPosition.y < 0.95f)
            {
                Body.velocity += Vector3.up * 0.15f * Time.timeScale;
            }
            else if (transform.localPosition.y < 1.05f)
            {
                Body.velocity -= Vector3.up * 0.05f * Time.timeScale;
            }
            else
            {
                Body.velocity -= Vector3.up * 0.15f * Time.timeScale;
            }

            Body.velocity *= Mathf.Exp(-Time.deltaTime);
		}

        #endregion

        #region Events

        protected void ShowDeploymentPanel()
        {
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("<color=#ff0000>{0}.ShowDeploymentPanel(), PanelManager.Instance.IsAnyChildActive = {1}</color>", this.ToString(), PanelManager.Instance.IsAnyChildActive);
            }
            else
            {
				UiConstructionBotPanel.Instance.ShowPanel(
					new UiConstructionBotPanel.PlayerDeploymentActions() {
					DeployDrill = OnDeployDrill
				}
				);
            }
        }

        public class DrillDeploymentHandle
        {
            private readonly Action<DrillDeploymentHandle> _removeDeploymentHandle;
            private readonly Action<Misner.Utility.Math.IntVector2> _onCreateConstructionBot;
            private readonly List<SelectionTileItemBehavior> _selectionTiles = new List<SelectionTileItemBehavior>();

            public DrillDeploymentHandle(Action<DrillDeploymentHandle> removeDeplymentHandle, Action<Misner.Utility.Math.IntVector2> onCreateConstructionBot)
            {
				this._removeDeploymentHandle = removeDeplymentHandle;
                this._onCreateConstructionBot = onCreateConstructionBot;
            }

            public void AddSelectionTile(SelectionTileItemBehavior selectionTile)
            {
                _selectionTiles.Add(selectionTile);
            }

            public void OnSelectionPerformed(Vector2Int tileLocation)
            {
                Debug.LogFormat("<color=#000000>{0}.OnSelectionPerformed(), tileLocation = {1}</color>", this.ToString(), tileLocation);

                if (_onCreateConstructionBot != null)
                {
                    _onCreateConstructionBot(new Utility.Math.IntVector2(tileLocation.x, tileLocation.y));
                }

                DestroyAllSelectionTiles();
            }

            private void DestroyAllSelectionTiles()
            {
                foreach (SelectionTileItemBehavior selectionTile in _selectionTiles)
                {
                    SelectionTileParentBehavior.Instance.DestroyObject(selectionTile);
                }

                if (_removeDeploymentHandle != null)
                {
                    _removeDeploymentHandle(this);
                }
            }
        }

        private readonly List<DrillDeploymentHandle> _deploymentHandles = new List<DrillDeploymentHandle>();

        protected void OnDeployDrill()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            List<Vector2Int> availableTiles = playerTeam.GenerateAvailableStructureTiles();

            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDrill(), TODO setup some drill deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            DrillDeploymentHandle deploymentHandle = new DrillDeploymentHandle(RemoveDeploymentHandle, OnCreateConstructionBot);

            foreach (Vector2Int tileLocation in availableTiles)
            {
                SelectionTileItemBehavior selectionTile = SelectionTileParentBehavior.Instance.CreateObjectAt(tileLocation, deploymentHandle);

                deploymentHandle.AddSelectionTile(selectionTile);
            }

            _deploymentHandles.Add(deploymentHandle);
        }

        private void RemoveDeploymentHandle(DrillDeploymentHandle deploymentHandle)
        {
            _deploymentHandles.Remove(deploymentHandle);
        }

        protected void OnCreateConstructionBot(Utility.Math.IntVector2 tileLocation)
        {
            Debug.LogFormat("{0}.OnCreateConstructionBot(), tileLocation = {1}", this.ToString(), tileLocation);

            GameObject newDrillStructure = Instantiate(DrillStructurePrefab);
            newDrillStructure.transform.SetParent(this.transform.parent);
            newDrillStructure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newDrillStructure.transform.localScale = Vector3.one * 0.9f;

            ActorBehavior actor = newDrillStructure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }

        #endregion
	}
}
