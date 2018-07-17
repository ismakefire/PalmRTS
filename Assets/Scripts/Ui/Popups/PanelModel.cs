using System;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
    public class PanelModel<TActions> where TActions : class
	{
        #region Private Variables

        private float _panelShowTime = -1f;
        private Action _hidePanel = null;

        #endregion

        #region Properties

        public TActions Actions { get; private set; }

        #endregion

        #region Public Interface

        public void ShowPanel(TActions actions, Action hidePanel)
        {
            Actions = actions;
            _hidePanel = hidePanel;
            _panelShowTime = Time.time;
        }

        public bool TooEarlyForPanel()
        {
            bool result = (Time.time - _panelShowTime < 0.1f);
            
            return result;
        }

        public bool CanShowPanel()
        {
            return (Actions != null) && !TooEarlyForPanel();
        }

        public void Clear()
        {
            Actions = null;
            _panelShowTime = -1f;
        }

        public void PlayPanelAction(Action panelAction)
        {
            if (!this.CanShowPanel())
            {
                Debug.LogFormat("<color=#ff0000>{0}.PlayPanelAction() TOO FAST!</color>", this.ToString());
            }
            else
            {
                Debug.LogFormat("{0}.PlayPanelAction(), we're all good!", this.ToString());

                if (panelAction != null)
                {
                    panelAction();
                }

                if (_hidePanel != null)
                {
                    _hidePanel();
                }
            }
        }

        #endregion
	}
}
