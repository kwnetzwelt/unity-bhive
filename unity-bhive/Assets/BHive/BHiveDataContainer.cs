using System;
using System.Collections.Generic;

namespace BHive
{
	public abstract class BHiveDataContainer
	{
        [Serializable]
        public class DataEntry
        {
            public string Key;
            public string Value;
        }
        protected Dictionary<string, string> configuration = new Dictionary<string, string>();

		/// <summary>
		/// Loads the configuration.
		/// </summary>
		/// <param name="pJsonConfiguration">P json configuration.</param>
		public void LoadConfiguration(List<DataEntry> pConfiguration)
		{
            configuration.Clear();
            foreach(var k in pConfiguration)
            {
                configuration[k.Key] = k.Value.ToString();
            }
			
		}
		
		public float GetFloat(string pKey)
		{
			if(configuration.ContainsKey(pKey))
			{
				float value = -1;
				if(float.TryParse( configuration[pKey], out value))
					return value;
                //throw new FormatException(string.Format( "Key {0} is not of requested type. ", pKey));
			}
			//throw new KeyNotFoundException(pKey + " not found");
            return 0;
		}
		
		public int GetInt(string pKey)
		{
			if(configuration.ContainsKey(pKey))
			{
				int value = -1;
				if(int.TryParse( configuration[pKey], out value))
					return value;
				//throw new FormatException("Key is not of requested type. ");
			}
			//throw new KeyNotFoundException(pKey + " not found");
            return 0;
		}

		public void SetValue(string pKey, object pValue)
		{
			configuration[pKey] = pValue.ToString();
		}

		public string GetString(string pKey)
		{
			if(configuration.ContainsKey(pKey))
				return configuration[pKey];
			//throw new KeyNotFoundException(pKey + " not found");
            return "";
		}

	}
}

