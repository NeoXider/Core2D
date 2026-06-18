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
        public static Object EntityIdToObject(int id)
        {
#if UNITY_6000_3_OR_NEWER
            return EditorUtility.EntityIdToObject(EntityId.FromULong((ulong)(uint)id));
#else
            return EditorUtility.InstanceIDToObject(id);
#endif
        }

        public static Object EntityIdToObject(long rawId)
        {
#if UNITY_6000_3_OR_NEWER
            return rawId != 0 ? EditorUtility.EntityIdToObject(EntityId.FromULong((ulong)rawId)) : null;
#else
            return rawId != 0 ? EditorUtility.InstanceIDToObject((int)rawId) : null;
#endif
        }

        public static Object EntityIdToObject(EntityId entityId)
        {
#if UNITY_6000_3_OR_NEWER
            return EditorUtility.EntityIdToObject(entityId);
#else
            return null;
#endif
        }

        public static long ToRawId(EntityId entityId)
        {
#if UNITY_6000_3_OR_NEWER
            return unchecked((long)EntityId.ToULong(entityId));
#else
            return 0;
#endif
        }

        public static EntityId FromRawId(long rawId)
        {
#if UNITY_6000_3_OR_NEWER
            return EntityId.FromULong((ulong)rawId);
#else
            return default;
#endif
        }

        public static string GetAssetPath(int instanceId)
        {
#if UNITY_6000_3_OR_NEWER
            return AssetDatabase.GetAssetPath((EntityId)instanceId);
#else
            return AssetDatabase.GetAssetPath(instanceId);
#endif
        }

        public static int GetObjectId(Object obj)
        {
            if (obj == null) return 0;
#if UNITY_6000_3_OR_NEWER
            return unchecked((int)EntityId.ToULong(obj.GetEntityId()));
#else
            return obj.GetInstanceID();
#endif
        }

        public static bool IsLoadingAssetPreview(Object obj)
        {
#if UNITY_6000_3_OR_NEWER
            return AssetPreview.IsLoadingAssetPreview(obj.GetEntityId());
#else
            return AssetPreview.IsLoadingAssetPreview(obj.GetInstanceID());
#endif
        }

        public static void PingObject(Object obj)
        {
#if UNITY_6000_3_OR_NEWER
            EditorGUIUtility.PingObject(obj.GetEntityId());
#else
            EditorGUIUtility.PingObject(obj.GetInstanceID());
#endif
        }
    }
}