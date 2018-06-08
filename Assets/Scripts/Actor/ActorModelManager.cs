using System;
using System.Collections.Generic;
using Misner.PalmRTS.Util;
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

        private readonly List<ActorBehavior> _actorBehaviors = new List<ActorBehavior>();

        #endregion

        #region Public Interface

		public void Add(ActorBehavior actorBehavior)
        {
            _actorBehaviors.Add(actorBehavior);
            actorBehavior.OnClicked += OnActorClicked;

            Debug.LogFormat("{0}.Add(), _actorBehaviors.Count = {1}", this.ToString(), _actorBehaviors.Count);
        }

        #endregion

        #region Actor Events

        protected void OnActorClicked(ActorBehavior actorBehavior)
        {
            Debug.LogFormat("{0}.OnActorClicked(), (actorBehavior != null) = {1}", this.ToString(), (actorBehavior != null));
            if (actorBehavior == null)
            {
                return;
            }

            if (KeyUtil.AnyCommand)
            {
                KillActor(actorBehavior);
            }
            else if (KeyUtil.AnyOption)
            {
                Debug.LogFormat("{0}.OnActorClicked(), jump!", this.ToString());

                actorBehavior.Jump();
            }
        }

        #endregion

        #region Private Methods

        private void KillActor(ActorBehavior actorBehavior)
        {
            _actorBehaviors.Remove(actorBehavior);
            UnityEngine.Object.Destroy(actorBehavior.gameObject);

            Debug.LogFormat("{0}.KillActor(), the clicked object. _actorBehaviors.Count = {1}", this.ToString(), _actorBehaviors.Count);
        }

        #endregion
	}
}
