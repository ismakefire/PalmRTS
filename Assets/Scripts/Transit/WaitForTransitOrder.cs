using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public class WaitForTransitOrder : ITransitOrder
    {
        #region ITransitOrder

        public int? Object { get; set; }

        public int? Subject { get; set; }

        public float? Duration
        {
            get
            {
				float? result = null;

                if (Object != null)
                {
                    result = Mathf.Max(1f, (float)Object);
                }

                return result;
            }
        }

        public void CompleteTransaction()
        {
        }

        #endregion
    }
}
