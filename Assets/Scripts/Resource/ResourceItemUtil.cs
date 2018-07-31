using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Resource
{
	public static class ResourceItemUtil
	{
        public static List<EResourceItem> GetAll()
        {
            List<EResourceItem> result = new List<EResourceItem>()
            {
                EResourceItem.SolidRock,
                EResourceItem.CrushedRock,
                EResourceItem.MetalPlate,
                EResourceItem.MetalBox,
            };
			
            return result;
        }
	}
}
