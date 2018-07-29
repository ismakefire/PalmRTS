using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class TransitOrderCard : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private Text _orderDescriptionText;

        [SerializeField]
        private Dropdown _verbDropdown;

        [SerializeField]
        private Dropdown _objectDropdown;

        [SerializeField]
        private InputField _objectInputfield;

        [SerializeField]
        private Dropdown _subjectDropdown;

        [SerializeField]
        private Button _removeOrderButton;

        [SerializeField]
        private Image _cardBackground;

        #endregion

        #region Properties

        public Dropdown VerbDropdown
        {
            get
            {
                return _verbDropdown;
            }
        }

        public Dropdown ObjectDropdown
        {
            get
            {
                return _objectDropdown;
            }
        }

        public InputField ObjectInputfield
        {
            get
            {
                return _objectInputfield;
            }
        }

        public Dropdown SubjectDropdown
        {
            get
            {
                return _subjectDropdown;
            }
        }

        public Button RemoveOrderButton
        {
            get
            {
                return _removeOrderButton;
            }
        }

        public Color CardBackgroundColor
        {
            get
            {
                return _cardBackground.color;
            }
            set
            {
                _cardBackground.color = value;
            }
        }

        #endregion
	}
}
