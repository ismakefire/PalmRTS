using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Selection;
using Misner.PalmRTS.Team;
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
            UiConstructionBotPanel.Instance.ShowPanel(
                new UiConstructionBotPanel.PlayerDeploymentActions() {
                    DeployDrill = OnDeployDrill
                }
            );
        }

        protected void OnDeployDrill()
        {
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(Actor.ControllingTeam);

            List<Vector2Int> availableTiles = playerTeam.GenerateAvailableStructureTiles();

            Debug.LogFormat("<color=#ff00ff>{0}.OnDeployDrill(), TODO setup some drill deployment stuff. availableTiles.Count = {1}</color>", this.ToString(), availableTiles.Count);

            foreach (Vector2Int tileLocation in availableTiles)
            {
                SelectionTileParentBehavior.Instance.CreateObjectAt(tileLocation, OnSelectionPerformed);
            }
        }

        protected void OnSelectionPerformed(Vector2Int tileLocation)
        {
            Debug.LogFormat("<color=#ff00ff>{0}.OnSelectionPerformed(), tileLocation = {1}</color>", this.ToString(), tileLocation);
        }

        #endregion
	}
}
