using Misner.PalmRTS.Actor;
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
            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);

            playerTeam.AddClickEvent(Actor, ShowHQPanel);
        }

		protected void Update ()
		{
            if (transform.localPosition.y < 1.9f)
            {
                Body.velocity += Vector3.up * 0.3f * Time.timeScale;
            }
            else if (transform.localPosition.y < 2.1f)
            {
                Body.velocity -= Vector3.up * 0.1f * Time.timeScale;
            }
            else
            {
                Body.velocity -= Vector3.up * 0.3f * Time.timeScale;
            }

            Body.velocity *= Mathf.Exp(-Time.deltaTime);
		}

        #endregion

        #region Events

        protected void ShowHQPanel()
        {
            UiConstructionBotPanel.Instance.ShowPanel();
        }

        #endregion
	}
}
