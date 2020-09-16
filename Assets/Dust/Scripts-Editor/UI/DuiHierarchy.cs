using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [InitializeOnLoad]
    public class DuiHierarchy
    {
        public static bool isEnabled = true;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        static DuiHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGUI;

            if (!isEnabled)
                return;

            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        public static void OnHierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            try
            {
                var obj = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

                if (Dust.IsNull(obj))
                    return;

                float iconWidth = selectionRect.size.y;
                float iconHeight = selectionRect.size.y;

                Rect iconRect = new Rect()
                {
                    x = selectionRect.position.x + selectionRect.size.x - iconWidth,
                    y = selectionRect.position.y,
                    width = iconWidth,
                    height = iconHeight,
                };

                var list = obj.GetComponents(typeof(Component));

                for (int i = list.Length - 1; i >= 0; i--)
                {
                    Texture iconTexture = Icons.GetTextureByComponent(list[i]);

                    if (Dust.IsNull(iconTexture))
                        continue;

                    DustGUI.Image(iconRect, iconTexture);

                    iconRect.x -= iconWidth;
                }
            }
            catch (Exception exception)
            {
                Dust.Debug.Error("DuiHierarchy: " + exception.ToString());
            }
        }
    }
}
#endif
