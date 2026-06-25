/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;
#if UNITY_6000_3_OR_NEWER
using UnityEngine;
#endif

namespace InfinityCode.UltimateEditorEnhancer.UnityTypes
{
    public static class LogEntryRef
    {
        private static FieldInfo _entityIdField;
        private static FieldInfo _instanceIDField;
        private static FieldInfo _messageField;
        private static FieldInfo _modeField;
        private static Type _type;

#if UNITY_6000_3_OR_NEWER
        private static FieldInfo entityIdField
        {
            get
            {
                if (_entityIdField == null) _entityIdField = type.GetField("entityId", Reflection.InstanceLookup);
                return _entityIdField;
            }
        }
#endif

        private static FieldInfo instanceIDField
        {
            get
            {
                if (_instanceIDField == null) _instanceIDField = type.GetField("instanceID", Reflection.InstanceLookup);
                return _instanceIDField;
            }
        }

        private static FieldInfo messageField
        {
            get
            {
                if (_messageField == null) _messageField = type.GetField("message", Reflection.InstanceLookup);
                return _messageField;
            }
        }

        private static FieldInfo modeField
        {
            get
            {
                if (_modeField == null) _modeField = type.GetField("mode", Reflection.InstanceLookup);
                return _modeField;
            }
        }

        public static Type type
        {
            get
            {
                if (_type == null)
                {
                    _type = Reflection.GetEditorType("LogEntry", "UnityEditorInternal");
                    if (_type == null) _type = Reflection.GetEditorType("LogEntry");
                }
                return _type;
            }
        }

        public static int GetMode(object instance)
        {
            if (instance == null || modeField == null) return 0;
            return (int)modeField.GetValue(instance);
        }

        public static int GetInstanceID(object instance)
        {
            if (instance == null) return 0;

#if UNITY_6000_3_OR_NEWER
            if (entityIdField != null)
            {
                object value = entityIdField.GetValue(instance);
                if (value is EntityId entityId) return Compatibility.GetObjectId(entityId);
            }
#endif

            return instanceIDField != null ? (int)instanceIDField.GetValue(instance) : 0;
        }

        public static string GetMessage(object instance)
        {
            return instance != null && messageField != null ? (string)messageField.GetValue(instance) : string.Empty;
        }
    }
}
