using System;
using System.Collections.Generic;


namespace BHive
{
	public abstract class BHiveNode : BHiveDataContainer
	{

		public BHiveNode()
		{
		}

        public int Parent = -1;
        
        

        [NonSerialized]
        public UnityEngine.GameObject Target;

		public BHiveController Controller
		{
			get;
			set;
		}

        public abstract BHiveNode Tick();

		public class BHiveNodeType
		{
			public string title { get; set;}
			public string description { get; set; }
			public System.Type type { get; set;}
		}

		public static IEnumerable<BHiveNodeType> GetAllTypes(System.Type pFilter)
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (var t in assembly.GetExportedTypes()) {
                    if (pFilter.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        

						BHiveInfoAttribute attr = System.Attribute.GetCustomAttribute(t, typeof(BHiveInfoAttribute)) as BHiveInfoAttribute;
						BHiveNodeType outType = new BHiveNodeType();

						if (attr != null) {
							outType.title = attr.title;
							outType.description = attr.description;
						} else {
							outType.title = t.Name;
							outType.description = t.FullName;
						}
						outType.type = t;

						yield return outType;
					}
				}
			}
			yield break;
		}


        /// <summary>
        /// Called once after instance is being created and deserialized
        /// </summary>
        public virtual void Initialize(BHiveNodeConfig config)
        {
            LoadConfiguration(config.Configuration);
            Name = GetType().Name;
            Id = config.Id;
                
        }

        public string Name
        {
            get;
            private set;
        }

        public int Id
        {
            get;
            private set;
        }
    }
}

