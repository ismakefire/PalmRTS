using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misner.PalmRTS.Resource
{
    internal class OptionEntry
    {
        #region Variables

		private static List<OptionEntry> _optionEntries = null;
		
		public readonly int Index;
		public readonly EResourceItem? Resource;
		public readonly string Name;

        #endregion

        #region Methods

		private OptionEntry(int index, EResourceItem? resource, string name)
		{
			this.Index = index;
			this.Resource = resource;
			this.Name = name;
		}

        internal static List<OptionEntry> GetOptionEntries()
        {
            if (_optionEntries == null)
            {
                _optionEntries = new List<OptionEntry>();

                _optionEntries.Add(new OptionEntry(_optionEntries.Count, null, "None (item)"));

                foreach (EResourceItem resourceItem in ResourceItemUtil.GetAll())
                {
                    // TODO: Use a better user facing string.
                    _optionEntries.Add(new OptionEntry(_optionEntries.Count, resourceItem, resourceItem.ToString()));
                }
            }

            return _optionEntries;
        }

        #endregion
    }

	public static class ResourceItemUtil
    {
        public static List<string> GenerateObjectDropdownOptions()
        {
            List<string> optionNames = new List<string>();

            foreach (OptionEntry option in OptionEntry.GetOptionEntries())
            {
                optionNames.Add(option.Name);
            }

            return optionNames;
        }

        public static EResourceItem? GetResourceFromDropdownOptionIndex(int index)
        {
            EResourceItem? result = null;
            
            List<OptionEntry> optionEntries = OptionEntry.GetOptionEntries();

            if (0 <= index && index < optionEntries.Count)
            {
                OptionEntry entry = optionEntries[index];

                result = entry.Resource;
            }

            return result;
        }

        public static List<EResourceItem> GetAll()
        {
            List<EResourceItem> result = new List<EResourceItem>()
            {
                EResourceItem.SolidRock,
                EResourceItem.CrushedRock,
                EResourceItem.MetalPlate,
                EResourceItem.MetalBox,
            };
			
            return result;
        }
	}
}
