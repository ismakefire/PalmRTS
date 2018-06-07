using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Util
{
	public static class StringExtensions
    {
        public static string SubStringFromEnd(this string str, int endingIndex)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= endingIndex)
            {
                return "";
            }
            else if (endingIndex <= 0)
            {
                return str;
            }
            else
            {
                return str.Substring(0, str.Length - endingIndex);
            }
        }

        public static string SubStringFromEnd(this string str, int endingIndex, int length)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= endingIndex || length <= 0)
            {
                return "";
            }
            else
            {
                int lengthLeft;
                if (endingIndex <= 0)
                {
                    lengthLeft = str.Length;
                }
                else
                {
                    lengthLeft = str.Length - endingIndex;
                }

				int lengthToUse = Mathf.Min(lengthLeft, length);
				
				return str.Substring(lengthLeft - lengthToUse, lengthToUse);
            }
        }
	}
}
