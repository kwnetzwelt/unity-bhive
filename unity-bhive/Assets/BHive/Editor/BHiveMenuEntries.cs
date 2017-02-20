using System;
using UnityEditor;
using System.IO;

namespace BHive.Editor
{
	public class BHiveMenuEntries
	{
		[MenuItem("Assets/Create/BHiveConfiguration")]
		public static void CreateConfiguration()
		{
            string path = EditorHelpers.CreateAssetPathAtcurrentLocation("BHiveConfig.asset");
            var asset = BHiveConfiguration.CreateInstance<BHiveConfiguration>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.Refresh();

			


		}

	}
}

