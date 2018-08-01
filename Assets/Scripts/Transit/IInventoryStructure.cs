using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Resource;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
	public interface IInventoryStructure
    {
        ResourceCollection Resources { get; }
        
        int Inventory_EmptyBoxCount { get; set; }
        int Inventory_DrillProductCount { get; set; }

        event Action InventoryChanged;
	}
}
