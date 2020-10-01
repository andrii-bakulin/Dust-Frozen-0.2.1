using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuFieldsMapEditor
    {
        private const float CELL_WIDTH_ICON = 32f;
        private const float CELL_WIDTH_STATE = 20f;
        private const float CELL_WIDTH_INTENSITY = 54f;
        private const float CELL_WIDTH_BLENDING = 50f;
        private const float CELL_WIDTH_CONTROL = 40f;

        //--------------------------------------------------------------------------------------------------------------

        private DuFieldsMap m_FieldsMapInstance;

        private DuEditor m_Editor;
        private DuEditor.DuProperty m_Fields;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly Dictionary<string, Rect> m_RectsUI = new Dictionary<string, Rect>();

        public DuFieldsMapEditor(DuEditor parentEditor, SerializedProperty fieldsMapProperty, DuFieldsMap fieldsMapInstance)
        {
            m_FieldsMapInstance = fieldsMapInstance;

            m_Editor = parentEditor;
            m_Fields = DuEditor.FindProperty(parentEditor, fieldsMapProperty, "m_Fields", "Fields");
        }

        //--------------------------------------------------------------------------------------------------------------

        public GameObject GetParentGameObject()
        {
            return (m_Editor.target as DuMonoBehaviour).gameObject;
        }

        public bool calculatePower => m_FieldsMapInstance.calculatePower;
        public bool calculateColor => m_FieldsMapInstance.calculateColor;

        //--------------------------------------------------------------------------------------------------------------

        public void OnInspectorGUI()
        {
            if (DustGUI.FoldoutBegin("Fields Map", "DuFieldsMap.Main"))
            {
                OptimizeFieldsArray();

                Vector2 scrollPosition = DuSessionState.GetVector3("DuFieldsMap.Fields.ScrollPosition", m_Editor.target, Vector2.zero);
                float totalHeight = 24 + 36 * Mathf.Clamp(m_Fields.property.arraySize + 1, 4, 8) + 16;

                int indentLevel = DustGUI.IndentLevelReset(); // Because it'll try to add left-spacing when draw fields
                Rect rect = DustGUI.BeginVerticalBox();
                DustGUI.BeginScrollView(ref scrollPosition, 0, totalHeight);
                {
                    DustGUI.BeginHorizontal();
                    {
                        float padding = 2;
                        DustGUI.Header("", CELL_WIDTH_ICON - padding);
                        DustGUI.Header("", CELL_WIDTH_STATE - padding);
                        DustGUI.Header("Name", 36);

                        DustGUI.SpaceExpand();

                        DustGUI.Header("Intensity", CELL_WIDTH_INTENSITY);

                        if (calculatePower)
                            DustGUI.Header("Power", CELL_WIDTH_BLENDING - padding);

                        if (calculateColor)
                            DustGUI.Header("Color", CELL_WIDTH_BLENDING - padding);

                        DustGUI.Header("", CELL_WIDTH_CONTROL);
                    }
                    DustGUI.EndHorizontal();

                    for (int i = 0; i < m_Fields.property.arraySize; i++)
                    {
                        SerializedProperty item = m_Fields.property.GetArrayElementAtIndex(i);

                        if (DrawFieldItem(item, i, m_Fields.property.arraySize))
                        {
                            m_Fields.isChanged = true;
                            EditorUtility.SetDirty(m_Editor);
                            break; // stop update anything... in next update it will redraw real state
                        }
                    }

                    DrawAddFieldButton();
                }
                DustGUI.EndScrollView();
                DustGUI.EndVertical();
                DustGUI.IndentLevelReset(indentLevel);

                DuEditor.Space();

                // DuEditor.PropertyField(m_Fields);
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
                        AddFieldsFromObjectsList(DragAndDrop.objectReferences);
                        Event.current.Use();
                    }
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DuSessionState.SetVector3("DuFieldsMap.Fields.ScrollPosition", m_Editor.target, scrollPosition);
            }
            DustGUI.FoldoutEnd();
        }

        private DuFieldsMap.FieldRecord UnpackFieldRecord(SerializedProperty item)
        {
            var record = new DuFieldsMap.FieldRecord();
            record.enabled = item.FindPropertyRelative("m_Enabled").boolValue;
            record.field = item.FindPropertyRelative("m_Field").objectReferenceValue as DuField;
            record.blendPowerMode = (DuFieldsMap.FieldRecord.BlendPowerMode) item.FindPropertyRelative("m_BlendPowerMode").enumValueIndex;
            record.blendColorMode = (DuFieldsMap.FieldRecord.BlendColorMode) item.FindPropertyRelative("m_BlendColorMode").enumValueIndex;
            record.intensity = item.FindPropertyRelative("m_Intensity").floatValue;
            return record;
        }

        private bool DrawFieldItem(SerializedProperty item, int itemIndex, int itemsCount)
        {
            var curRecord = UnpackFieldRecord(item); // just to save previous state
            var newRecord = UnpackFieldRecord(item); // this record will fix changes

            if (Dust.IsNull(newRecord.field))
                return false; // Notice: but it should never be this way!

            bool clickOnDelete;
            bool clickOnMoveUp;
            bool clickOnMoveDw;

            DustGUI.BeginHorizontal();
            {
                var fieldEnabledInScene = newRecord.field.enabled &&
                                          newRecord.field.gameObject.activeInHierarchy;

                var fieldIcon = UI.Icons.GetTextureByComponent(newRecord.field, !fieldEnabledInScene ? "Disabled" : "");

                if (DustGUI.IconButton(fieldIcon, CELL_WIDTH_ICON, CELL_WIDTH_ICON, UI.ExtraList.styleMiniButton))
                    Selection.activeGameObject = newRecord.field.gameObject;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var btnStateIcon = newRecord.enabled ? UI.Icons.STATE_ENABLED : UI.Icons.STATE_DISABLED;

                if (DustGUI.IconButton(btnStateIcon, CELL_WIDTH_STATE, 32, UI.ExtraList.styleMiniButton))
                    newRecord.enabled = !newRecord.enabled;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Lock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                var fieldName = new GUIContent(newRecord.field.FieldName(), newRecord.field.gameObject.name);
                var fieldHint = fieldEnabledInScene ? newRecord.field.FieldDynamicHint() : "Field not active";

                if (fieldHint != "")
                {
                    DustGUI.BeginVertical();
                    {
                        DustGUI.SimpleLabel(fieldName, 0, 14);
                        DustGUI.SimpleLabel(fieldHint, 0, 10, UI.ExtraList.styleHintLabel);
                    }
                    DustGUI.EndVertical();
                }
                else
                {
                    DustGUI.SimpleLabel(fieldName, 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
                }

                DustGUI.SpaceExpand();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                string intensityValue = newRecord.intensity.ToString("F2");

                if (DustGUI.Button(intensityValue, CELL_WIDTH_INTENSITY, 20f, UI.ExtraList.styleIntensityButton, DustGUI.ButtonState.Pressed))
                {
                    Rect buttonRect = m_RectsUI["item" + itemIndex.ToString()];
                    buttonRect.y += 5f;

                    PopupWindow.Show(buttonRect, DuPopupExtraSlider.Create(m_Editor.serializedObject, "Intensity", item.FindPropertyRelative("m_Intensity")));
                }

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["item" + itemIndex.ToString()] = GUILayoutUtility.GetLastRect();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (calculatePower)
                {
                    var enumValue = DustGUI.DropDownList(newRecord.blendPowerMode, CELL_WIDTH_BLENDING, 0, UI.ExtraList.styleDropDownList);
                    newRecord.blendPowerMode = (DuFieldsMap.FieldRecord.BlendPowerMode) enumValue;
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (calculateColor)
                {
                    if (newRecord.field.IsAllowCalculateFieldColor())
                    {
                        var enumValue = DustGUI.DropDownList(newRecord.blendColorMode, CELL_WIDTH_BLENDING, 0, UI.ExtraList.styleDropDownList);
                        newRecord.blendColorMode = (DuFieldsMap.FieldRecord.BlendColorMode) enumValue;

                        if (newRecord.field.IsHasFieldColorPreview())
                        {
                            Gradient previewGradient = newRecord.field.GetFieldColorPreview();

                            if (Dust.IsNotNull(previewGradient))
                            {
                                Rect lastElement = GUILayoutUtility.GetLastRect();
                                lastElement.x += 2f;
                                lastElement.width -= 4f;
                                lastElement.y += lastElement.height + 1f;
                                lastElement.height = 4f;

                                if (lastElement.width > 0 && lastElement.height > 0)
                                {
                                    var opacity = newRecord.blendColorMode != DuFieldsMap.FieldRecord.BlendColorMode.Ignore ? 1f : 0.25f;
                                    DustGUI.Gradient(lastElement, previewGradient, opacity);
                                }
                            }
                        }
                    }
                    else
                    {
                        DustGUI.Lock();
                        DustGUI.DropDownList(DuFieldsMap.FieldRecord.BlendColorMode.Ignore, CELL_WIDTH_BLENDING, 0, UI.ExtraList.styleDropDownList);
                        DustGUI.Unlock();
                    }
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    DustGUI.Unlock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                clickOnDelete = DustGUI.IconButton(UI.Icons.ACTION_DELETE, 20, 32, UI.ExtraList.styleMiniButton);

                DustGUI.BeginVertical(20);
                {
                    DustGUI.ButtonState stateUp = itemIndex > 0 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;
                    DustGUI.ButtonState stateDw = itemIndex < itemsCount - 1 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;

                    clickOnMoveUp = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_UP, 20, 16, UI.ExtraList.styleMiniButton, stateUp);
                    clickOnMoveDw = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_DOWN, 20, 16, UI.ExtraList.styleMiniButton, stateDw);
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

            if (curRecord.blendPowerMode != newRecord.blendPowerMode) {
                item.FindPropertyRelative("m_BlendPowerMode").enumValueIndex = (int) newRecord.blendPowerMode;
                return true;
            }

            if (curRecord.blendColorMode != newRecord.blendColorMode) {
                item.FindPropertyRelative("m_BlendColorMode").enumValueIndex = (int) newRecord.blendColorMode;
                return true;
            }

            if (!curRecord.intensity.Equals(newRecord.intensity)) {
                item.FindPropertyRelative("m_Intensity").floatValue = newRecord.intensity;
                return true;
            }

            if (clickOnDelete) {
                m_Fields.property.DeleteArrayElementAtIndex(itemIndex);
                return true;
            }

            if (clickOnMoveUp) {
                m_Fields.property.MoveArrayElement(itemIndex, itemIndex - 1);
                return true;
            }

            if (clickOnMoveDw) {
                m_Fields.property.MoveArrayElement(itemIndex, itemIndex + 1);
                return true;
            }

            return false;
        }

        private void DrawAddFieldButton()
        {
            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(UI.Icons.ACTION_ADD_FIELD, CELL_WIDTH_ICON, CELL_WIDTH_ICON, UI.ExtraList.styleMiniButton))
                    PopupWindow.Show(m_RectsUI["Add"], DuPopupButtons.Fields(this));

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["Add"] = GUILayoutUtility.GetLastRect();

                DustGUI.Label("Add field", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
            }
            DustGUI.EndHorizontal();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void AddFieldsFromObjectsList(Object[] objectReferences)
        {
            if (Dust.IsNull(objectReferences) || objectReferences.Length == 0)
                return;

            foreach (Object obj in objectReferences)
            {
                if (obj is GameObject)
                {
                    DuField[] fields = (obj as GameObject).GetComponents<DuField>();

                    foreach (var field in fields)
                    {
                        AddField(field);
                    }
                }
                else if (obj is DuField)
                {
                    AddField(obj as DuField);
                }
            }
        }

        private void AddField(DuField field)
        {
            int count = m_Fields.property.arraySize;

            for (int i = 0; i < count; i++)
            {
                SerializedProperty record = m_Fields.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(record))
                    continue;

                SerializedProperty refObject = record.FindPropertyRelative("m_Field");

                if (Dust.IsNull(refObject) || !refObject.objectReferenceValue.Equals(field))
                    continue;

                return; // No need to insert 2nd time
            }

            m_Fields.property.InsertArrayElementAtIndex(count);

            var defaultRec = new DuFieldsMap.FieldRecord();

            SerializedProperty newRecord = m_Fields.property.GetArrayElementAtIndex(count);

            newRecord.FindPropertyRelative("m_Enabled").boolValue = defaultRec.enabled;
            newRecord.FindPropertyRelative("m_Field").objectReferenceValue = field;
            newRecord.FindPropertyRelative("m_BlendPowerMode").enumValueIndex = (int) defaultRec.blendPowerMode;
            newRecord.FindPropertyRelative("m_BlendColorMode").enumValueIndex = (int) defaultRec.blendColorMode;
            newRecord.FindPropertyRelative("m_Intensity").floatValue = defaultRec.intensity;

            m_Editor.serializedObject.ApplyModifiedProperties();
        }

        private void OptimizeFieldsArray()
        {
            DuEditorHelper.OptimizeObjectReferencesArray(ref m_Fields, "m_Field");
        }
    }
}
