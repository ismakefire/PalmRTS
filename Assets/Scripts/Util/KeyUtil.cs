using UnityEngine;

namespace Misner.PalmRTS.Util
{
	public static class KeyUtil
    {
        public static bool AnyShift
        {
            get
            {
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }
	}
}
