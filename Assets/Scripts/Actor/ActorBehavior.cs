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

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            if (GetComponent<Collider>() == null)
            {
                Debug.LogErrorFormat("{0}.Start(), a Collider component must be attached for this script to function.", this.ToString());
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
