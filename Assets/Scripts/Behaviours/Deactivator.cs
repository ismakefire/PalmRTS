using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Behaviour
{
	public class Deactivator : MonoBehaviour
	{
        #region Properties

        public bool IsDeactivated
        {
            get
            {
                return !gameObject.activeSelf;
            }
            set
            {
                gameObject.SetActive(!value);
            }
        }

        #endregion
	}
}
