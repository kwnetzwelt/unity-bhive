using System;
using System.Collections.Generic;
using UnityEngine;

namespace BHive
{
	[Serializable]
	public class BHiveConfiguration : ScriptableObject
	{
		public List<BHiveNodeConfig> Nodes = new List<BHiveNodeConfig>();
		public List<BHiveNode.DataEntry> Configuration = new List<BHiveNode.DataEntry>();

		public BHiveNodeConfig GetNodeById(int id)
		{
			return Nodes.Find( x => x.Id == id);
		}


        public int GetNextNodeId()
        {
            int i = 0;
            Nodes.ForEach((x) => { i = Mathf.Max(x.Id, i); });
            return ++i;
        }
    }
}

