using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Postica.Common
{
    internal static class UnityObjectEditorCompat
    {
        internal static int GetObjectId(Object obj)
        {
#if UNITY_6000_3_OR_NEWER
            return obj == null ? 0 : unchecked((int)EntityId.ToULong(obj.GetEntityId()));
#else
            return obj == null ? 0 : obj.GetInstanceID();
#endif
        }

        internal static bool IsLoadingAssetPreview(Object obj)
        {
#if UNITY_6000_3_OR_NEWER
            return AssetPreview.IsLoadingAssetPreview(obj.GetEntityId());
#else
            return AssetPreview.IsLoadingAssetPreview(obj.GetInstanceID());
#endif
        }
    }
}
