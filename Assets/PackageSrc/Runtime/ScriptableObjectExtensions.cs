using UnityEditor;
using UnityEngine;

namespace UAppToolkit.SubAssets
{
    public static class ScriptableObjectExtensions 
    {
        public static T AddSubAsset<T>(this ScriptableObject scriptableObject, string subAssetName) where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T>();
            instance.name = subAssetName;
            AssetDatabase.AddObjectToAsset(instance, scriptableObject);
            EditorUtility.SetDirty(scriptableObject);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(scriptableObject));
            return instance;
        }

        public static void RemoveSubAsset(this ScriptableObject subAsset)
        {
            var mainAsset = subAsset.GetMainAsset();
            if (mainAsset == null)
            {
                Debug.LogWarning("Can not find main asset for sub-asset", subAsset);
                return;
            }

            if (mainAsset == subAsset)
            {
                Debug.LogWarning("Can not delete main asset", subAsset);
                return;
            }

            AssetDatabase.RemoveObjectFromAsset(subAsset);
            EditorUtility.SetDirty(mainAsset);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mainAsset));
        }

        public static ScriptableObject GetMainAsset(this ScriptableObject subAsset)
        {
            var assetPath = AssetDatabase.GetAssetPath(subAsset);
            if (string.IsNullOrWhiteSpace(assetPath))
            {
                return null;
            }
            return AssetDatabase.LoadMainAssetAtPath(assetPath) as ScriptableObject;
        }
    }
}
