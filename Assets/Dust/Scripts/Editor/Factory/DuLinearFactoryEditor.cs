﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuLinearFactory))]
    [CanEditMultipleObjects]
    public class DuLinearFactoryEditor : DuFactoryEditor
    {
        private DuProperty m_Count;
        private DuProperty m_Offset;
        private DuProperty m_Amount;
        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;
        private DuProperty m_StepRotation;

        void OnEnable()
        {
            OnEnableFactory();

            m_Count = FindProperty("m_Count", "Count");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_Amount = FindProperty("m_Amount", "Amount");
            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");
            m_StepRotation = FindProperty("m_StepRotation", "Step Rotation");
        }

        public override void OnInspectorGUI()
        {
            BeginData();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Linear", "DuFactory.Linear"))
            {
                PropertyExtendedIntSlider(m_Count, 0, 100, 1, 0);
                PropertyExtendedIntSlider(m_Offset, 0, 100, 1, 0);
                Space();
                PropertyField(m_Position);
                PropertyField(m_Rotation);
                PropertyField(m_Scale);
                PropertyExtendedSlider(m_Amount, 0f, 1f, 0.01f);
                Space();
                PropertyField(m_StepRotation);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireRebuildClones |= m_Count.isChanged;

            m_IsRequireResetupClones |= m_Offset.isChanged;
            m_IsRequireResetupClones |= m_Amount.isChanged;
            m_IsRequireResetupClones |= m_Position.isChanged;
            m_IsRequireResetupClones |= m_Rotation.isChanged;
            m_IsRequireResetupClones |= m_Scale.isChanged;
            m_IsRequireResetupClones |= m_StepRotation.isChanged;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Objects();
            OnInspectorGUI_FactoryMachines();
            OnInspectorGUI_Transform();
            OnInspectorGUI_Display();
            OnInspectorGUI_Tools();

            //----------------------------------------------------------------------------------------------------------
            // Validate & Normalize Data

            if (m_Count.isChanged)
                m_Count.valInt = DuLinearFactory.Normalizer.Count(m_Count.valInt);

            if (m_Offset.isChanged)
                m_Offset.valInt = DuLinearFactory.Normalizer.Offset(m_Offset.valInt);

            //----------------------------------------------------------------------------------------------------------

            CommitDataAndUpdateStates();
        }
    }
}