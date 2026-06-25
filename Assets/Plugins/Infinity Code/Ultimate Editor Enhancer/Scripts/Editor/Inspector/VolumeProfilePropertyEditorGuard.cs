/*           INFINITY CODE          */
/*     https://infinity-code.com    */

#if UNITY_6000_3_OR_NEWER
using System.Reflection;
using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.InspectorTools
{
    [InitializeOnLoad]
    public static class VolumeProfilePropertyEditorGuard
    {
        private const int MaxUpdatePasses = 20;
        private static int updatePassesRemaining;

        private const string InspectorWindowTypeName = "UnityEditor.InspectorWindow";
        private const string PropertyEditorTypeName = "UnityEditor.PropertyEditor";
        private const string VolumeComponentTypeName = "UnityEngine.Rendering.VolumeComponent";
        private const string VolumeProfileTypeName = "UnityEngine.Rendering.VolumeProfile";

        static VolumeProfilePropertyEditorGuard()
        {
            ScheduleCleanup();
            Selection.selectionChanged += ScheduleCleanup;
            EditorApplication.playModeStateChanged += _ => ScheduleCleanup();
            EditorApplication.update += CleanupDuringUpdate;
        }

        private static void ScheduleCleanup()
        {
            updatePassesRemaining = MaxUpdatePasses;
            ClearInvalidVolumeEditors();
            EditorApplication.delayCall += ClearInvalidVolumeEditors;
        }

        private static void CleanupDuringUpdate()
        {
            if (updatePassesRemaining <= 0) return;
            updatePassesRemaining--;
            ClearInvalidVolumeEditors();
        }

        private static void ClearInvalidVolumeEditors()
        {
            foreach (EditorWindow window in UnityEngine.Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (window == null) continue;
                if (!CanContainInspectedEditors(window)) continue;

                ActiveEditorTracker tracker = GetTracker(window);
                if (tracker == null) continue;
                if (!HasBrokenVolumeEditor(tracker)) continue;

                ActiveEditorTrackerRef.SetObjectsLockedByThisTracker(tracker, new List<Object>());
                tracker.ForceRebuild();
                window.Repaint();
            }
        }

        private static bool CanContainInspectedEditors(EditorWindow window)
        {
            string typeName = window.GetType().FullName;
            return typeName == InspectorWindowTypeName || typeName == PropertyEditorTypeName;
        }

        private static bool HasBrokenVolumeEditor(ActiveEditorTracker tracker)
        {
            bool hasBrokenEditor = false;
            bool hasVolumeEditor = false;

            foreach (Editor editor in tracker.activeEditors)
            {
                if (editor == null)
                {
                    hasBrokenEditor = true;
                    continue;
                }

                if (IsVolumeEditor(editor.GetType().FullName)) hasVolumeEditor = true;

                Object target = null;
                try
                {
                    target = editor.target;
                }
                catch
                {
                    hasBrokenEditor = true;
                }

                if (target == null)
                {
                    hasBrokenEditor = true;
                    continue;
                }

                if (IsVolumeObject(target)) hasVolumeEditor = true;
            }

            return hasBrokenEditor && hasVolumeEditor;
        }

        private static bool IsVolumeEditor(string editorTypeName)
        {
            return !string.IsNullOrEmpty(editorTypeName)
                && editorTypeName.StartsWith("UnityEditor.Rendering.", System.StringComparison.Ordinal)
                && editorTypeName.EndsWith("Editor", System.StringComparison.Ordinal);
        }

        private static bool IsVolumeObject(Object target)
        {
            System.Type targetType = target.GetType();
            while (targetType != null)
            {
                string typeName = targetType.FullName;
                if (typeName == VolumeProfileTypeName || typeName == VolumeComponentTypeName) return true;
                targetType = targetType.BaseType;
            }

            return false;
        }

        private static ActiveEditorTracker GetTracker(EditorWindow window)
        {
            PropertyInfo trackerProperty = window.GetType().GetProperty(
                "tracker",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return trackerProperty != null
                ? trackerProperty.GetValue(window) as ActiveEditorTracker
                : null;
        }
    }
}
#endif
