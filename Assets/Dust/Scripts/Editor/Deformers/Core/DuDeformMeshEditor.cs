using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDeformMesh)), CanEditMultipleObjects]
    public class DuDeformMeshEditor : DuEditor
    {
        private const float CELL_WIDTH_ICON = 32f;
        private const float CELL_WIDTH_STATE = 20f;
        private const float CELL_WIDTH_INTENSITY = 54f;

        //--------------------------------------------------------------------------------------------------------------

        private DuProperty m_Deformers;

        private DuProperty m_RecalculateBounds;
        private DuProperty m_RecalculateNormals;
        private DuProperty m_RecalculateTangents;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private GUIStyle m_StyleMiniButton = GUIStyle.none;
        private GUIStyle styleMiniButton
        {
            get
            {
                if (m_StyleMiniButton == GUIStyle.none)
                    m_StyleMiniButton = DustGUI.NewStyleButton().Padding(2, 0).Margin(0).Build();

                return m_StyleMiniButton;
            }
        }

        private GUIStyle m_StyleIntensityButton = GUIStyle.none;
        private GUIStyle styleIntensityButton
        {
            get
            {
                if (m_StyleIntensityButton == GUIStyle.none)
                    m_StyleIntensityButton = DustGUI.NewStyleButton().MarginTop(6).Build();

                return m_StyleIntensityButton;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly Dictionary<string, Rect> m_RectsUI = new Dictionary<string, Rect>();

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_Deformers = FindProperty("m_Deformers", "Deformers");

            m_RecalculateBounds = FindProperty("m_RecalculateBounds", "Recalculate Bounds");
            m_RecalculateNormals = FindProperty("m_RecalculateNormals", "Recalculate Normals");
            m_RecalculateTangents = FindProperty("m_RecalculateTangents", "Recalculate Tangents");
        }

        public override void OnInspectorGUI()
        {
            var main = target as DuDeformMesh;

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Dust.IsNull(main.meshOriginal))
            {
                if (Dust.IsNotNull(main.meshFilter.sharedMesh) && !main.meshFilter.sharedMesh.isReadable)
                {
                    DustGUI.HelpBoxError("Mesh is not readable." + "\n" + "Enabled \"Read/Write Enabled\" flag for this mesh.");
                }
                else if (main.enabled)
                {
                    main.ReEnableMeshForDeformer();
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Deformers", "DuDeformMesh.Deformers"))
            {
                OptimizeDeformersArray();

                Vector2 scrollPosition = DuSessionState.GetVector3("DuDeformMesh.Deformers.ScrollPosition", target, Vector2.zero);
                float totalHeight = 24 + 36 * Mathf.Clamp(m_Deformers.property.arraySize + 1, 4, 8) + 16;

                int indentLevel = DustGUI.IndentLevelReset(); // Because it'll try to add left-spacing when draw deformers
                Rect rect = DustGUI.BeginVerticalBox();
                DustGUI.BeginScrollView(ref scrollPosition, 0, totalHeight);
                {
                    for (int i = 0; i < m_Deformers.property.arraySize; i++)
                    {
                        SerializedProperty item = m_Deformers.property.GetArrayElementAtIndex(i);

                        if (DrawDeformerItem(item, i, m_Deformers.property.arraySize))
                        {
                            m_Deformers.isChanged = true;
                            EditorUtility.SetDirty(this);
                            break; // stop update anything... in next update it will redraw real state
                        }
                    }

                    DrawAddDeformerButton();
                }
                DustGUI.EndScrollView();
                DustGUI.EndVertical();
                DustGUI.IndentLevelReset(indentLevel);

                Space();

                // PropertyField(m_Deformers);
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (rect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.DragPerform)
                    {
                        AddDeformersFromObjectsList(DragAndDrop.objectReferences);
                        Event.current.Use();
                    }
                }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DuSessionState.SetVector3("DuDeformMesh.Deformers.ScrollPosition", target, scrollPosition);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_RecalculateBounds);
            PropertyField(m_RecalculateNormals);
            PropertyField(m_RecalculateTangents);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int verticesCount = main.GetMeshVerticesCount();

            if (verticesCount >= 0)
            {
                Space();
                DustGUI.HelpBoxInfo("Mesh has " + verticesCount + " vertices");
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string statsInfo = "STATS:" + "\n";
            statsInfo += "Mesh updates: " + main.stats.updatesCount + "\n";
            statsInfo += "Last update: " + main.stats.lastUpdateTime + " sec";

            DustGUI.HelpBoxWarning(statsInfo);
            this.Repaint();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Require forced redraw scene view

            DustGUI.ForcedRedrawSceneView();
        }

        private DuDeformMesh.Record UnpackDeformerRecord(SerializedProperty item)
        {
            var record = new DuDeformMesh.Record();
            record.enabled = item.FindPropertyRelative("m_Enabled").boolValue;
            record.deformer = item.FindPropertyRelative("m_Deformer").objectReferenceValue as DuDeformer;
            record.intensity = item.FindPropertyRelative("m_Intensity").floatValue;
            return record;
        }

        private bool DrawDeformerItem(SerializedProperty item, int itemIndex, int itemsCount)
        {
            var curRecord = UnpackDeformerRecord(item); // just to save previous state
            var newRecord = UnpackDeformerRecord(item); // this record will fix changes

            if (Dust.IsNull(newRecord.deformer))
                return false; // Notice: but it should never be this way!

            bool clickOnDelete;
            bool clickOnMoveUp;
            bool clickOnMoveDw;

            DustGUI.BeginHorizontal();
            {
                var deformerEnabledInScene = newRecord.deformer.enabled && newRecord.deformer.gameObject.activeInHierarchy;
                var deformerIcon = Icons.GetTextureByComponent(newRecord.deformer, !deformerEnabledInScene ? "Disabled" : "");

                if (DustGUI.IconButton(deformerIcon, CELL_WIDTH_ICON, CELL_WIDTH_ICON, styleMiniButton))
                    Selection.activeGameObject = newRecord.deformer.gameObject;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var btnStateIcon = newRecord.enabled ? Icons.STATE_ENABLED : Icons.STATE_DISABLED;

                if (DustGUI.IconButton(btnStateIcon, CELL_WIDTH_STATE, 32f, styleMiniButton))
                    newRecord.enabled = !newRecord.enabled;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Lock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.SimpleLabel(newRecord.deformer.DeformerName(), 0, DustGUI.Config.ICON_BUTTON_HEIGHT);

                DustGUI.SpaceExpand();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                string intensityValue = newRecord.intensity.ToString("F2");

                if (DustGUI.Button(intensityValue, CELL_WIDTH_INTENSITY, 20f, styleIntensityButton, DustGUI.ButtonState.Pressed))
                {
                    Rect buttonRect = m_RectsUI["item" + itemIndex.ToString()];
                    buttonRect.y += 5f;

                    PopupWindow.Show(buttonRect, DuPopupExtraSlider.Create(serializedObject, "Intensity", item.FindPropertyRelative("m_Intensity")));
                }

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["item" + itemIndex.ToString()] = GUILayoutUtility.GetLastRect();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Unlock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                clickOnDelete = DustGUI.IconButton(Icons.ACTION_DELETE, 20, 32, styleMiniButton);

                DustGUI.BeginVertical(20);
                {
                    DustGUI.ButtonState stateUp = itemIndex > 0 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;
                    DustGUI.ButtonState stateDw = itemIndex < itemsCount - 1 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;

                    clickOnMoveUp = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_UP, 20, 16, styleMiniButton, stateUp);
                    clickOnMoveDw = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_DOWN, 20, 16, styleMiniButton, stateDw);
                }
                DustGUI.EndVertical();
            }
            DustGUI.EndHorizontal();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Actions

            if (curRecord.enabled != newRecord.enabled) {
                item.FindPropertyRelative("m_Enabled").boolValue = newRecord.enabled;
                return true;
            }

            if (!curRecord.intensity.Equals(newRecord.intensity)) {
                item.FindPropertyRelative("m_Intensity").floatValue = newRecord.intensity;
                return true;
            }

            if (clickOnDelete) {
                m_Deformers.property.DeleteArrayElementAtIndex(itemIndex);
                return true;
            }

            if (clickOnMoveUp) {
                m_Deformers.property.MoveArrayElement(itemIndex, itemIndex - 1);
                return true;
            }

            if (clickOnMoveDw) {
                m_Deformers.property.MoveArrayElement(itemIndex, itemIndex + 1);
                return true;
            }

            return false;
        }

        private void DrawAddDeformerButton()
        {
            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(Icons.ACTION_ADD_DEFORMER, CELL_WIDTH_ICON, CELL_WIDTH_ICON, styleMiniButton))
                    PopupWindow.Show(m_RectsUI["Add"], DuPopupButtons.Deformers(this));

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["Add"] = GUILayoutUtility.GetLastRect();

                DustGUI.Label("Add Deformer", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
            }
            DustGUI.EndHorizontal();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void AddDeformersFromObjectsList(Object[] objectReferences)
        {
            if (Dust.IsNull(objectReferences) || objectReferences.Length == 0)
                return;

            foreach (Object obj in objectReferences)
            {
                if (obj is GameObject)
                {
                    DuDeformer[] deformers = (obj as GameObject).GetComponents<DuDeformer>();

                    foreach (var deformer in deformers)
                    {
                        AddDeformer(deformer);
                    }
                }
                else if (obj is DuDeformer)
                {
                    AddDeformer(obj as DuDeformer);
                }
            }
        }

        private void AddDeformer(DuDeformer deformer)
        {
            int count = m_Deformers.property.arraySize;

            for (int i = 0; i < count; i++)
            {
                SerializedProperty fldRecord = m_Deformers.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(fldRecord))
                    continue;

                SerializedProperty deformerObject = fldRecord.FindPropertyRelative("m_Deformer");

                if (Dust.IsNull(deformerObject) || !deformerObject.objectReferenceValue.Equals(deformer))
                    continue;

                return; // No need to insert 2nd time
            }

            m_Deformers.property.InsertArrayElementAtIndex(count);

            var defaultRec = new DuFieldsMap.FieldRecord();

            SerializedProperty newRecord = m_Deformers.property.GetArrayElementAtIndex(count);

            newRecord.FindPropertyRelative("m_Enabled").boolValue = defaultRec.enabled;
            newRecord.FindPropertyRelative("m_Deformer").objectReferenceValue = deformer;
            newRecord.FindPropertyRelative("m_Intensity").floatValue = defaultRec.intensity;

            serializedObject.ApplyModifiedProperties();
        }

        private void OptimizeDeformersArray()
        {
            bool changed = false;

            for (int i = m_Deformers.property.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty fldRecord = m_Deformers.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(fldRecord))
                    continue;

                SerializedProperty deformerObject = fldRecord.FindPropertyRelative("m_Deformer");

                if (Dust.IsNotNull(deformerObject) && Dust.IsNull(deformerObject.objectReferenceValue))
                {
                    m_Deformers.property.DeleteArrayElementAtIndex(i);
                    changed = true;
                }
            }

            if (!changed)
                return;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
