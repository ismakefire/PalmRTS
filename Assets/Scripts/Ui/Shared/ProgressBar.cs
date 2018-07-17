using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class ProgressBar : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private Image _progressFillImage;

        #endregion
        #region Properties

        private float _progress = 1.0f;
        public float Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = Mathf.Clamp01(value);

                _progressFillImage.transform.localScale = new Vector3(_progress, 1f, 1f);
            }
        }

        #endregion
	}
}
