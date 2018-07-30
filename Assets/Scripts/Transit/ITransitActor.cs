﻿using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Actor;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
	public interface ITransitActor
	{
		ActorBehavior Actor { get; }

        int Inventory_EmptyBoxCount { get; set; }
        int Inventory_DrillProductCount { get; set; }
	}
}
