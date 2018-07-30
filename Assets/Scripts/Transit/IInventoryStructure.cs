using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
	public interface IInventoryStructure
    {
        int Inventory_EmptyBoxCount { get; set; }
        int Inventory_DrillProductCount { get; set; }

        event Action InventoryChanged;
	}
}
