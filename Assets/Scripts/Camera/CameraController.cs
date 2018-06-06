using System;
using System.Collections.Generic;
using Misner.PalmRTS.PlayerInput;
using UnityEngine;

namespace Misner.PalmRTS.Camera
{
    public class CameraController : MonoBehaviour, KeyInputListener.IInputTarget
	{
        #region Types

        [Serializable]
        public struct DirectionKey
        {
            [SerializeField]
            public string name;

			[SerializeField]
            public KeyCode key;

            [SerializeField]
            public Vector3 direction;
        }

        #endregion

        #region Variables

        private KeyInputListener _keyInputListener = null;

        private HashSet<DirectionKey> _keysDown = new HashSet<DirectionKey>();

        #endregion

        #region SerializeField

        /// <summary>
        /// Camera movment speed in Unity unitys Per Second.
        /// </summary>
		[SerializeField]
        private float _speedInUps = 5f;

        /// <summary>
        /// The direction keys with WASD as the default. Click the gear icon and select "Reset" to return to these values.
        /// </summary>
        [SerializeField]
        private DirectionKey[] _directionKeys = {
            new DirectionKey() {
                name = "W (Up)",
				key = KeyCode.W,
                direction = Vector3.forward,
            },
            new DirectionKey() {
                name = "A (Left)",
                key = KeyCode.A,
                direction = Vector3.left,
            },
            new DirectionKey() {
                name = "S (Down)",
                key = KeyCode.S,
                direction = Vector3.back,
            },
            new DirectionKey() {
                name = "D (Right)",
                key = KeyCode.D,
                direction = Vector3.right,
            },
        };

        #endregion

        #region MonoBehaviour

        protected void Awake()
        {
            _keyInputListener = gameObject.AddComponent<KeyInputListener>();
        }

        protected void Start()
        {
            if (_keyInputListener != null)
            {
                _keyInputListener.InputTarget = this;
            }
        }

        /// <summary>
        /// Updates with the user's framerate.
        /// </summary>
        protected void Update()
        {
            EvaluateInput();
        }

        #endregion

        #region KeyInputListener.IInputTarget

        public void OnKeyDown(KeyCode keyCode)
        {
            DirectionKey? currentKey = GetKeyDirectionFromCode(keyCode);

            if (currentKey != null && !_keysDown.Contains(currentKey.Value))
            {
                // Only adds one copy of the key, incase we change our container.
                _keysDown.Add(currentKey.Value);
            }
        }

        public void OnKeyUp(KeyCode keyCode)
        {
            DirectionKey? currentKey = GetKeyDirectionFromCode(keyCode);

            if (currentKey != null)
            {
                _keysDown.Remove(currentKey.Value);
            }
        }

        #endregion

        #region Private Methods

        private DirectionKey? GetKeyDirectionFromCode(KeyCode keyCode)
        {
            foreach (DirectionKey directionKey in _directionKeys)
            {
                if (directionKey.key == keyCode)
                {
                    return directionKey;
                }
            }

            return null;
        }

        private void EvaluateInput()
        {
            // Are any keys down?
            if (_keysDown.Count <= 0)
            {
                return;
            }


            // Do those keys add up to any motion?
            Vector3 directionSum = Vector3.zero;

            foreach (DirectionKey downKey in _keysDown)
            {
                directionSum += downKey.direction;
            }

            if (directionSum.sqrMagnitude < 0.001f)
            {
                return;
            }


            // Given the user time passed since last frame, move the camera.
            Vector3 finalDirection = directionSum.normalized;
            Vector3 displacementThisFrameInUnits = finalDirection * _speedInUps * Time.deltaTime;

            this.transform.localPosition += displacementThisFrameInUnits;
        }

        #endregion
	}
}
