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
            _collection[item] = newValue;
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
