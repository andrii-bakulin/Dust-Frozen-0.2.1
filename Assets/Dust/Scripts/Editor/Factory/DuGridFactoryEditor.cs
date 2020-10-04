using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuGridFactory))]
    [CanEditMultipleObjects]
    public class DuGridFactoryEditor : DuFactoryEditor
    {
        private DuProperty m_Count;
        private DuProperty m_Size;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Factory/Grid Factory")]
        public static void AddComponent()
        {
            CreateFactoryByType(typeof(DuGridFactory));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactory();

            m_Count = FindProperty("m_Count", "Count");
            m_Size = FindProperty("m_Size", "Size");
        }

        public override void OnInspectorGUI()
        {
            BeginData();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Grid", "DuFactory.Grid"))
            {
                PropertyField(m_Count);
                PropertyField(m_Size);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireRebuildInstances |= m_Count.isChanged;

            m_IsRequireResetupInstances |= m_Size.isChanged;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Objects();
            OnInspectorGUI_FactoryMachines();
            OnInspectorGUI_Transform();
            OnInspectorGUI_Display();
            OnInspectorGUI_Tools();

            //----------------------------------------------------------------------------------------------------------
            // Validate & Normalize Data

            if (m_Count.isChanged)
                m_Count.valVector3Int = DuGridFactory.Normalizer.Count(m_Count.valVector3Int);

            //----------------------------------------------------------------------------------------------------------

            CommitDataAndUpdateStates();
        }
    }
}
