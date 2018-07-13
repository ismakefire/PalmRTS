using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using UnityEngine;

namespace Misner.PalmRTS.Player
{
    [RequireComponent(typeof(ActorBehavior))]
	public class TransitBotActorBehavior : MonoBehaviour
    {
        #region Properties

        //public ActorBehavior Actor
        //{
        //    get
        //    {
        //        return GetComponent<ActorBehavior>();
        //    }
        //}

        public Rigidbody Body
        {
            get
            {
                return GetComponent<Rigidbody>();
            }
        }

        #endregion

        #region SerializeField

        [SerializeField]
        private Rigidbody _wheelBody;

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            if (_wheelBody == null)
            {
                Debug.LogFormat("<color=#ff00ff>{0}.Start(), _wheelBody is null.</color>", this.ToString());
            }
        }

		// Update is called once per frame
		protected void Update ()
		{
            float angularAcceluration = 50f;
            _wheelBody.angularVelocity *= Mathf.Exp(-Time.deltaTime * 10.0f);
            _wheelBody.angularVelocity += (Body.rotation * Vector3.right) * angularAcceluration * Time.deltaTime;

            Body.angularVelocity = Vector3.up * 1f;
		}

        #endregion
	}
}
