using System.Collections;
using System.Collections.Generic;
using Misner.PalmRTS.Player;
using UnityEngine;

namespace Misner.PalmRTS.Team
{
	public class TeamManager
	{
        #region Variables

        private Dictionary<ETeam, ITeam> _teamEnumToImplementation = new Dictionary<ETeam, ITeam>();

        #endregion

        #region Singleton

        private static TeamManager _instance = null;

        public static TeamManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeamManager();
                }

                return _instance;
            }
        }

        private TeamManager()
        {
        }

        #endregion

        #region Public Interface

        public ITeam GetTeam(ETeam teamKey)
        {
            if (!_teamEnumToImplementation.ContainsKey(teamKey))
            {
                _teamEnumToImplementation[teamKey] = CreateTeam(teamKey);
            }

            return _teamEnumToImplementation[teamKey];
        }

        public TTeam GetTeam<TTeam>(ETeam teamKey) where TTeam : ITeam
        {
            return (TTeam)( GetTeam(teamKey) );
        }

        #endregion

        #region Private Methods

        private ITeam CreateTeam(ETeam teamKey)
        {
            switch (teamKey)
            {
                case ETeam.Player:
                    return new PlayerTeam();
                
                default:
                    break;
            }
            
            return null;
        }

        #endregion
	}
}
