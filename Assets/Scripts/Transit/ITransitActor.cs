using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
	public interface ITransitActor
	{
		ActorBehavior Actor { get; }

        int EmptyBoxCount { get; set; }
        int DrillProductCount { get; set; }
	}
}
