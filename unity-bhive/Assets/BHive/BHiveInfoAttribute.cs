using System;
using System.Collections.Generic;

namespace BHive
{
	public class BHiveInfoAttribute : Attribute
	{
		public string title {
			get;
			set;
		}
		public string description {
			get;
			set;
		}

		public BHiveInfoAttribute(string pTitle)
		{
			title = pTitle;
            description = "";
		}

		public BHiveInfoAttribute(string pTitle, string pDescription)
		{
			title = pTitle;
			description = pDescription;
		}
	}

}

