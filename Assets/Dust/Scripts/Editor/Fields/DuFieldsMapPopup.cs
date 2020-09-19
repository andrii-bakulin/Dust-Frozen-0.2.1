using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public class DuFieldsMapPopup : PopupWindowContent
    {
        internal const float BUTTON_WIDTH = 128f;
        internal const float BUTTON_HEIGHT = 28f;

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

        private int m_ColsCount;
        private int m_RowsCount;

        //--------------------------------------------------------------------------------------------------------------

        private DuFieldsMapEditor m_FieldsMap;

        private List<ColumnRecord> m_ColumnRecords = new List<ColumnRecord>();

        //--------------------------------------------------------------------------------------------------------------

        public static DuFieldsMapPopup AllFields(DuFieldsMapEditor fieldsMap)
        {
            return new DuFieldsMapPopup(fieldsMap);
        }

        //--------------------------------------------------------------------------------------------------------------

        private DuFieldsMapPopup(DuFieldsMapEditor fieldsMap)
        {
            m_FieldsMap = fieldsMap;

            var colBasicFields = new ColumnRecord() {title = "Basic Fields"};
            {
                AddItem(colBasicFields, typeof(DuConstantField), "Constant");
                AddItem(colBasicFields, typeof(DuStepObjectsField), "Step Objects");
                AddItem(colBasicFields, typeof(DuTimeField), "Time");
            }
            m_ColumnRecords.Add(colBasicFields);
            m_ColsCount++;

            var colObjectFields = new ColumnRecord() {title = "Object Fields"};
            {
                AddItem(colObjectFields, typeof(DuConeField), "Cone");
                AddItem(colObjectFields, typeof(DuCubeField), "Cube");
                AddItem(colObjectFields, typeof(DuCylinderField), "Cylinder");
                AddItem(colObjectFields, typeof(DuDirectionalField), "Directional");
                AddItem(colObjectFields, typeof(DuRadialField), "Radial");
                AddItem(colObjectFields, typeof(DuSphereField), "Sphere");
                AddItem(colObjectFields, typeof(DuTorusField), "Torus");
            }
            m_ColumnRecords.Add(colObjectFields);
            m_ColsCount++;

            var colMathFields = new ColumnRecord() {title = "Math Fields"};
            {
                AddItem(colMathFields, typeof(DuClampField), "Clamp");
                AddItem(colMathFields, typeof(DuCurveField), "Curve");
                AddItem(colMathFields, typeof(DuFitField), "Fit");
                AddItem(colMathFields, typeof(DuInvertField), "Invert");
                AddItem(colMathFields, typeof(DuRemapField), "Remap");
                AddItem(colMathFields, typeof(DuRoundField), "Round");
            }
            m_ColumnRecords.Add(colMathFields);
            m_ColsCount++;
        }

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
            float padding = 4f;
            float titleHeight = 16f;
            float buttonPadding = 1f;

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
                    GUIContent cntFields = new GUIContent();
                    cntFields.image = Icons.GetTextureByClassName(cellRecord.type.ToString());
                    cntFields.text = cellRecord.title;

                    if (DustGUI.IconButton(cntFields, BUTTON_WIDTH, BUTTON_HEIGHT, btnStyle))
                    {
                        DuField.AddFieldComponentByType(m_FieldsMap.GetParentGameObject(), cellRecord.type);
                        editorWindow.Close();
                    }
                }

                DustGUI.EndVertical();
            }

            DustGUI.EndHorizontal();
        }
    }
}
#endif
