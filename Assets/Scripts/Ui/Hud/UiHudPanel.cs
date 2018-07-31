using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Player;
using Misner.PalmRTS.Team;
using UnityEngine;
using UnityEngine.UI;

namespace Misner.PalmRTS.UI
{
	public class UiHudPanel : MonoBehaviour
	{
        #region SerializeField

        [SerializeField]
        private Text _moneyText;

        [SerializeField]
        private Text _debtText;

        #endregion

        #region Properties

        public string MoneyText
        {
            get
            {
                return _moneyText.text;
            }
            set
            {
                _moneyText.text = value;
            }
        }

        public string DebtText
        {
            get
            {
                return _debtText.text;
            }
            set
            {
                _debtText.text = value;
            }
        }

        #endregion

        #region MonoBehaviour Singleton

        private static UiHudPanel _instance = null;

        public static UiHudPanel Instance
        {
            get
            {
                return _instance;
            }
        }

        // Use this for initialization
        protected void Awake()
        {
            _instance = this;

            PlayerTeam playerTeam = TeamManager.Instance.GetTeam<PlayerTeam>(ETeam.Player);
            if (playerTeam != null)
            {
                this.MoneyText = playerTeam.GetPlayerMoneyString();
                this.DebtText = playerTeam.GetPlayerDebtString();
            }
        }

        #endregion
	}
}
