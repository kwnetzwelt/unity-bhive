using System;
using System.Collections.Generic;

namespace BHive
{
    [Serializable]
	public class BHiveNodeConfig
	{
		
		public int Id;

        public UnityEngine.Vector2 Position;

        public List<BHiveDataContainer.DataEntry> Configuration = new List<BHiveDataContainer.DataEntry>();
		public string SerializedTypeName;

        System.Type mInternalType;
        string mInternalDescription;

        public string CustomDescription;


        
        public bool IsBroken
        {
            get;
            set;
        }

        public bool isCondition;
        public bool isDefault;

        public int positiveChild
        {
            get
            {
                return Children[0];
            }
            set
            {
                Children[0] = value;
            }

        }
        public int negativeChild
        {
            get
            {
                return Children[1];
            }
            set
            {
                Children[1] = value;
            }
        }

		public System.Type InferType()
		{
            if (mInternalType == null)
            {
                try
                {
                    mInternalType = Type.GetType(SerializedTypeName, true);
                }catch(Exception e)
                {
                    IsBroken = true;
                    mInternalType = typeof(Nullable);
                    throw e;
                }
            }
            return mInternalType;

		}
		public void SetType(System.Type pType)
		{
			SerializedTypeName = pType.AssemblyQualifiedName;
            mInternalType = pType;
		}


        public string GetDescription()
        {
            if(mInternalDescription == null)
            {
                var attr = System.Attribute.GetCustomAttribute(InferType(), typeof(BHiveInfoAttribute)) as BHiveInfoAttribute;
                if (attr != null)
                    mInternalDescription = attr.description;
                else
                    mInternalDescription = "";
            }
            return mInternalDescription;
        }

        public int[] Children = new int[2];
        /*
        public void SortChild(BHiveNodeConfig nodeConfig, int p)
        {
            int idx = Children.IndexOf(nodeConfig.Id);
            if (idx == -1)
                throw new Exception("Child not found!");

            Children.Remove(nodeConfig.Id);
            idx = idx + p;
            if (idx < 0)
                idx = 0;
            if (idx > (Children.Count))
                idx = Children.Count;
            Children.Insert(idx, nodeConfig.Id);
        }*/
    }



}

