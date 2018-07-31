using System;
using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Transit;
using UnityEngine;

namespace Misner.PalmRTS.UI
{
    public class OrdersCardsHandler
    {
        public readonly TransitOrderController TransitOrder;

        private readonly TransitOrderCard _transitOrderCard;
        private readonly Action<OrdersCardsHandler> _removeOrderButton;

        private bool isActive = true;

        public OrdersCardsHandler(UiPlayerDepotPanel unused, TransitOrderController transitOrder, TransitOrderCard transitOrderCard, Action<OrdersCardsHandler> removeOrderButton)
        {
            TransitOrder = transitOrder;
            _transitOrderCard = transitOrderCard;
            _removeOrderButton = removeOrderButton;

            _transitOrderCard.VerbDropdown.onValueChanged.AddListener(OnVerbDropdownChanged);
            _transitOrderCard.ObjectDropdown.onValueChanged.AddListener(OnObjectDropdownChanged);
            _transitOrderCard.ObjectInputfield.onEndEdit.AddListener(OnObjectInputfieldChanged);
            _transitOrderCard.SubjectDropdown.onValueChanged.AddListener(OnSubjectDropdownChanged);

            _transitOrderCard.VerbDropdown.value = 0;
            _transitOrderCard.ObjectDropdown.gameObject.SetActive(false);
            _transitOrderCard.ObjectInputfield.gameObject.SetActive(false);
            _transitOrderCard.SubjectDropdown.gameObject.SetActive(false);

            _transitOrderCard.RemoveOrderButton.onClick.AddListener(OnRemoveOrderButtonClicked);
            transitOrder.PrimaryOrderChanged += OnPrimaryOrderChanged;

            OnPrimaryOrderChanged(transitOrder.IsPrimaryOrder);
        }

        private void OnPrimaryOrderChanged(bool isPrimaryOrder)
        {
            if (isPrimaryOrder)
            {
                _transitOrderCard.CardBackgroundColor = Color.green;
            }
            else
            {
                _transitOrderCard.CardBackgroundColor = Color.blue;
            }
        }

        public void Clear()
        {
            if (isActive)
            {
                _transitOrderCard.transform.parent = null;
                UnityEngine.Object.Destroy(_transitOrderCard.gameObject);

                _transitOrderCard.VerbDropdown.onValueChanged.RemoveListener(OnVerbDropdownChanged);
                _transitOrderCard.ObjectDropdown.onValueChanged.RemoveListener(OnObjectDropdownChanged);
                _transitOrderCard.ObjectInputfield.onEndEdit.RemoveListener(OnObjectInputfieldChanged);
                _transitOrderCard.SubjectDropdown.onValueChanged.RemoveListener(OnSubjectDropdownChanged);

                _transitOrderCard.RemoveOrderButton.onClick.RemoveListener(OnRemoveOrderButtonClicked);
                TransitOrder.PrimaryOrderChanged -= OnPrimaryOrderChanged;

                isActive = false;
            }
        }

        public void Setup(int? verbValue, int? objectValue, int? subjectValue)
        {
            if (verbValue != null)
            {
                _transitOrderCard.VerbDropdown.value = verbValue.Value;
                ChangeVerbInternal(verbValue.Value);

                if (objectValue != null)
                {
                    _transitOrderCard.ObjectDropdown.value = objectValue.Value;
                    _transitOrderCard.ObjectInputfield.text = objectValue.Value.ToString();
                    ChangeObjectInternal(objectValue.Value);

                    if (subjectValue != null)
                    {
                        _transitOrderCard.SubjectDropdown.value = subjectValue.Value;
                        ChangeSubjectInternal(subjectValue.Value);
                    }
                }
            }
        }

        protected void OnVerbDropdownChanged(int verbValue)
        {
			TransitOrder.Verb = verbValue;
			TransitOrder.Object = null;
			TransitOrder.Subject = null;

            if (TransitOrder.Verb == 3)
            {
                int objectResult = 10;
                
                TransitOrder.Object = objectResult;
                ChangeObjectInternal(objectResult);
            }

            ChangeVerbInternal(verbValue);
        }

        protected void OnObjectDropdownChanged(int objectValue)
        {
            switch (TransitOrder.Verb)
            {
                case 1:
                case 2:
                    break;

                default:
                    return;
            }

            if (TransitOrder.Object != objectValue)
            {
                TransitOrder.Object = objectValue;
                TransitOrder.Subject = null;
            }

            ChangeObjectInternal(objectValue);
        }

        protected void OnObjectInputfieldChanged(string objectValue)
        {
            Debug.LogFormat("<color=#ffffff>{0}.OnObjectInputfieldChanged(), TransitOrder.Verb = {1}</color>", this.ToString(), TransitOrder.Verb);

            if (TransitOrder.Verb == 3)
            {
				int objectResult;
				
				if (int.TryParse(objectValue, out objectResult))
				{
					Debug.LogFormat("<color=#ffffff>{0}.OnObjectInputfieldChanged(), result = {1}</color>", this.ToString(), objectResult);

                    TransitOrder.Object = objectResult;
					ChangeObjectInternal(objectResult);
				}
            }
        }

		protected void OnSubjectDropdownChanged(int subjectValue)
		{
			TransitOrder.Subject = subjectValue;
			ChangeSubjectInternal(subjectValue);
		}

        protected void OnRemoveOrderButtonClicked()
        {
            Clear();

            _removeOrderButton(this);
        }

        private void ChangeVerbInternal(int verbValue)
        {
            switch (verbValue)
            {
                case 0:
                    _transitOrderCard.ObjectDropdown.gameObject.SetActive(false);
                    _transitOrderCard.ObjectInputfield.gameObject.SetActive(false);
                    _transitOrderCard.SubjectDropdown.gameObject.SetActive(false);
                    break;

                case 1:
                case 2:
                    _transitOrderCard.ObjectInputfield.gameObject.SetActive(false);
                    _transitOrderCard.SubjectDropdown.gameObject.SetActive(false);

                    _transitOrderCard.ObjectDropdown.gameObject.SetActive(true);
                    _transitOrderCard.ObjectDropdown.value = 0;
                    break;

                case 3:
                    _transitOrderCard.ObjectDropdown.gameObject.SetActive(false);
                    _transitOrderCard.SubjectDropdown.gameObject.SetActive(false);

                    _transitOrderCard.ObjectInputfield.gameObject.SetActive(true);
                    _transitOrderCard.ObjectInputfield.text = "10";
                    break;

                default:
                    break;
            }
        }

        private void ChangeObjectInternal(int objectValue)
        {
            switch (objectValue)
            {
                case 0:
                    _transitOrderCard.SubjectDropdown.gameObject.SetActive(false);
                    break;

                default:
                    //Debug.LogFormat("<color=#ff00ff>{0}.ChangeObjectInternal(), _transitOrderCard.VerbDropdown.value = {1}</color>", this.ToString(), _transitOrderCard.VerbDropdown.value);
                    if (_transitOrderCard.VerbDropdown.value != 3)
                    {
                        _transitOrderCard.SubjectDropdown.gameObject.SetActive(true);
                    }
                    else
                    {
                        _transitOrderCard.SubjectDropdown.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        private void ChangeSubjectInternal(int subjectValue)
        {
        }
    }

}
