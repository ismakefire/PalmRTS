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

        // TODO: Maybe get these from a Singleton?
        public GameObject DrillStructurePrefab { get; set; }

        public GameObject DepotStructurePrefab { get; set; }

        public GameObject MachineFactoryStructurePrefab { get; set; }

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
					DeployDrill = OnDeployDrill,
                    DeployDepot = OnDeployDepot,
                    DeployMachineFactory = OnDeployMachineFactory
				}
				);
            }
        }




        public class DrillDeploymentHandle : SelectionTileItemBehavior.IStructureDeploymentHandle
        {
            private readonly Action<DrillDeploymentHandle> _removeDeploymentHandle;
            private readonly Action<Misner.Utility.Math.IntVector2> _onCreateDrillStructure ;
            private readonly List<SelectionTileItemBehavior> _selectionTiles = new List<SelectionTileItemBehavior>();

            public DrillDeploymentHandle(Action<DrillDeploymentHandle> removeDeplymentHandle, Action<Misner.Utility.Math.IntVector2> onCreateDrillStructure)
            {
				this._removeDeploymentHandle = removeDeplymentHandle;
                this._onCreateDrillStructure = onCreateDrillStructure;
            }

            public void AddSelectionTile(SelectionTileItemBehavior selectionTile)
            {
                _selectionTiles.Add(selectionTile);
            }

            #region SelectionTileItemBehavior.IStructureDeploymentHandle

			public void OnSelectionPerformed(Vector2Int tileLocation)
			{
				Debug.LogFormat("<color=#000000>{0}.OnSelectionPerformed(), tileLocation = {1}</color>", this.ToString(), tileLocation);
				
				if (_onCreateDrillStructure  != null)
				{
					_onCreateDrillStructure (new Utility.Math.IntVector2(tileLocation.x, tileLocation.y));
				}
				
				DestroyAllSelectionTiles();
			}

            #endregion

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

        private readonly List<DrillDeploymentHandle> _drillDeploymentHandles = new List<DrillDeploymentHandle>();

        protected void OnDeployDrill()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            List<Vector2Int> availableTiles = playerTeam.GenerateAvailableStructureTiles();

            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDrill(), TODO setup some drill deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            DrillDeploymentHandle deploymentHandle = new DrillDeploymentHandle(RemoveDrillDeploymentHandle, OnCreateDrillStructure);

            foreach (Vector2Int tileLocation in availableTiles)
            {
                SelectionTileItemBehavior selectionTile = SelectionTileParentBehavior.Instance.CreateObjectAt(tileLocation, deploymentHandle);

                deploymentHandle.AddSelectionTile(selectionTile);
            }

            _drillDeploymentHandles.Add(deploymentHandle);
        }

        private void RemoveDrillDeploymentHandle(DrillDeploymentHandle deploymentHandle)
        {
            _drillDeploymentHandles.Remove(deploymentHandle);
        }

        protected void OnCreateDrillStructure(Utility.Math.IntVector2 tileLocation)
        {
            Debug.LogFormat("{0}.OnCreateDrillStructure(), tileLocation = {1}", this.ToString(), tileLocation);

            GameObject newDrillStructure = Instantiate(DrillStructurePrefab);
            newDrillStructure.transform.SetParent(this.transform.parent);
            newDrillStructure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newDrillStructure.transform.localScale = Vector3.one * 0.9f;

            ActorBehavior actor = newDrillStructure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }







        public class Depot_DeploymentHandle : SelectionTileItemBehavior.IStructureDeploymentHandle
        {
            private readonly Action<Depot_DeploymentHandle> _removeDeploymentHandle;
            private readonly Action<Misner.Utility.Math.IntVector2> _onCreateDepot_Structure;
            private readonly List<SelectionTileItemBehavior> _selectionTiles = new List<SelectionTileItemBehavior>();

            public Depot_DeploymentHandle(Action<Depot_DeploymentHandle> removeDeplymentHandle, Action<Misner.Utility.Math.IntVector2> onCreateDepot_Structure)
            {
                this._removeDeploymentHandle = removeDeplymentHandle;
                this._onCreateDepot_Structure = onCreateDepot_Structure;
            }

            public void AddSelectionTile(SelectionTileItemBehavior selectionTile)
            {
                _selectionTiles.Add(selectionTile);
            }

            #region SelectionTileItemBehavior.IStructureDeploymentHandle

            public void OnSelectionPerformed(Vector2Int tileLocation)
            {
                Debug.LogFormat("<color=#000000>{0}.OnSelectionPerformed(), tileLocation = {1}</color>", this.ToString(), tileLocation);

                if (_onCreateDepot_Structure != null)
                {
                    _onCreateDepot_Structure(new Utility.Math.IntVector2(tileLocation.x, tileLocation.y));
                }

                DestroyAllSelectionTiles();
            }

            #endregion

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

        private readonly List<Depot_DeploymentHandle> _depot_DeploymentHandle = new List<Depot_DeploymentHandle>();

        protected void OnDeployDepot()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            List<Vector2Int> availableTiles = playerTeam.GenerateAvailableStructureTiles();

            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDepot(), TODO setup some depot deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            Depot_DeploymentHandle deploymentHandle = new Depot_DeploymentHandle(RemoveDepot_DeploymentHandle, OnCreateDepot_Structure);

            foreach (Vector2Int tileLocation in availableTiles)
            {
                SelectionTileItemBehavior selectionTile = SelectionTileParentBehavior.Instance.CreateObjectAt(tileLocation, deploymentHandle);

                deploymentHandle.AddSelectionTile(selectionTile);
            }

            _depot_DeploymentHandle.Add(deploymentHandle);
        }

        protected void RemoveDepot_DeploymentHandle(Depot_DeploymentHandle depot_DeploymentHandle)
        {
            _depot_DeploymentHandle.Remove(depot_DeploymentHandle);
        }

        protected void OnCreateDepot_Structure(Utility.Math.IntVector2 tileLocation)
        {
            Debug.LogFormat("{0}.OnCreateDepot_Structure(), tileLocation = {1}", this.ToString(), tileLocation);

            GameObject newDepot_Structure = Instantiate(DepotStructurePrefab);
            newDepot_Structure.transform.SetParent(this.transform.parent);
            newDepot_Structure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newDepot_Structure.transform.localScale = Vector3.one * 0.9f;

            ActorBehavior actor = newDepot_Structure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }









        public class MachineFactory_DeploymentHandle : SelectionTileItemBehavior.IStructureDeploymentHandle
        {
            private readonly Action<MachineFactory_DeploymentHandle> _removeDeploymentHandle;
            private readonly Action<Misner.Utility.Math.IntVector2> _onCreateMachineFactory_Structure;
            private readonly List<SelectionTileItemBehavior> _selectionTiles = new List<SelectionTileItemBehavior>();

            public MachineFactory_DeploymentHandle(Action<MachineFactory_DeploymentHandle> removeDeplymentHandle, Action<Misner.Utility.Math.IntVector2> onCreateMachineFactory_Structure)
            {
                this._removeDeploymentHandle = removeDeplymentHandle;
                this._onCreateMachineFactory_Structure = onCreateMachineFactory_Structure;
            }

            public void AddSelectionTile(SelectionTileItemBehavior selectionTile)
            {
                _selectionTiles.Add(selectionTile);
            }

            #region SelectionTileItemBehavior.IStructureDeploymentHandle

            public void OnSelectionPerformed(Vector2Int tileLocation)
            {
                Debug.LogFormat("<color=#000000>{0}.OnSelectionPerformed(), tileLocation = {1}</color>", this.ToString(), tileLocation);

                if (_onCreateMachineFactory_Structure != null)
                {
                    _onCreateMachineFactory_Structure(new Utility.Math.IntVector2(tileLocation.x, tileLocation.y));
                }

                DestroyAllSelectionTiles();
            }

            #endregion

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

        private readonly List<MachineFactory_DeploymentHandle> _machineFactory_DeploymentHandle = new List<MachineFactory_DeploymentHandle>();

        protected void OnDeployMachineFactory()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            List<Vector2Int> availableTiles = playerTeam.GenerateAvailableStructureTiles();

            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployMachineFactory(), TODO setup some machine factory deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            MachineFactory_DeploymentHandle deploymentHandle = new MachineFactory_DeploymentHandle(RemoveMachineFactory_DeploymentHandle, OnCreateMachineFactory_Structure);

            foreach (Vector2Int tileLocation in availableTiles)
            {
                SelectionTileItemBehavior selectionTile = SelectionTileParentBehavior.Instance.CreateObjectAt(tileLocation, deploymentHandle);

                deploymentHandle.AddSelectionTile(selectionTile);
            }

            _machineFactory_DeploymentHandle.Add(deploymentHandle);
        }

        protected void RemoveMachineFactory_DeploymentHandle(MachineFactory_DeploymentHandle machineFactory_DeploymentHandle)
        {
            _machineFactory_DeploymentHandle.Remove(machineFactory_DeploymentHandle);
        }

        protected void OnCreateMachineFactory_Structure(Utility.Math.IntVector2 tileLocation)
        {
            Debug.LogFormat("{0}.OnCreateMachineFactory_Structure(), tileLocation = {1}", this.ToString(), tileLocation);

            GameObject newMachineFactory_Structure = Instantiate(MachineFactoryStructurePrefab);
            newMachineFactory_Structure.transform.SetParent(this.transform.parent);
            newMachineFactory_Structure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newMachineFactory_Structure.transform.localScale = Vector3.one * 0.9f;

            ActorBehavior actor = newMachineFactory_Structure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }




        #endregion
	}
}
