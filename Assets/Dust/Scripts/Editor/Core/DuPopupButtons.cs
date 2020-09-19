using System.Collections.Generic;
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
            Fields = 0,
            Deformers = 1,
        }

        private class ColumnRecord
        {
            public string title;
            public List<CellRecord> cells = new List<CellRecord>();
        }

        private struct CellRecord
        {
            public string title;
            public System.Type type;
        }

        //--------------------------------------------------------------------------------------------------------------

        private PopupMode m_PopupMode;

        private DuFieldsMapEditor m_FieldsMap;
        private DuDeformMeshEditor m_DeformMesh;

        private List<ColumnRecord> m_ColumnRecords = new List<ColumnRecord>();
        private int m_ColsCount;
        private int m_RowsCount;

        //--------------------------------------------------------------------------------------------------------------

        public static DuPopupButtons Fields(DuFieldsMapEditor fieldsMap)
        {
            var popup = new DuPopupButtons();
            popup.m_PopupMode = PopupMode.Deformers;
            popup.m_FieldsMap = fieldsMap;

            ColumnRecord column;

            column = new ColumnRecord() {title = "Basic Fields"};
            {
                popup.AddItem(column, typeof(DuConstantField), "Constant");
                popup.AddItem(column, typeof(DuStepObjectsField), "Step Objects");
                popup.AddItem(column, typeof(DuTimeField), "Time");

                popup.m_ColumnRecords.Add(column);
            }

            column = new ColumnRecord() {title = "Object Fields"};
            {
                popup.AddItem(column, typeof(DuConeField), "Cone");
                popup.AddItem(column, typeof(DuCubeField), "Cube");
                popup.AddItem(column, typeof(DuCylinderField), "Cylinder");
                popup.AddItem(column, typeof(DuDirectionalField), "Directional");
                popup.AddItem(column, typeof(DuRadialField), "Radial");
                popup.AddItem(column, typeof(DuSphereField), "Sphere");
                popup.AddItem(column, typeof(DuTorusField), "Torus");

                popup.m_ColumnRecords.Add(column);
            }

            column = new ColumnRecord() {title = "Math Fields"};
            {
                popup.AddItem(column, typeof(DuClampField), "Clamp");
                popup.AddItem(column, typeof(DuCurveField), "Curve");
                popup.AddItem(column, typeof(DuFitField), "Fit");
                popup.AddItem(column, typeof(DuInvertField), "Invert");
                popup.AddItem(column, typeof(DuRemapField), "Remap");
                popup.AddItem(column, typeof(DuRoundField), "Round");

                popup.m_ColumnRecords.Add(column);
            }

            popup.m_ColsCount = popup.m_ColumnRecords.Count;
            return popup;
        }

        public static DuPopupButtons Deformers(DuDeformMeshEditor deformMesh)
        {
            var popup = new DuPopupButtons();
            popup.m_PopupMode = PopupMode.Deformers;
            popup.m_DeformMesh = deformMesh;

            ColumnRecord column;

            column = new ColumnRecord() {title = "Deformers"};
            {
                popup.AddItem(column, typeof(DuTwistDeformer), "Twist");
                popup.AddItem(column, typeof(DuWaveDeformer), "Wave");

                popup.m_ColumnRecords.Add(column);
            }

            popup.m_ColsCount = popup.m_ColumnRecords.Count;
            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void AddItem(ColumnRecord columnRecord, System.Type type, string title)
        {
            CellRecord button;
            button.title = title;
            button.type = type;
            columnRecord.cells.Add(button);

            m_RowsCount = Mathf.Max(m_RowsCount, columnRecord.cells.Count);
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
            GUIStyle btnStyle = DustGUI.GetIconButtonStyle();
            btnStyle.alignment = TextAnchor.MiddleLeft;

            DustGUI.BeginHorizontal();

            foreach (var columnRecord in m_ColumnRecords)
            {
                DustGUI.BeginVertical();

                DustGUI.Header(columnRecord.title, BUTTON_WIDTH);

                foreach (var cellRecord in columnRecord.cells)
                {
                    GUIContent btnContent = new GUIContent();
                    btnContent.image = Icons.GetTextureByClassName(cellRecord.type.ToString());
                    btnContent.text = cellRecord.title;

                    if (DustGUI.IconButton(btnContent, BUTTON_WIDTH, BUTTON_HEIGHT, btnStyle))
                    {
                        if (m_PopupMode == PopupMode.Fields)
                        {
                            DuField.AddFieldComponentByType(m_FieldsMap.GetParentGameObject(), cellRecord.type);
                        }
                        else if (m_PopupMode == PopupMode.Deformers)
                        {
                            DuDeformer.AddDeformerComponentByType((m_DeformMesh.target as DuMonoBehaviour).gameObject, cellRecord.type);
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
