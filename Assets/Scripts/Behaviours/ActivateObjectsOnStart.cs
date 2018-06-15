using Misner.PalmRTS.Util;
using UnityEngine;

namespace Misner.PalmRTS.Behaviour
{
	public class ActivateObjectsOnStart : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private GameObject[] _activationTargets;

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            if (!_activationTargets.IsNullOrEmpty())
            {
                foreach (GameObject target in _activationTargets)
                {
                    target.SetActive(true);
                }
            }
        }

        #endregion
	}
}
