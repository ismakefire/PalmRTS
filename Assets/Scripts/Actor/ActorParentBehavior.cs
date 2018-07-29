using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Util;
using UnityEngine;

namespace Misner.PalmRTS.Actor
{
	public class ActorParentBehavior : MonoBehaviour
    {
        #region MonoBehaviour

        // Use this for initialization
        protected void Start()
        {
            ActorBehavior[] existingBehaviors = transform.GetComponentsInChildren<ActorBehavior>();

            if (!existingBehaviors.IsNullOrEmpty())
            {
                //Debug.LogFormat("{0}.Start(), existing tiles found! existingBehaviors.Length = {1}", this.ToString(), existingBehaviors.Length);

                foreach (ActorBehavior childBehavior in existingBehaviors)
                {
                    ActorModelManager.Instance.Add(childBehavior);
                }
            }
        }

        // Update is called once per frame
        protected void Update()
        {
        }

        #endregion
	}
}
