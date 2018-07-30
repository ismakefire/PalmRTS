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
        #region Types

        public enum State
        {
            Undefined,

			Idle,

            AwaitingInput,
            FlyingToLocation,
            DeployingStructure,
		}
		
		#endregion

		#region SerializeField

        [SerializeField]
        private GameObject CrusherStructurePrefab;

        [SerializeField]
        private GameObject SmelterStructurePrefab;

        [SerializeField]
        private GameObject FabricatorStructurePrefab;

        #endregion

        #region Properties

        public ActorBehavior Actor
        {
            get
            {
                return GetComponent<ActorBehavior>();
            }
        }

        public PlayerTeam OurTeam
        {
            get
            {
                PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);

                return playerTeam;
            }
        }

        public Rigidbody Body
        {
            get
            {
                return GetComponent<Rigidbody>();
            }
        }

        public State CurrentState { get; private set; }

        // TODO: Maybe get these from a Singleton?
        public GameObject DrillStructurePrefab { get; set; }

        public GameObject DepotStructurePrefab { get; set; }

        public GameObject MachineFactoryStructurePrefab { get; set; }

        public GameObject ConnectorStructurePrefab { get; set; }

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        protected void Start()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            playerTeam.AddClickEvent(Actor, ShowDeploymentPanel);

            CurrentState = State.Idle;
        }

        private Vector3? _flightTarget = null;
        private Action _onFlightComplete = null;

		private void ApplyVerticalFloat ()
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
        }

		protected void Update ()
		{
            switch (CurrentState)
            {
                case State.Idle:
                case State.AwaitingInput:
                    {
						ApplyVerticalFloat();
                        
						Body.velocity *= Mathf.Exp(-Time.deltaTime);
                    }
                    break;

                case State.FlyingToLocation:
                    if (_flightTarget != null)
                    {
                        Vector3 displacement = _flightTarget.Value - transform.localPosition;

                        float startingDot = Vector3.Dot(displacement, Body.velocity);

                        float burstAccelerationUsedMps2 = 1.0f;
                        float fluidAccelerationUsedMps2 = 0.08f;

                        float detectionRadius = 1.5f;
                        if (displacement.sqrMagnitude > 5f * detectionRadius)
                        {
                            if (UnityEngine.Random.Range(0, 8) == 0 || Body.velocity.magnitude < 1f)
                            {
								Body.velocity += displacement.normalized * burstAccelerationUsedMps2 * Time.timeScale;
                            }
                        }
                        else if (displacement.sqrMagnitude > 3f * detectionRadius)
                        {
                            Body.velocity += displacement.normalized * 3f * fluidAccelerationUsedMps2 * Time.timeScale;
                        }
                        else if (displacement.sqrMagnitude > detectionRadius)
                        {
                            Body.velocity += displacement.normalized * fluidAccelerationUsedMps2 * Time.timeScale;
                        }
                        else
                        {
                            CurrentState = State.Idle;

                            if (_onFlightComplete != null)
                            {
                                _onFlightComplete();
                                _onFlightComplete = null;
                            }
                        }

                        ApplyVerticalFloat();

                        if (Body.velocity.magnitude > 1f && startingDot < 0f)
                        {
                            Body.velocity *= Mathf.Exp(-Time.deltaTime * 4f);
                        }
                        else
                        {
                            Body.velocity *= Mathf.Exp(-Time.deltaTime);
                        }
                    }
                    break;
            }
		}

        protected void FlyToLocation(Vector2Int tileLocation, Action completeFlight)
        {
            if (CurrentState != State.AwaitingInput)
            {
                Debug.LogFormat("<color=#ff00ff>{0}.FlyToLocation(), bot not ready for input.</color>", this.ToString());
            }
            else
            {
				_flightTarget = new Vector3(tileLocation.x, 0f, tileLocation.y);
                _onFlightComplete = completeFlight;

                CurrentState = State.FlyingToLocation;
            }
        }

        #endregion

        #region Events

        protected void ShowDeploymentPanel()
        {
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("<color=#ff0000>{0}.ShowDeploymentPanel(), PanelManager.Instance.IsAnyChildActive = {1}</color>", this.ToString(), PanelManager.Instance.IsAnyChildActive);
            }
            else if (CurrentState == State.Idle)
            {
				UiConstructionBotPanel.Instance.ShowPanel(
					new UiConstructionBotPanel.PlayerDeploymentActions() {
    					DeployDrill = OnDeployDrill,
    					DeployDepot = OnDeployDepot,
    					DeployMachineFactory = OnDeployMachineFactory,
                        DeployConnector = OnDeployConnector,
                        DeployCrusher = OnDeployCrusher,
                        DeploySmelter = OnDeploySmelter,
                        DeployFabricator = OnDeployFabricator
    				}
				);
            }
        }




        public class StructureDeploymentHandle : SelectionTileItemBehavior.IStructureDeploymentHandle
        {
            private readonly ConstructionBotActorBehavior _constructionBot;
            private readonly Action<StructureDeploymentHandle> _removeDeploymentHandle;
            private readonly Action<Misner.Utility.Math.IntVector2> _onDeployStructure;
            private readonly List<SelectionTileItemBehavior> _selectionTiles = new List<SelectionTileItemBehavior>();

            private Vector2Int? _tileLocation = null;

            public StructureDeploymentHandle(ConstructionBotActorBehavior constructionBot, Action<StructureDeploymentHandle> removeDeplymentHandle, Action<Misner.Utility.Math.IntVector2> onDeployStructure)
            {
                this._constructionBot = constructionBot;
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
                //Debug.LogFormat("<color=#000000>{0}.OnSelectionPerformed(), tileLocation = {1}, _constructionBot.CurrentState = {2}</color>", this.ToString(), tileLocation, _constructionBot.CurrentState);
                
                if (_constructionBot.CurrentState == State.AwaitingInput)
                {
                    _tileLocation = tileLocation;

                    _constructionBot.FlyToLocation(tileLocation, CompleteFlight);

                    DestroyAllSelectionTiles();
                }
			}

            #endregion

            private void DestroyAllSelectionTiles()
            {
                foreach (SelectionTileItemBehavior selectionTile in _selectionTiles)
                {
                    SelectionTileParentBehavior.Instance.DestroyObject(selectionTile);
                }
            }

            protected void CompleteFlight()
            {
                if (_onDeployStructure != null && _tileLocation != null)
                {
                    _onDeployStructure (new Utility.Math.IntVector2(_tileLocation.Value.x, _tileLocation.Value.y));
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
            CurrentState = State.AwaitingInput;
            
            List<Vector2Int> availableTiles = GetAvailableTiles();

            StructureDeploymentHandle deploymentHandle = new StructureDeploymentHandle(this, RemoveDeploymentHandle, onDeploymentSelected);

            deploymentHandle.ApplyToTilesLocations(availableTiles);

            _deploymentHandles.Add(deploymentHandle);
        }



		protected void OnDeployDrill()
        {
            if (OurTeam.SpendMoney(40))
            {
				BeginStructureDeployment(OnCreateDrillStructure);
            }
		}
		
		protected void OnCreateDrillStructure(Utility.Math.IntVector2 tileLocation)
		{
			//Debug.LogFormat("{0}.OnCreateDrillStructure(), tileLocation = {1}", this.ToString(), tileLocation);
			
			GameObject newDrillStructure = Instantiate(DrillStructurePrefab);
			newDrillStructure.transform.SetParent(this.transform.parent);
			newDrillStructure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
			newDrillStructure.transform.localScale = Vector3.one * 0.9f;
			
			ActorBehavior actor = newDrillStructure.GetComponent<ActorBehavior>();
			ActorModelManager.Instance.Add(actor);
		}
		

        protected void OnDeployDepot()
        {
            if (OurTeam.SpendMoney(40))
            {
				Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDepot(), TODO setup some depot deployment stuff.</color>", this.ToString());
				
				BeginStructureDeployment(OnCreateDepot_Structure);
            }
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
            if (OurTeam.SpendMoney(40))
            {
				Debug.LogFormat("<color=#ff00ff>{0}.OnDeployMachineFactory(), TODO setup some machine factory deployment stuff.</color>", this.ToString());
				
				BeginStructureDeployment(OnCreateMachineFactory_Structure);
            }
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


        protected void OnDeployConnector()
        {
            if (OurTeam.SpendMoney(20))
            {
				Debug.LogFormat("<color=#ff00ff>{0}.OnDeployConnector(), TODO setup some machine factory deployment stuff.</color>", this.ToString());
				
				BeginStructureDeployment(OnCreateConnector_Structure);
            }
        }

        protected void OnCreateConnector_Structure(Utility.Math.IntVector2 tileLocation)
        {
            Debug.LogFormat("{0}.OnCreateConnector_Structure(), tileLocation = {1}", this.ToString(), tileLocation);

            GameObject newConnector_Structure = Instantiate(ConnectorStructurePrefab);
            newConnector_Structure.transform.SetParent(this.transform.parent);
            newConnector_Structure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newConnector_Structure.transform.localScale = Vector3.one * 0.9f;

            ActorBehavior actor = newConnector_Structure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }













        
        protected void OnDeployCrusher()
        {
            if (OurTeam.SpendMoney(40))
            {
				BeginStructureDeployment(OnCreateCrusherStructure);
            }
        }
        
        protected void OnCreateCrusherStructure(Utility.Math.IntVector2 tileLocation)
        {
            //Debug.LogFormat("{0}.OnCreateCrusherStructure(), tileLocation = {1}", this.ToString(), tileLocation);
            
            GameObject newCrusherStructure = Instantiate(CrusherStructurePrefab);
            newCrusherStructure.transform.SetParent(this.transform.parent);
            newCrusherStructure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newCrusherStructure.transform.localScale = Vector3.one * 0.9f;
            
            ActorBehavior actor = newCrusherStructure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }


        protected void OnDeploySmelter()
        {
            if (OurTeam.SpendMoney(40))
            {
				BeginStructureDeployment(OnCreateSmelterStructure);
            }
        }
        
        protected void OnCreateSmelterStructure(Utility.Math.IntVector2 tileLocation)
        {
            //Debug.LogFormat("{0}.OnCreateSmelterStructure(), tileLocation = {1}", this.ToString(), tileLocation);
            
            GameObject newSmelterStructure = Instantiate(SmelterStructurePrefab);
            newSmelterStructure.transform.SetParent(this.transform.parent);
            newSmelterStructure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newSmelterStructure.transform.localScale = Vector3.one * 0.9f;
            
            ActorBehavior actor = newSmelterStructure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }


        protected void OnDeployFabricator()
        {
            if (OurTeam.SpendMoney(40))
            {
				BeginStructureDeployment(OnCreateFabricatorStructure);
            }
        }

        protected void OnCreateFabricatorStructure(Utility.Math.IntVector2 tileLocation)
        {
            //Debug.LogFormat("{0}.OnCreateFabricatorStructure(), tileLocation = {1}", this.ToString(), tileLocation);

            GameObject newFabricatorStructure = Instantiate(FabricatorStructurePrefab);
            newFabricatorStructure.transform.SetParent(this.transform.parent);
            newFabricatorStructure.transform.localPosition = new Vector3((float)tileLocation.x, 0f, (float)tileLocation.y) + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.01f;
            newFabricatorStructure.transform.localScale = Vector3.one * 0.9f;

            ActorBehavior actor = newFabricatorStructure.GetComponent<ActorBehavior>();
            ActorModelManager.Instance.Add(actor);
        }



        #endregion
	}
}
