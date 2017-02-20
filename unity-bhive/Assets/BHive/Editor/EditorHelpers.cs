using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace BHive.Editor
{
    public class EditorHelpers
    {

        const string ResourcesPrefix = "Resources/";

        public static string CreateAssetPathAtcurrentLocation(string name)
        {
            string localPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            localPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            FileInfo fi = new FileInfo(localPath);


            DirectoryInfo di;
            if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                di = new DirectoryInfo(localPath);
            }
            else {
                di = fi.Directory;
            }
            string assetPath = di.FullName;
#if UNITY_EDITOR_WIN
            assetPath = assetPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
#endif

            assetPath = assetPath + "/" + name;
            assetPath = "Assets" + assetPath.Substring(UnityEngine.Application.dataPath.Length);

            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            return assetPath;
        }

        internal static bool IsInResourcesPath(UnityEngine.Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);

            return path.IndexOf(ResourcesPrefix) != -1;
        }

        internal static string GetResourcesPath(UnityEngine.GameObject asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            var idx = path.IndexOf(ResourcesPrefix);
            path = path.Substring(idx + ResourcesPrefix.Length);
            return path.Substring(0, path.LastIndexOf("."));
        }
    }

}