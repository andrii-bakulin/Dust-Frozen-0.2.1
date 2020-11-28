using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuPopupButtons : PopupWindowContent
    {
        private const float BUTTON_WIDTH = 128f;
        private const float BUTTON_HEIGHT = 28f;

        private enum PopupMode
        {
            Deformers = 1,
            FactoryMachines = 2,
            Fields = 3,
        }

        private enum EntityType
        {
            Deformer = 11,

            FactoryMachine = 21,

            BasicField = 31,
            FactoryField = 32,
            Space2DField = 33,
            Space3DField = 34,
            MathField = 35,
        }

        //--------------------------------------------------------------------------------------------------------------

        private class ColumnRecord
        {
            public string title;
            public List<CellRecord> cells = new List<CellRecord>();
        }

        private struct CellRecord : IEquatable<CellRecord>
        {
            public string title;
            public System.Type type;

            public bool Equals(CellRecord other)
            {
                throw new NotImplementedException();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private static Dictionary<EntityType, Dictionary<System.Type, string>> m_Entities;

        //--------------------------------------------------------------------------------------------------------------

        private PopupMode m_PopupMode;

        private DuDeformMeshEditor m_DeformMesh;
        private DuFactoryEditor m_Factory;
        private DuFieldsMapEditor m_FieldsMap;

        private List<ColumnRecord> m_ColumnRecords = new List<ColumnRecord>();
        private int m_ColsCount;
        private int m_RowsCount;

        //--------------------------------------------------------------------------------------------------------------

        public static DuPopupButtons Deformers(DuDeformMeshEditor deformMesh)
        {
            var popup = new DuPopupButtons();
            popup.m_PopupMode = PopupMode.Deformers;
            popup.m_DeformMesh = deformMesh;

            GenerateColumn(popup, EntityType.Deformer, "Deformers");

            return popup;
        }

        public static DuPopupButtons FactoryMachines(DuFactoryEditor factory)
        {
            var popup = new DuPopupButtons();
            popup.m_PopupMode = PopupMode.FactoryMachines;
            popup.m_Factory = factory;

            GenerateColumn(popup, EntityType.FactoryMachine, "Machines");

            return popup;
        }

        public static DuPopupButtons Fields(DuFieldsMapEditor fieldsMap)
        {
            var popup = new DuPopupButtons();
            popup.m_PopupMode = PopupMode.Fields;
            popup.m_FieldsMap = fieldsMap;

            GenerateColumn(popup, EntityType.BasicField, "Basic Fields");

            if (fieldsMap.fieldsMapInstance.fieldsMapMode == DuFieldsMap.FieldsMapMode.FactoryMachine)
                GenerateColumn(popup, EntityType.FactoryField, "Factory Fields");

            GenerateColumn(popup, EntityType.Space2DField, "2D Fields");
            GenerateColumn(popup, EntityType.Space3DField, "3D Fields");
            GenerateColumn(popup, EntityType.MathField, "Math Fields");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void AddDeformer(System.Type type, string title)
        {
            AddEntity(EntityType.Deformer, type, title);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static void AddFactoryMachine(System.Type type, string title)
        {
            AddEntity(EntityType.FactoryMachine, type, title);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static void AddBasicField(System.Type type, string title)
        {
            AddEntity(EntityType.BasicField, type, title);
        }

        public static void AddFactoryField(System.Type type, string title)
        {
            AddEntity(EntityType.FactoryField, type, title);
        }

        public static void AddSpace2DField(System.Type type, string title)
        {
            AddEntity(EntityType.Space2DField, type, title);
        }

        public static void AddSpace3DField(System.Type type, string title)
        {
            AddEntity(EntityType.Space3DField, type, title);
        }

        public static void AddMathField(System.Type type, string title)
        {
            AddEntity(EntityType.MathField, type, title);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static void AddEntity(EntityType entityType, System.Type type, string title)
        {
            if (Dust.IsNull(m_Entities))
                m_Entities = new Dictionary<EntityType, Dictionary<System.Type, string>>();

            if (!m_Entities.ContainsKey(entityType))
                m_Entities[entityType] = new Dictionary<System.Type, string>();

            m_Entities[entityType][type] = title;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static void GenerateColumn(DuPopupButtons popup, EntityType entityType, string title)
        {
            if (Dust.IsNull(m_Entities))
                return;

            var column = new ColumnRecord
            {
                title = title
            };

            if (m_Entities.ContainsKey(entityType))
            {
                var sortedEntities = from entry in m_Entities[entityType]
                    orderby entry.Value ascending
                    select entry;

                foreach (var pair in sortedEntities)
                {
                    CellRecord button;
                    button.title = pair.Value;
                    button.type = pair.Key;
                    column.cells.Add(button);

                    popup.m_RowsCount = Mathf.Max(popup.m_RowsCount, column.cells.Count);
                }
            }

            popup.m_ColumnRecords.Add(column);
            popup.m_ColsCount = popup.m_ColumnRecords.Count;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override Vector2 GetWindowSize()
        {
            var padding = 4f;
            var titleHeight = 16f;
            var buttonPadding = 1f;

            return new Vector2(m_ColsCount * (BUTTON_WIDTH + 2f * buttonPadding) + 2f * padding,
                titleHeight + m_RowsCount * (BUTTON_HEIGHT + 2f * buttonPadding) + 2f * padding);
        }

        public override void OnGUI(Rect rect)
        {
            GUIStyle btnStyle = DustGUI.iconButtonStyle;
            btnStyle.alignment = TextAnchor.MiddleLeft;

            DustGUI.BeginHorizontal();

            foreach (var columnRecord in m_ColumnRecords)
            {
                DustGUI.BeginVertical();

                DustGUI.Header(columnRecord.title, BUTTON_WIDTH);

                foreach (var cellRecord in columnRecord.cells)
                {
                    GUIContent btnContent = new GUIContent();
                    btnContent.image = UI.Icons.GetTextureByClassName(cellRecord.type.ToString());
                    btnContent.text = cellRecord.title;

                    if (DustGUI.IconButton(btnContent, BUTTON_WIDTH, BUTTON_HEIGHT, btnStyle))
                    {
                        if (m_PopupMode == PopupMode.Deformers)
                        {
                            DuDeformerEditor.AddDeformerComponentByType((m_DeformMesh.target as DuMonoBehaviour).gameObject, cellRecord.type);
                        }
                        else if (m_PopupMode == PopupMode.FactoryMachines)
                        {
                            DuFactoryMachineEditor.AddFactoryMachineComponentByType((m_Factory.target as DuMonoBehaviour).gameObject, cellRecord.type);
                        }
                        else if (m_PopupMode == PopupMode.Fields)
                        {
                            DuFieldEditor.AddFieldComponentByType(m_FieldsMap.GetParentGameObject(), cellRecord.type);
                        }

                        editorWindow.Close();
                    }
                }

                DustGUI.EndVertical();
            }

            DustGUI.EndHorizontal();
        }
    }
}
