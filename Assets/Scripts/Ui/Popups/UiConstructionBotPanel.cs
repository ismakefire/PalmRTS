﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class UiConstructionBotPanel : MonoBehaviour
    {
        #region Types

        public class PlayerDeploymentActions
        {
            public Action RecycleStructure { get; set; }
            
            public Action DeployDrill { get; set; }
            public Action DeployDepot { get; set; }
            public Action DeployMachineFactory { get; set; }
            public Action DeployConnector { get; set; }

            public Action DeployCrusher { get; set; }
            public Action DeploySmelter{ get; set; }
            public Action DeployFabricator { get; set; }
        }

        #endregion

        #region Variables

        private PlayerDeploymentActions _actions = null;

        #endregion

        #region Singleton

        private static UiConstructionBotPanel _instance = null;

        public static UiConstructionBotPanel Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region SerializeField

        [SerializeField]
        private Button _recycleStructureButton;

        [SerializeField]
        private Button _deployDrillButton;

        [SerializeField]
        private Button _deployDepotButton;

        [SerializeField]
        private Button _deployMachineFactoryButton;

        [SerializeField]
        private Button _deployConnectorButton;



        [SerializeField]
        private Button _deployCrusherButton;

        [SerializeField]
        private Button _deploySmelterButton;

        [SerializeField]
        private Button _deployFabricatorButton;

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Debug.LogErrorFormat("{0}.Awake(), why are there two of these?", this.ToString());
            }

            _recycleStructureButton.onClick.AddListener(OnRecycleStructureButtonClicked);

            _deployDrillButton.onClick.AddListener(OnDeployDrillButtonClicked);
            if (_deployDepotButton != null)
            {
				_deployDepotButton.onClick.AddListener(OnDeployDepotButtonClicked);
			}
            if (_deployMachineFactoryButton != null)
            {
				_deployMachineFactoryButton.onClick.AddListener(OnDeployMachineFactoryButtonClicked);
            }
            _deployConnectorButton.onClick.AddListener(OnDeployConnectorButtonClicked);

            _deployCrusherButton.onClick.AddListener(OnDeployCrusherButtonClicked);
            _deploySmelterButton.onClick.AddListener(OnDeploySmelterButtonClicked);
            _deployFabricatorButton.onClick.AddListener(OnDeployFabricatorButtonClicked);
        }

        // Update is called once per frame
        protected void Start()
        {
            HidePanel();
        }

        #endregion

        #region Public Interface

        private float _panelShowTime = -1f;

        public void ShowPanel(PlayerDeploymentActions actions = null)
        {
			_panelShowTime = Time.time;
            _actions = actions;
            this.gameObject.SetActive(true);

            //Debug.LogFormat("{0}.ShowPanel()", this.ToString());
        }

        public void HidePanel()
        {
            _actions = null;
            this.gameObject.SetActive(false);

            //Debug.LogFormat("{0}.HidePanel()", this.ToString());
        }

        #endregion

        #region Events

        protected void OnRecycleStructureButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnRecycleStructureButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.RecycleStructure != null)
            {
                Debug.LogFormat("{0}.OnRecycleStructureButtonClicked(), we're all good!", this.ToString());

                _actions.RecycleStructure();
            }
            else
            {
                Debug.LogFormat("{0}.OnRecycleStructureButtonClicked(), (_actions != null && _actions.RecycleStructure != null) = {1}", this.ToString(), (_actions != null && _actions.RecycleStructure != null));
            }

            HidePanel();
        }

        protected void OnDeployDrillButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployDrillButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeployDrill != null)
            {
                Debug.LogFormat("{0}.OnDeployDrillButtonClicked(), we're all good!", this.ToString());

                _actions.DeployDrill();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeployDrillButtonClicked(), (_actions != null && _actions.DeployDrill != null) = {1}", this.ToString(), (_actions != null && _actions.DeployDrill != null));
            }

            HidePanel();
        }

        protected void OnDeployDepotButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployDepotButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeployDepot != null)
            {
                Debug.LogFormat("{0}.OnDeployDepotButtonClicked(), we're all good!", this.ToString());

                _actions.DeployDepot();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeployDepotButtonClicked(), (_actions != null && _actions.DeployDepot != null) = {1}", this.ToString(), (_actions != null && _actions.DeployDepot != null));
            }

            HidePanel();
        }

        protected void OnDeployMachineFactoryButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployMachineFactoryButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeployMachineFactory != null)
            {
                Debug.LogFormat("{0}.OnDeployMachineFactoryButtonClicked(), we're all good!", this.ToString());

                _actions.DeployMachineFactory();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeployMachineFactoryButtonClicked(), (_actions != null && _actions.DeployMachineFactory != null) = {1}", this.ToString(), (_actions != null && _actions.DeployMachineFactory != null));
            }

            HidePanel();
        }

        protected void OnDeployConnectorButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployConnectorButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeployConnector != null)
            {
                Debug.LogFormat("{0}.OnDeployConnectorButtonClicked(), we're all good!", this.ToString());

                _actions.DeployConnector();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeployConnectorButtonClicked(), (_actions != null && _actions.DeployConnector != null) = {1}", this.ToString(), (_actions != null && _actions.DeployConnector != null));
            }

            HidePanel();
        }

        protected void OnDeployCrusherButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployCrusherButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeployCrusher != null)
            {
                Debug.LogFormat("{0}.OnDeployCrusherButtonClicked(), we're all good!", this.ToString());

                _actions.DeployCrusher();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeployCrusherButtonClicked(), (_actions != null && _actions.DeployCrusher != null) = {1}", this.ToString(), (_actions != null && _actions.DeployCrusher != null));
            }

            HidePanel();
        }

        protected void OnDeploySmelterButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeploySmelterButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeploySmelter != null)
            {
                Debug.LogFormat("{0}.OnDeploySmelterButtonClicked(), we're all good!", this.ToString());

                _actions.DeploySmelter();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeploySmelterButtonClicked(), (_actions != null && _actions.DeploySmelter != null) = {1}", this.ToString(), (_actions != null && _actions.DeploySmelter != null));
            }

            HidePanel();
        }

        protected void OnDeployFabricatorButtonClicked()
        {
            if (Time.time - _panelShowTime < 0.1f)
            {
                Debug.LogFormat("<color=#ff0000>{0}.OnDeployFabricatorButtonClicked() TOO FAST!</color>", this.ToString());
                return;
            }

            if (_actions != null && _actions.DeployFabricator != null)
            {
                Debug.LogFormat("{0}.OnDeployFabricatorButtonClicked(), we're all good!", this.ToString());

                _actions.DeployFabricator();
            }
            else
            {
                Debug.LogFormat("{0}.OnDeployFabricatorButtonClicked(), (_actions != null && _actions.DeployFabricator != null) = {1}", this.ToString(), (_actions != null && _actions.DeployFabricator != null));
            }

            HidePanel();
        }

        #endregion
	}
}
