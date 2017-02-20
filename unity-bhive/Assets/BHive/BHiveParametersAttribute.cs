using System;
namespace BHive
{
	public class BHiveParametersAttribute : Attribute
	{
		public string[] Configuration
		{
			get;
			set;
		}

		public BHiveParametersAttribute(params string[] pConfiguration)
		{
			Configuration = pConfiguration;
		}

	}

}

