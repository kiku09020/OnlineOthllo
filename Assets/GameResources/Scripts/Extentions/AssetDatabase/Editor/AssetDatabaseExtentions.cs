using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Extentions.Editor {
    public class AssetDatabaseExtentions {
        public static T LoadAssetAtPath<T>(string path) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null) {
                Debug.Log($"指定されたアセットが読み込めませんでした\n({path})");
            }

            return asset;
        }
    }
}