using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public interface ITransitOrder
    {
        int? Object { get; set; }
        List<string> GenerateObjectDropdownOptions();

        int? Subject { get; set; }

        float? Duration { get; }

        void CompleteTransaction();
	}
}
