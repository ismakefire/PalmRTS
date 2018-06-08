using System;
using Misner.Utility.Collections;

namespace Misner.PalmRTS.Actor
{
	public class ActorSelectionModel
	{
        #region Variables

        private readonly HashList<ActorBehavior> _currentSelection = new HashList<ActorBehavior>();

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                return _currentSelection.Count;
            }
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            _currentSelection.Clear();
        }

        public void Set(ActorBehavior actorBehavior)
        {
            _currentSelection.Clear();
            _currentSelection.Add(actorBehavior);
        }

        public void Remove(ActorBehavior actorBehavior)
        {
            _currentSelection.Remove(actorBehavior);
        }

        public void Toggle(ActorBehavior actorBehavior)
        {
            if (_currentSelection.Contains(actorBehavior))
            {
                _currentSelection.Remove(actorBehavior);
            }
            else
            {
                _currentSelection.Add(actorBehavior);
            }
        }

        public bool Contains(ActorBehavior actorBehavior)
        {
            return _currentSelection.Contains(actorBehavior);
        }

        #endregion
    }
}
