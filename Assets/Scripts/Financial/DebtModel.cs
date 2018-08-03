using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Financial
{
	public class DebtModel : MonoBehaviour
	{
        #region Variables

        private double _balance = 0f;

        /// <summary>
        /// Our debts interest rate in Percent Per Second.
        /// </summary>
        private double _interestRatePPS = 0f;

        #endregion

        #region Properties

        public event Func<int, bool> BalanceChange;

        public event Action DebtChanged;

        public int DebtBalance
        {
            get
            {
                return Mathf.RoundToInt( (float)_balance );
            }
        }

        #endregion

        #region Public Static Methods

        public static DebtModel Create(int amount, double pps)
        {
            GameObject unityObject = new GameObject();
            DebtModel model = unityObject.AddComponent<DebtModel>();

            model._balance = (double)amount;
            model._interestRatePPS = pps;
            
            return model;
        }

        #endregion

        #region Public Interface

        public void BorrowMoney(int borrowRequest)
        {
            if (BalanceChange(+borrowRequest))
            {
                float percentOfDollarOwed = (_timeOfNextCharge - _timeMeasured) / _timeOfNextCharge;
                float percentOfDollarLeft = 1 - percentOfDollarOwed;

                // Afforded.
                _balance += borrowRequest;
                _timeOfNextCharge = _timeMeasured + (float)CalculateTime() * percentOfDollarLeft; // This is an approxiation, but oh well.

                Debug.LogFormat("<color=#ff00ff>{0}.BorrowMoney({1}), percentOfDollarOwed = {2}, _timeMeasured = {3}, _timeOfNextCharge = {4}</color>", this.ToString(), borrowRequest, percentOfDollarOwed, _timeMeasured, _timeOfNextCharge);

                if (DebtChanged != null)
                {
                    DebtChanged();
                }
            }
            else
            {
                // Can't afford. Do nothing. 
            }
        }

        public void PayoffMoney(int payoffRequest)
        {
			if (payoffRequest > _balance)
			{
                if (BalanceChange(Mathf.RoundToInt((float)_balance)))
                {
                    // Afforded.
                    _balance = 0;

                    if (DebtChanged != null)
                    {
                        DebtChanged();
                    }
                }
                else
                {
                    // Can't afford. Do nothing. 
                }
			}
			else
            {
                if (BalanceChange(-payoffRequest))
                {
                    float percentOfDollarOwed = (_timeOfNextCharge - _timeMeasured) / _timeOfNextCharge;
                    float percentOfDollarLeft = 1 - percentOfDollarOwed;

                    // Afforded.
                    _balance -= payoffRequest;
                    _timeOfNextCharge = _timeMeasured + (float)CalculateTime() * percentOfDollarLeft; // This is an approxiation, but oh well.

                    Debug.LogFormat("<color=#ff00ff>{0}.PayoffMoney({1}), percentOfDollarOwed = {2}, _timeMeasured = {3}, _timeOfNextCharge = {4}</color>", this.ToString(), payoffRequest, percentOfDollarOwed, _timeMeasured, _timeOfNextCharge);

                    if (DebtChanged != null)
                    {
                        DebtChanged();
                    }
                }
                else
                {
                    // Can't afford. Do nothing. 
                }
            }
        }

        #endregion

        #region Time Calculation

        private float _timeOfNextCharge;
        private float _timeMeasured;

        private double CalculateTime()
        {
			double time = (ln(_balance + 1) - ln(_balance)) / _interestRatePPS;

            return time;
        }

        #endregion

        #region MonoBehaviour

		// Use this for initialization
		protected void Start ()
		{
            double time = CalculateTime();
            Debug.LogFormat("<color=#ff00ff>{0}.Start(), ln(_balance + 1f) = {1}, ln(_balance) = {2}, time = {3}</color>", this.ToString(), ln(_balance + 1), ln(_balance), time);

            _timeOfNextCharge = (float)time;
            _timeMeasured = 0f;
		}

		// Update is called once per frame
		protected void Update ()
		{
            if (_balance > 0)
            {
				_timeMeasured += Time.deltaTime;
				
				if (_timeMeasured >= _timeOfNextCharge)
				{
					PerformCharge();
					_timeMeasured -= _timeOfNextCharge;
				}
            }
        }

        #endregion

        #region Helper Methods

        private void PerformCharge()
        {
            if (BalanceChange(-1))
            {
                // Afforded. This has been paid by the user. Change nothing.
            }
            else
            {
                // Can't afford. Pay for it here and recalculate.
                _balance += 1;
                _timeOfNextCharge = (float)CalculateTime();

                if (DebtChanged != null)
                {
                    DebtChanged();
                }
            }
        }

        private static readonly double e = Math.Exp(1);

        private static double ln(double d)
        {
            double result = Math.Log(d, e);

            return result;
        }

        #endregion
	}
}
