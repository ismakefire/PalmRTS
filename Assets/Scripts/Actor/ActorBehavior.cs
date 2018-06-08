using Misner.PalmRTS.Util;
using UnityEngine;

namespace Misner.PalmRTS.Actor
{
	public class ActorBehavior : MonoBehaviour
	{
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
            if (KeyUtil.AnyShift)
            {
                Debug.LogFormat("{0}.OnMouseDown(), calling destroy on self! >_<", this.ToString());

                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogFormat("{0}.OnMouseDown(), jump!", this.ToString());

                GetComponent<Rigidbody>().velocity += Random.onUnitSphere * 9f;
            }
        }

        #endregion
	}
}
