using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCubeField)), CanEditMultipleObjects]
    public class DuCubeFieldEditor : DuObjectFieldEditor
    {
        private DuProperty m_Size;

        void OnEnable()
        {
            OnEnableField();

            m_Size = FindProperty("m_Size", "Size");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field", "DuCubeField.Params"))
            {
                PropertyField(m_Size);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmosBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Size.isChanged)
                m_Size.valVector3 = DuCubeField.ShapeNormalizer.Size(m_Size.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            foreach (var subTarget in targets)
            {
                var origin = subTarget as DuCubeField;

                if (m_Size.isChanged || DustGUI.IsUndoRedoPerformed())
                    origin.ResetCalcData();
            }
        }
    }
}
#endif
