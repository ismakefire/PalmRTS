using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public interface ITransitActor : IInventoryStructure
	{
		ActorBehavior Actor { get; }
	}
}
