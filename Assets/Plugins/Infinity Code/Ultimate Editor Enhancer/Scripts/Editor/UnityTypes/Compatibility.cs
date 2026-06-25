/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections;
using InfinityCode.UltimateEditorEnhancer.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace InfinityCode.UltimateEditorEnhancer.UnityTypes
{
    [HideInIntegrity]
    public static class Compatibility
    {
        public static int GetObjectId(Object obj)
        {
            if (obj == null) return 0;

#if UNITY_6000_3_OR_NEWER
            return GetObjectId(obj.GetEntityId());
#else
            return obj.GetInstanceID();
#endif
        }

#if UNITY_6000_3_OR_NEWER
        public static int GetObjectId(EntityId entityId)
        {
            return unchecked((int)EntityId.ToULong(entityId));
        }
#endif

        public static Object EntityIdToObject(int id)
        {
#if UNITY_6000_3_OR_NEWER
            return EditorUtility.EntityIdToObject(ToEntityId(id));
#else
            return EditorUtility.InstanceIDToObject(id);
#endif
        }

#if UNITY_6000_3_OR_NEWER
        public static Object EntityIdToObject(EntityId id)
        {
            return EditorUtility.EntityIdToObject(id);
        }
#endif

        public static string GetAssetPath(int instanceId)
        {
#if UNITY_6000_3_OR_NEWER
            return AssetDatabase.GetAssetPath(ToEntityId(instanceId));
#else
            return AssetDatabase.GetAssetPath(instanceId);
#endif
        }

        public static bool IsLoadingAssetPreview(Object obj)
        {
            if (obj == null) return false;

#if UNITY_6000_3_OR_NEWER
            return AssetPreview.IsLoadingAssetPreview(obj.GetEntityId());
#else
            return AssetPreview.IsLoadingAssetPreview(obj.GetInstanceID());
#endif
        }

        public static void PingObject(Object obj)
        {
            if (obj == null) return;

#if UNITY_6000_3_OR_NEWER
            EditorGUIUtility.PingObject(obj.GetEntityId());
#else
            EditorGUIUtility.PingObject(obj.GetInstanceID());
#endif
        }

#if UNITY_6000_3_OR_NEWER
        public static EntityId ToEntityId(int id)
        {
            return EntityId.FromULong(unchecked((ulong)(uint)id));
        }
#endif
    }
}
