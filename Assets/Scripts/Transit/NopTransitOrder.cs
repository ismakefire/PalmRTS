using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public class NopTransitOrder : ITransitOrder
    {
        #region ITransitOrder

		public int? Object { get; set; }
		
		public int? Subject { get; set; }

        public float? Duration
        {
            get
            {
                return null;
            }
        }

        public void CompleteTransaction()
        {
        }

        #endregion
	}
}
