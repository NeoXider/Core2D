/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public class HierarchyItem
    {
        public int id;
        public Rect rect;
        public GameObject gameObject;
        public Object target;
        public bool hovered;
        public bool selected;

#if UNITY_6000_3_OR_NEWER
        public void Set(EntityId entityId, Rect rect)
        {
            id = unchecked((int)EntityId.ToULong(entityId));
            this.rect = rect;

            target = EditorUtility.EntityIdToObject(entityId);
            gameObject = target as GameObject;

            Vector2 p = Event.current.mousePosition;
            hovered = p.x >= 0 && p.x <= rect.xMax + 16 && p.y >= rect.y && p.y < rect.yMax;
            selected = Selection.entityIds.Contains(entityId);
        }
#endif

        public void Set(int id, Rect rect)
        {
            this.id = id;
            this.rect = rect;

            target = Compatibility.EntityIdToObject(id);
            gameObject = target as GameObject;

            Vector2 p = Event.current.mousePosition;
            hovered = p.x >= 0 && p.x <= rect.xMax + 16 && p.y >= rect.y && p.y < rect.yMax;

#if UNITY_6000_3_OR_NEWER
            selected = Selection.entityIds.Contains(EntityId.FromULong((ulong)(uint)id));
#else
            selected = Selection.instanceIDs.Contains(id);
#endif
        }
    }
}