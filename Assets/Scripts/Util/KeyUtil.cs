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

        public static bool AnyControl
        {
            get
            {
                return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            }
        }

        public static bool AnyOption
        {
            get
            {
                return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            }
        }

        public static bool AnyCommand
        {
            get
            {
                bool isApple = Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightApple);
                bool isCommand = Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
                
                return isApple || isCommand;
            }
        }
	}
}
