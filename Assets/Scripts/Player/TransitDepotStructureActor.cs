using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
	public class TransitDepotStructureActor : MonoBehaviour
    {
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

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
        {
            OurTeam.AddClickEvent(Actor, ShowTransitDepotPanel);

            StructureTileManager.Instance.Add(Actor);
		}

        #endregion

        #region Events

        protected void ShowTransitDepotPanel()
        {
            Debug.LogFormat("<color=#007fff>{0}.ShowTransitDepotPanel()</color>", this.ToString());
        }

        #endregion
	}
}
