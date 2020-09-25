using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryEditor : DuEditor
    {
        private const float CELL_WIDTH_ICON = 32f;

        //--------------------------------------------------------------------------------------------------------------

        private DuProperty m_Objects;
        private DuProperty m_IterateMode;
        private DuProperty m_InstanceMode;
        private DuProperty m_ForcedSetActive;
        private DuProperty m_Seed;

        private DuProperty m_Power;
        private DuProperty m_Color;
        private DuProperty m_FactoryMachines;

        private DuProperty m_TransformSpace;
        private DuProperty m_TransformSequence;
        private DuProperty m_TransformPosition;
        private DuProperty m_TransformRotation;
        private DuProperty m_TransformScale;

        private DuProperty m_InspectorDisplay;
        private DuProperty m_InspectorScale;

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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly Dictionary<string, Rect> m_RectsUI = new Dictionary<string, Rect>();

        protected bool m_IsRequireRebuildClones;
        protected bool m_IsRequireResetupClones;

        //--------------------------------------------------------------------------------------------------------------

        protected void OnEnableFactory()
        {
            m_Objects = FindProperty("m_Objects", "Objects");
            m_IterateMode = FindProperty("m_IterateMode", "Iterate");
            m_InstanceMode = FindProperty("m_InstanceMode", "Instance Mode");
            m_ForcedSetActive = FindProperty("m_ForcedSetActive", "Forced Set Active");
            m_Seed = FindProperty("m_Seed", "Forced Set Active");

            m_Power = FindProperty("m_Power", "Init Power");
            m_Color = FindProperty("m_Color", "Init Color");
            m_FactoryMachines = FindProperty("m_FactoryMachines", "Factory Machines");

            m_TransformSpace = FindProperty("m_TransformSpace", "Transform Space");
            m_TransformSequence = FindProperty("m_TransformSequence", "Transform Sequence");
            m_TransformPosition = FindProperty("m_TransformPosition", "Position");
            m_TransformRotation = FindProperty("m_TransformRotation", "Rotation");
            m_TransformScale = FindProperty("m_TransformScale", "Scale");

            m_InspectorDisplay = FindProperty("m_InspectorDisplay", "Display");
            m_InspectorScale = FindProperty("m_InspectorScale", "Scale");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void BeginData()
        {
            m_IsRequireRebuildClones = m_IsRequireResetupClones = DustGUI.IsUndoRedoPerformed();

            serializedObject.Update();
        }

        protected void CommitDataAndUpdateStates()
        {
            serializedObject.ApplyModifiedProperties();

            foreach (var subTarget in targets)
            {
                var origin = subTarget as DuFactory;

                if (m_IsRequireRebuildClones)
                    origin.RebuildInstances();

                if (m_IsRequireResetupClones)
                    origin.UpdateInstancesZeroStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnInspectorGUI_Objects()
        {
            if (DustGUI.FoldoutBegin("Source Objects", "DuFactory.Objects"))
            {
                PropertyField(m_Objects);
                Space();
                PropertyField(m_IterateMode);

                if ((DuFactory.IterateMode) m_IterateMode.enumValueIndex == DuFactory.IterateMode.Random)
                    PropertySeedFixed(m_Seed);

                PropertyField(m_InstanceMode);
                PropertyField(m_ForcedSetActive);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireRebuildClones |= m_Objects.isChanged;
            m_IsRequireRebuildClones |= m_IterateMode.isChanged;
            m_IsRequireRebuildClones |= m_InstanceMode.isChanged;
            m_IsRequireRebuildClones |= m_Seed.isChanged;
            m_IsRequireRebuildClones |= m_ForcedSetActive.isChanged;
        }

        protected void OnInspectorGUI_FactoryMachines()
        {
            if (DustGUI.FoldoutBegin("Factory Machines", "DuFactory.FactoryMachines"))
            {
                PropertyExtendedSlider(m_Power, 0f, 1f, 0.01f);
                PropertyField(m_Color);
                Space();

                DrawFactoryMachinesBlock();
                PropertyField(m_FactoryMachines); // @todo! hide
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireResetupClones |= m_Power.isChanged;
            m_IsRequireResetupClones |= m_Color.isChanged;
            m_IsRequireResetupClones |= m_FactoryMachines.isChanged;
        }

        protected void OnInspectorGUI_Transform()
        {
            if (DustGUI.FoldoutBegin("Transform", "DuFactory.Transform"))
            {
                PropertyField(m_TransformPosition);
                PropertyField(m_TransformRotation);
                PropertyField(m_TransformScale);
                PropertyField(m_TransformSpace);

                if ((DuFactory.TransformSpace) m_TransformSpace.enumValueIndex == DuFactory.TransformSpace.Instance)
                    PropertyField(m_TransformSequence);

                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireResetupClones |= m_TransformPosition.isChanged;
            m_IsRequireResetupClones |= m_TransformRotation.isChanged;
            m_IsRequireResetupClones |= m_TransformScale.isChanged;
            m_IsRequireResetupClones |= m_TransformSpace.isChanged;
            m_IsRequireResetupClones |= m_TransformSequence.isChanged;
        }

        protected void OnInspectorGUI_Display()
        {
            if (DustGUI.FoldoutBegin("Display", "DuFactory.Display"))
            {
                PropertyField(m_InspectorDisplay);
                PropertyExtendedSlider(m_InspectorScale, 0.5f, 3f, 0.01f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_Tools()
        {
            if (DustGUI.FoldoutBegin("Tools", "DuFactory.Tools", false))
            {
                if (DustGUI.Button("Forced Rebuild Clones"))
                    m_IsRequireRebuildClones |= true;
            }
            DustGUI.FoldoutEnd();
        }

        //--------------------------------------------------------------------------------------------------------------

        void DrawFactoryMachinesBlock()
        {
            if (targets.Length > 1)
            {
                DustGUI.BeginVerticalBox(0, 32 * 3f);
                DustGUI.Label("Cannot edit machines for multiple factories", 0, DustGUI.Config.ICON_BUTTON_HEIGHT, Color.gray);
                DustGUI.EndVertical();
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OptimizeFactoryMachinesArray();

            Vector2 scrollPosition = DuSessionState.GetVector3("DuFactory.FactoryMachine.ScrollPosition", target, Vector2.zero);
            float totalHeight = 32 * Mathf.Clamp(m_FactoryMachines.property.arraySize + 1, 3, 8);

            int indentLevel = DustGUI.IndentLevelReset(); // Because it'll try to add left-spacing when draw fields
            Rect rect = DustGUI.BeginVerticalBox();
            DustGUI.BeginScrollView(ref scrollPosition, 0, totalHeight);
            {
                for (int i = 0; i < m_FactoryMachines.property.arraySize; i++)
                {
                    SerializedProperty item = m_FactoryMachines.property.GetArrayElementAtIndex(i);

                    if (DrawFactoryMachineItem(item, i, m_FactoryMachines.property.arraySize))
                    {
                        m_FactoryMachines.isChanged = true;
                        EditorUtility.SetDirty(this);
                        break; // stop update anything... in next update it will redraw real state
                    }
                }

                DrawAddFactoryMachineButton();
            }
            DustGUI.EndScrollView();
            DustGUI.EndVertical();
            DustGUI.IndentLevelReset(indentLevel);

            for (int i = 0; i < m_FactoryMachines.property.arraySize; i++)
            {
                SerializedProperty item = m_FactoryMachines.property.GetArrayElementAtIndex(i);
                var record = UnpackFactoryMachineRecord(item);

                if (Dust.IsNull(record.factoryMachine))
                    continue; // Notice: but it should never be this way!

                var property = FindProperty(this, item, "m_Intensity", record.factoryMachine.FactoryMachineName());

                PropertyExtendedSlider(property, 0f, 1f, 0.01f);
            }

            Space();

            DuSessionState.SetVector3("DuFactory.FactoryMachine.ScrollPosition", target, scrollPosition);

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
                    AddFactoryMachineFromObjectsList(DragAndDrop.objectReferences);
                    Event.current.Use();
                }
            }
        }

        private DuFactoryMachine.Record UnpackFactoryMachineRecord(SerializedProperty item)
        {
            var record = new DuFactoryMachine.Record();
            record.factoryMachine = item.FindPropertyRelative("m_FactoryMachine").objectReferenceValue as DuFactoryMachine;
            record.intensity = item.FindPropertyRelative("m_Intensity").floatValue;
            record.enabled = item.FindPropertyRelative("m_Enabled").boolValue;
            return record;
        }

        private bool DrawFactoryMachineItem(SerializedProperty item, int itemIndex, int itemsCount)
        {
            var record = UnpackFactoryMachineRecord(item);

            if (Dust.IsNull(record.factoryMachine))
                return false; // Notice: but it should never be this way!

            bool clickOnIcon;
            bool clickOnEnable;
            bool clickOnDelete;
            bool clickOnMoveUp;
            bool clickOnMoveDw;

            var miniButtonStyle = new GUIStyle(GUI.skin.button);
            miniButtonStyle.padding = new RectOffset(2, 2, 0, 0);
            miniButtonStyle.margin = new RectOffset(0, 0, 0, 0);

            DustGUI.BeginHorizontal();
            {
                clickOnIcon = DustGUI.IconButton(Icons.GetTextureByComponent(record.factoryMachine),
                    record.enabled ? DustGUI.ButtonState.Normal : DustGUI.ButtonState.Pressed);

                DustGUI.Label(record.factoryMachine.FactoryMachineName(), 0, DustGUI.Config.ICON_BUTTON_HEIGHT,
                    record.enabled ? DustGUI.labelNormalColor : Color.gray);

                clickOnEnable = DustGUI.IconButton(record.enabled ? Icons.STATE_ENABLED : Icons.STATE_DISABLED);
                clickOnDelete = DustGUI.IconButton(Icons.ACTION_DELETE, 20, 32, miniButtonStyle);

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

            if (clickOnIcon)
            {
                Selection.activeGameObject = record.factoryMachine.gameObject;
                return false;
            }

            if (clickOnEnable)
            {
                item.FindPropertyRelative("m_Enabled").boolValue = !item.FindPropertyRelative("m_Enabled").boolValue;
                return true;
            }

            if (clickOnDelete)
            {
                m_FactoryMachines.property.DeleteArrayElementAtIndex(itemIndex);
                return true;
            }

            if (clickOnMoveUp)
            {
                m_FactoryMachines.property.MoveArrayElement(itemIndex, itemIndex - 1);
                return true;
            }

            if (clickOnMoveDw)
            {
                m_FactoryMachines.property.MoveArrayElement(itemIndex, itemIndex + 1);
                return true;
            }

            return false;
        }

        private void DrawAddFactoryMachineButton()
        {
            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(Icons.ACTION_ADD_FACTORY_MACHINE, CELL_WIDTH_ICON, CELL_WIDTH_ICON, styleMiniButton))
                    PopupWindow.Show(m_RectsUI["Add"], DuPopupButtons.FactoryMachines(this));

                if (Event.current.type == EventType.Repaint)
                    m_RectsUI["Add"] = GUILayoutUtility.GetLastRect();

                DustGUI.Label("Add Factory Machine", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
            }
            DustGUI.EndHorizontal();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private void AddFactoryMachineFromObjectsList(Object[] objectReferences)
        {
            if (Dust.IsNull(objectReferences) || objectReferences.Length == 0)
                return;

            foreach (Object obj in objectReferences)
            {
                if (obj is GameObject)
                {
                    DuFactoryMachine[] factoryMachines = (obj as GameObject).GetComponents<DuFactoryMachine>();

                    foreach (var factoryMachine in factoryMachines)
                    {
                        AddFactoryMachine(factoryMachine);
                    }
                }
                else if (obj is DuFactoryMachine)
                {
                    AddFactoryMachine(obj as DuFactoryMachine);
                }
            }
        }

        private void AddFactoryMachine(DuFactoryMachine factoryMachine)
        {
            int count = m_FactoryMachines.property.arraySize;

            for (int i = 0; i < count; i++)
            {
                SerializedProperty record = m_FactoryMachines.property.GetArrayElementAtIndex(i);

                if (Dust.IsNull(record))
                    continue;

                SerializedProperty refObject = record.FindPropertyRelative("m_FactoryMachine");

                if (Dust.IsNull(refObject) || !refObject.objectReferenceValue.Equals(factoryMachine))
                    continue;

                return; // No need to insert 2nd time
            }

            m_FactoryMachines.property.InsertArrayElementAtIndex(count);

            var defaultRec = new DuFactoryMachine.Record();

            SerializedProperty newRecord = m_FactoryMachines.property.GetArrayElementAtIndex(count);
            newRecord.FindPropertyRelative("m_FactoryMachine").objectReferenceValue = factoryMachine;
            newRecord.FindPropertyRelative("m_Intensity").floatValue = defaultRec.intensity;
            newRecord.FindPropertyRelative("m_Enabled").boolValue = defaultRec.enabled;

            serializedObject.ApplyModifiedProperties();
        }

        private void OptimizeFactoryMachinesArray()
        {
            DuEditorHelper.OptimizeObjectReferencesArray(ref m_FactoryMachines, "m_FactoryMachine");
        }
    }
}
