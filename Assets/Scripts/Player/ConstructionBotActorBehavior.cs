using System;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Selection;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Terrain;
using Misner.PalmRTS.UI;
using Misner.Utility.Math;
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




        public class StructureDeploymentHandle : SelectionTileItemBehavior.IStructureDeploymentHandle
        {
            private readonly Action<StructureDeploymentHandle> _removeDeploymentHandle;
            private readonly Action<Misner.Utility.Math.IntVector2> _onDeployStructure;
            private readonly List<SelectionTileItemBehavior> _selectionTiles = new List<SelectionTileItemBehavior>();

            public StructureDeploymentHandle(Action<StructureDeploymentHandle> removeDeplymentHandle, Action<Misner.Utility.Math.IntVector2> onDeployStructure)
            {
				this._removeDeploymentHandle = removeDeplymentHandle;
                this._onDeployStructure = onDeployStructure;
            }

            public void ApplyToTilesLocations(List<Vector2Int> availableTiles)
            {
                foreach (Vector2Int tileLocation in availableTiles)
                {
                    SelectionTileItemBehavior selectionTile = SelectionTileParentBehavior.Instance.CreateObjectAt(tileLocation, this);
                    
                    AddSelectionTile(selectionTile);
                }
            }

            public void AddSelectionTile(SelectionTileItemBehavior selectionTile)
            {
                _selectionTiles.Add(selectionTile);
            }

            #region SelectionTileItemBehavior.IStructureDeploymentHandle

			public void OnSelectionPerformed(Vector2Int tileLocation)
			{
				Debug.LogFormat("<color=#000000>{0}.OnSelectionPerformed(), tileLocation = {1}</color>", this.ToString(), tileLocation);
				
				if (_onDeployStructure  != null)
				{
					_onDeployStructure (new Utility.Math.IntVector2(tileLocation.x, tileLocation.y));
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








        private readonly List<StructureDeploymentHandle> _deploymentHandles = new List<StructureDeploymentHandle>();

        private List<Vector2Int> GetAvailableTiles()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            List<Vector2Int> availableTiles = playerTeam.GenerateAvailableStructureTiles();

            return availableTiles;
        }

        private void RemoveDeploymentHandle(StructureDeploymentHandle deploymentHandle)
        {
            _deploymentHandles.Remove(deploymentHandle);
        }

        private void BeginStructureDeployment(Action<IntVector2> onDeploymentSelected)
        {
            List<Vector2Int> availableTiles = GetAvailableTiles();

            StructureDeploymentHandle deploymentHandle = new StructureDeploymentHandle(RemoveDeploymentHandle, onDeploymentSelected);

            deploymentHandle.ApplyToTilesLocations(availableTiles);

            _deploymentHandles.Add(deploymentHandle);
        }





        protected void OnDeployDrill()
        {
            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDrill(), TODO setup some drill deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            BeginStructureDeployment(OnCreateDrillStructure);
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







        protected void OnDeployDepot()
        {
            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDepot(), TODO setup some depot deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            BeginStructureDeployment(OnCreateDepot_Structure);
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









        protected void OnDeployMachineFactory()
        {
            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployMachineFactory(), TODO setup some machine factory deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            BeginStructureDeployment(OnCreateMachineFactory_Structure);
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
