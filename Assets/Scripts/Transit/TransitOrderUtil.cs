using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
	public static class TransitOrderUtil
	{
        public static Vector2Int? GetOffsetFromSubject(this ITransitOrder transitOrder)
        {
            Vector2Int? offset = null;

            switch (transitOrder.Subject)
            {
                case 1:
                    offset = Vector2Int.up;
                    break;

                case 2:
                    offset = Vector2Int.down;
                    break;

                case 3:
                    offset = Vector2Int.left;
                    break;

                case 4:
                    offset = Vector2Int.right;
                    break;

                default:
                    break;
            }

            return offset;
        }
	}
}
