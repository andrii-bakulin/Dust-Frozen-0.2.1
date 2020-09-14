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
            public string className;
            public string title;

            public CellRecord(string className, string title)
            {
                this.className = className;
                this.title = title;
            }
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

            var colObjectFields = new ColumnRecord() {title = "Object Fields"};
            {
                AddItem(colObjectFields, "DustEngine.DuConeField", "Cone");
                AddItem(colObjectFields, "DustEngine.DuCubeField", "Cube");
                AddItem(colObjectFields, "DustEngine.DuCylinderField", "Cylinder");
                AddItem(colObjectFields, "DustEngine.DuDirectionalField", "Directional");
                AddItem(colObjectFields, "DustEngine.DuRadialField", "Radial");
                AddItem(colObjectFields, "DustEngine.DuSphereField", "Sphere");
                AddItem(colObjectFields, "DustEngine.DuTorusField", "Torus");
            }
            m_ColumnRecords.Add(colObjectFields);
            m_ColsCount++;

            var colMathFields = new ColumnRecord() {title = "Math Fields"};
            {
                // @todo!
                // AddItem(colMathFields, "DustEngine.DuSolidField", "Solid");
                // AddItem(colMathFields, "DustEngine.DuStepField", "Step");
                // AddItem(colMathFields, "DustEngine.DuTimeField", "Time");
                //
                // AddItem(colMathFields, "DustEngine.DuClampField", "Clamp");
                // AddItem(colMathFields, "DustEngine.DuCurveField", "Curve");
                // AddItem(colMathFields, "DustEngine.DuInvertField", "Invert");
                // AddItem(colMathFields, "DustEngine.DuRangeMapField", "RangeMap");
                // AddItem(colMathFields, "DustEngine.DuRemapField", "Remap");
            }
            m_ColumnRecords.Add(colMathFields);
            m_ColsCount++;
        }

        private void AddItem(ColumnRecord columnRecord, string className, string title)
        {
            CellRecord button;
            button.className = className;
            button.title = title;
            columnRecord.cells.Add(button);

            m_RowsCount = Mathf.Max(m_ColsCount, columnRecord.cells.Count);
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
                    cntFields.image = Icons.GetTextureByClassName(cellRecord.className);
                    cntFields.text = cellRecord.title;

                    if (DustGUI.IconButton(cntFields, BUTTON_WIDTH, BUTTON_HEIGHT, btnStyle))
                    {
                        DuField.AddFieldComponentByType(m_FieldsMap.GetParentGameObject(), System.Type.GetType(cellRecord.className));
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
