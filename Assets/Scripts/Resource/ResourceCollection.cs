using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Resource
{
	public class ResourceCollection
    {
        #region Variables

		private readonly Dictionary<EResourceItem, int> _collection = new Dictionary<EResourceItem, int>();

        #endregion

        #region Public Interface

        public event Action Changed;

		public int Get(EResourceItem item)
		{
            if (_collection.ContainsKey(item))
            {
                return _collection[item];
            }
            else
            {
                return 0;
            }
		}

		public void Set(EResourceItem item, int newValue)
		{
            if (Get(item) != newValue)
            {
				_collection[item] = newValue;

                if (Changed != null)
                {
                    Changed();
                }
            }
		}

        public void Add(EResourceItem resourceKey, int amountToAdd)
        {
            if (amountToAdd > 0)
            {
                int previousAmount = Get(resourceKey);
                int newAmount = previousAmount + amountToAdd;

                Set(resourceKey, newAmount);
            }
        }

        public bool Remove(EResourceItem resourceKey, int amountToRemove)
        {
            bool didWeRemoveAnything = false;
            
            if (amountToRemove > 0)
            {
                int previousAmount = Get(resourceKey);
                int newAmount = previousAmount - amountToRemove;

                if (newAmount >= 0)
                {
                    Set(resourceKey, newAmount);
					didWeRemoveAnything = true;
                }
            }

            return didWeRemoveAnything;
        }

        public bool Has(EResourceItem item)
        {
            return (Get(item) > 0);
        }

        public void Print()
        {
            string values = "";

            foreach (EResourceItem itemKey in _collection.Keys)
            {
                int itemValue = _collection[itemKey];

                if (values.Length > 1)
                {
                    values += ", ";
                }

                values += string.Format("({0}, {1})", itemKey, itemValue);
            }

            Debug.LogFormat("<color=#ff00ff>{0}.Print(), _collection.Keys.Count = {1}, values = ( {2} )</color>", this.ToString(), _collection.Keys.Count, values);
        }

        #endregion
	}
}
