using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using Misner.PalmRTS.Structure;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.UI;
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
            if (PanelManager.Instance.IsAnyChildActive)
            {
                Debug.LogFormat("<color=#ff0000>{0}.ShowTransitDepotPanel(), PanelManager.Instance.IsAnyChildActive = {1}</color>", this.ToString(), PanelManager.Instance.IsAnyChildActive);
            }
            else
            {
				Debug.LogFormat("<color=#ff00ff>{0}.TODO()</color>", this.ToString());

                UiPlayerDepotPanel.Instance.ShowPanel(new UiPlayerDepotPanel.PlayerDepotActions());

                //UiPlayerHqPanel.Instance.ShowPanel(
                //    new UiPlayerHqPanel.PlayerHQActions()
                //    {
                //        CreateConstructionBot = OnCreateConstructionBot,
                //        CreateTransitVehicle = OnCreateTransitVehicle,
                //        CreateMiningDrill = OnCreateMiningDrill
                //    }
                //);
            }
        }

        #endregion
	}
}
