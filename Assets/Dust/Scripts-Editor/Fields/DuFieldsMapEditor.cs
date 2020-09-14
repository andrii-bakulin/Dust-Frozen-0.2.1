using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public class DuFieldsMapEditor
    {
        private DuFieldsMap m_FieldsMapInstance;

        private DuEditor m_Editor;
        private DuEditor.DuProperty m_Fields;

        private Rect m_RectButtonFieldsPopup;

        public DuFieldsMapEditor(DuEditor parentEditor, SerializedProperty fieldsMapProperty, DuFieldsMap fieldsMapInstance)
        {
            m_FieldsMapInstance = fieldsMapInstance;

            m_Editor = parentEditor;
            m_Fields = DuEditor.FindProperty(fieldsMapProperty, "m_Fields", "Fields");
        }

        //--------------------------------------------------------------------------------------------------------------

        public GameObject GetParentGameObject()
        {
            return (m_Editor.target as DuMonoBehaviour).gameObject;
        }

        public bool calculateValues => m_FieldsMapInstance.calculateValues;

        public bool calculateColors => m_FieldsMapInstance.calculateColors;

        //--------------------------------------------------------------------------------------------------------------

        public void OnInspectorGUI()
        {
            if (DustGUI.FoldoutBegin("Fields Map", "DuFieldsMap.Main"))
            {
                OptimizeFieldsArray();

                Vector2 scrollPosition = DuSessionState.GetVector3("DuFieldsMapEditor.Fields.ScrollPosition", m_Editor.target, Vector2.zero);
                float totalHeight = 24 + 36 * Mathf.Clamp(m_Fields.property.arraySize + 1, 4, 8) + 16;

                int indentLevel = DustGUI.IndentLevelReset(); // Because it'll try to add left-spacing when draw fields
                Rect rect = DustGUI.BeginVerticalBox();
                DustGUI.BeginScrollView(ref scrollPosition, 0, totalHeight);
                {
                    DustGUI.BeginHorizontal();
                    {
                        float padding = 2;
                        DustGUI.Header("", 52 - padding * 2); // First 2 buttons
                        DustGUI.Header("Name", 70 - padding);
                        DustGUI.SpaceExpand();

                        if (calculateValues && calculateColors)
                        {
                            DustGUI.Header("", 32 - padding); // Calculation on/off button for weight
                            DustGUI.Header("", 32 - padding); // Calculation on/off button for color
                        }

                        DustGUI.Header("Blending", 70 - padding);
                        DustGUI.Header("Intensity", 80 - padding);
                        DustGUI.Header("", 40 - padding * 2); // Control buttons
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

                DuSessionState.SetVector3("DuFieldsMapEditor.Fields.ScrollPosition", m_Editor.target, scrollPosition);
            }
            DustGUI.FoldoutEnd();
        }

        private DuFieldsMap.FieldRecord UnpackFieldRecord(SerializedProperty item)
        {
            var record = new DuFieldsMap.FieldRecord();
            record.enabled = item.FindPropertyRelative("m_Enabled").boolValue;
            record.field = item.FindPropertyRelative("m_Field").objectReferenceValue as DuField;
            record.blend = (DuFieldsMap.FieldRecord.BlendMode) item.FindPropertyRelative("m_BlendMode").enumValueIndex;
            record.calculateValue = item.FindPropertyRelative("m_CalculateValue").boolValue;
            record.calculateColor = item.FindPropertyRelative("m_CalculateColor").boolValue;
            record.intensity = item.FindPropertyRelative("m_Intensity").floatValue;
            return record;
        }

        private bool DrawFieldItem(SerializedProperty item, int itemIndex, int itemsCount)
        {
            var curRecord = UnpackFieldRecord(item); // just to save previos state
            var newRecord = UnpackFieldRecord(item); // this record will fix changes

            if (Dust.IsNull(newRecord.field))
                return false; // Notice: but it should never be this way!

            bool clickOnDelete;
            bool clickOnMoveUp;
            bool clickOnMoveDw;

            var miniButtonStyle = new GUIStyle(GUI.skin.button);
            miniButtonStyle.padding = new RectOffset(2, 2, 0, 0);
            miniButtonStyle.margin = new RectOffset(0, 0, 0, 0);

            var dropDownListStyle = new GUIStyle(EditorStyles.popup);
            dropDownListStyle.margin.top = 6;

            var titleStyle = new GUIStyle(GUI.skin.label);

            var intensityLabelStyle = new GUIStyle(GUI.skin.textField);
            intensityLabelStyle.margin = new RectOffset(35,0,5,0);

            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(Icons.GetTextureByComponent(newRecord.field), newRecord.enabled ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Pressed))
                    Selection.activeGameObject = newRecord.field.gameObject;

                // @todo!
                //if (DustGUI.IconButton(newRecord.enabled ? Icons.STATE_ENABLED : Icons.STATE_DISABLED, 20, 32, miniButtonStyle))
                //    newRecord.enabled = !newRecord.enabled;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (!newRecord.enabled)
                    titleStyle.normal.textColor = Color.gray;

                DustGUI.SimpleLabel(newRecord.field.FieldName(), 0, DustGUI.Config.ICON_BUTTON_HEIGHT, titleStyle);

                DustGUI.SpaceExpand();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                /* @todo!
                if (calculateValues && calculateColors)
                {
                    if (DustGUI.IconButton(newRecord.calculateValue ? Icons.FIELDS_MAP_WEIGHT_ENABLED : Icons.FIELDS_MAP_WEIGHT_DISABLED))
                        newRecord.calculateValue = !newRecord.calculateValue;

                    if (DustGUI.IconButton(newRecord.calculateColor ? Icons.FIELDS_MAP_COLOR_ENABLED : Icons.FIELDS_MAP_COLOR_DISABLED))
                        newRecord.calculateColor = !newRecord.calculateColor;
                }
                */

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                newRecord.blend = (DuFieldsMap.FieldRecord.BlendMode) DustGUI.DropDownList(newRecord.blend, 70, 0, dropDownListStyle);

                DustGUI.BeginVertical(80);
                {
                    float newIntensity = DustGUI.SliderOnly01(newRecord.intensity);

                    if (!newIntensity.Equals(newRecord.intensity))
                        newRecord.intensity = DuMath.Round(newIntensity, 2);

                    DustGUI.Space(9f);

                    newRecord.intensity = DustGUI.Field("", newRecord.intensity, 40, 0, intensityLabelStyle);
                    newRecord.intensity = Mathf.Clamp01(newRecord.intensity);
                }
                DustGUI.EndVertical();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                // @todo!
                //clickOnDelete = DustGUI.IconButton(Icons.ACTION_DELETE, 20, 32, miniButtonStyle);
                clickOnDelete = false;

                DustGUI.BeginVertical(20);
                {
                    DustGUI.ButtonState stateUp = itemIndex > 0 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;
                    DustGUI.ButtonState stateDw = itemIndex < itemsCount - 1 ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Locked;

                    clickOnMoveUp = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_UP, 20, 16, miniButtonStyle, stateUp);
                    clickOnMoveDw = DustGUI.IconButton(DustGUI.Config.RESOURCE_ICON_ARROW_DOWN, 20, 16, miniButtonStyle, stateDw);
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

            if (curRecord.blend != newRecord.blend) {
                item.FindPropertyRelative("m_BlendMode").enumValueIndex = (int) newRecord.blend;
                return true;
            }

            if (curRecord.calculateValue != newRecord.calculateValue) {
                item.FindPropertyRelative("m_CalculateValue").boolValue = newRecord.calculateValue;
                return true;
            }

            if (curRecord.calculateColor != newRecord.calculateColor) {
                item.FindPropertyRelative("m_CalculateColor").boolValue = newRecord.calculateColor;
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
            var miniButtonStyle = new GUIStyle(GUI.skin.button);
            miniButtonStyle.padding = new RectOffset(2, 2, 0, 0);
            miniButtonStyle.margin = new RectOffset(0, 0, 0, 0);

            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(Icons.ADD_FIELD))
                    PopupWindow.Show(m_RectButtonFieldsPopup, DuFieldsMapPopup.AllFields(this));

                if (Event.current.type == EventType.Repaint)
                    m_RectButtonFieldsPopup = GUILayoutUtility.GetLastRect();

                DustGUI.Label("To add field click on [+] or drag-and-drop it here", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
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
                SerializedProperty fldRecord = m_Fields.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(fldRecord))
                    continue;

                SerializedProperty fieldObject = fldRecord.FindPropertyRelative("m_Field");

                if (Dust.IsNull(fieldObject) || !fieldObject.objectReferenceValue.Equals(field))
                    continue;

                return; // No need to insert 2nd time
            }

            m_Fields.property.InsertArrayElementAtIndex(count);

            var defaultRec = new DuFieldsMap.FieldRecord();

            SerializedProperty newRecord = m_Fields.property.GetArrayElementAtIndex(count);

            newRecord.FindPropertyRelative("m_Enabled").boolValue = defaultRec.enabled;
            newRecord.FindPropertyRelative("m_Field").objectReferenceValue = field;
            newRecord.FindPropertyRelative("m_BlendMode").enumValueIndex = (int) defaultRec.blend;
            newRecord.FindPropertyRelative("m_CalculateValue").boolValue = defaultRec.calculateValue;
            newRecord.FindPropertyRelative("m_CalculateColor").boolValue = defaultRec.calculateColor;
            newRecord.FindPropertyRelative("m_Intensity").floatValue = defaultRec.intensity;

            m_Editor.serializedObject.ApplyModifiedProperties();
        }

        private void OptimizeFieldsArray()
        {
            bool changed = false;

            for (int i = m_Fields.property.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty fldRecord = m_Fields.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(fldRecord))
                    continue;

                SerializedProperty fieldObject = fldRecord.FindPropertyRelative("m_Field");

                if (Dust.IsNotNull(fieldObject) && Dust.IsNull(fieldObject.objectReferenceValue))
                {
                    m_Fields.property.DeleteArrayElementAtIndex(i);
                    changed = true;
                }
            }

            if (!changed)
                return;

            m_Editor.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
