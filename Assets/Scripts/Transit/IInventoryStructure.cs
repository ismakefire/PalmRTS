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

        event Action InventoryChanged;
    }
}
