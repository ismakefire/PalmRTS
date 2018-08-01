using System;
using Misner.PalmRTS.Team;
using Misner.PalmRTS.Util;
using UnityEngine;

namespace Misner.PalmRTS.Actor
{
	public class ActorBehavior : MonoBehaviour
	{
        #region Properties

        public event Action<ActorBehavior> OnClicked;

        #endregion

        #region SerializeField

		[SerializeField]
		private ETeam _actorTeam;

        #endregion

        #region Properties

        public ETeam ControllingTeam
        {
            get
            {
                return _actorTeam;
            }
        }

        public Vector2Int TilePosition
        {
            get
            {
                return new Vector2Int(
                    Mathf.RoundToInt(transform.localPosition.x),
                    Mathf.RoundToInt(transform.localPosition.z)
                );
            }
        }

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            if (GetComponent<Collider>() == null)
            {
                Debug.LogErrorFormat("{0}.Start(), a Collider component must be attached for this script to function.", this.ToString());
            }

            ITeam team = TeamManager.Instance.GetTeam(ControllingTeam);

            if (team != null)
            {
                team.OnActorAdded(this);
            }
        }

        protected void OnDestroy()
        {
            ITeam team = TeamManager.Instance.GetTeam(ControllingTeam);

            if (team != null)
            {
                team.OnActorRemoved(this);
            }
        }

        protected void OnMouseDown()
        {
            if (OnClicked != null)
            {
                OnClicked(this);
            }
        }

        #endregion

        #region Public Methods

        public void Jump()
        {
            this.GetComponent<Rigidbody>().velocity += UnityEngine.Random.onUnitSphere * 9f;
        }

        #endregion
	}
}
