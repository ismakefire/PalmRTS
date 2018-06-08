using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Actor
{
	public class ActorModelManager
	{
        #region Singleton

        private static ActorModelManager _instance = null;

        public static ActorModelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActorModelManager();
                }

                return _instance;
            }
        }

        private ActorModelManager()
        {
        }

        #endregion

        #region Variables

        private List<ActorBehavior> _actorBehaviors = new List<ActorBehavior>();

        #endregion

        #region Public Interface

		public void Add(ActorBehavior actorBehavior)
        {
            _actorBehaviors.Add(actorBehavior);

            Debug.LogFormat("{0}.Add(), _actorBehaviors.Count = {1}", this.ToString(), _actorBehaviors.Count);
        }

        #endregion
	}
}
