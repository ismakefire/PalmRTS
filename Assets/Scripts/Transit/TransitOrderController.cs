using System;
using Misner.PalmRTS.Player;
using UnityEngine;

namespace Misner.PalmRTS.Transit
{
    public class TransitOrderController
    {
        private readonly TransitDepotStructureActor _transitDepot;

        private ITransitOrder _currentOrder = new NopTransitOrder();
        
        public event Action<bool> PrimaryOrderChanged;
        
        public TransitOrderController(TransitDepotStructureActor transitDepot, int? verb, int? @object = null, int? subject = null)
        {
            _transitDepot = transitDepot;
            Verb = verb;
            Object = @object;
            Subject = subject;
        }

        private bool _isPrimaryOrder = false;

        public bool IsPrimaryOrder
        {
            get
            {
                return _isPrimaryOrder;
            }
            set
            {
                if (_isPrimaryOrder != value)
                {
					if (PrimaryOrderChanged != null)
					{
						PrimaryOrderChanged(value);
					}
					
					_isPrimaryOrder = value;
                }
            }
        }

        public int? _verb;

        public int? Verb
        {
            get
            {
                return _verb;
            }
            set
            {
                if (_verb != value)
                {
					ITransitOrder nextOrder;
					
					switch (value)
					{
						case 0:
							nextOrder = new NopTransitOrder();
							break;
							
						case 1:
                            nextOrder = new TakeFromTransitOrder(_transitDepot);
							break;

                        case 2:
                            nextOrder = new InsertIntoTransitOrder(_transitDepot);
                            break;

                        //case 3:
                            //nextOrder = new SendToTransitOrder();
                            //break;

                        case 4:
                            nextOrder = new WaitForTransitOrder();
                            break;

						default:
							Debug.LogFormat("<color=#ff0000>{0}.Verb set(), what is this?</color>", this.ToString());
							nextOrder = new NopTransitOrder();
							break;
					}
					
					nextOrder.Object = _currentOrder.Object;
					nextOrder.Subject = _currentOrder.Subject;
					
					//Debug.LogFormat("<color=#ff00ff>{0}.Verb set(), nextOrder = {1}</color>", this.ToString(), nextOrder.ToString());

                    _currentOrder = nextOrder;
					_verb = value;
                }
            }
        }

        public int? Object
        {
            get
            {
                return _currentOrder.Object;
            }
            set
            {
                //Debug.LogFormat("<color=#00ffff>{0}.Object set(), _currentOrder.Object = {1}, value = {2}</color>", this.ToString(), _currentOrder.Object, value);
                
                _currentOrder.Object = value;
            }
        }

        public int? Subject
        {
            get
            {
                return _currentOrder.Subject;
            }
            set
            {
                _currentOrder.Subject = value;
            }
        }

        public float Duration
        {
            get
            {
                return (_currentOrder.Duration != null) ? _currentOrder.Duration.Value : 2.5f;
            }
        }

        public void CompleteOrder()
        {
            //Debug.LogFormat("<color=#ff00ff>{0}.CompleteOrder()</color>", this.ToString());

            _currentOrder.CompleteTransaction();
        }
    }
}
