using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    internal static class ObjectReferenceId
    {
        internal static long GetSerializedId(Object obj)
        {
            if (!obj)
            {
                return 0;
            }

#if UNITY_6000_3_OR_NEWER
            return unchecked((long)EntityId.ToULong(obj.GetEntityId()));
#else
            return obj.GetInstanceID();
#endif
        }
    }
}
