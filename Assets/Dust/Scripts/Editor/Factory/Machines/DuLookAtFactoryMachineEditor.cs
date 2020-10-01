﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuLookAtFactoryMachine))]
    [CanEditMultipleObjects]
    public class DuLookAtFactoryMachineEditor : DuFactoryMachineEditor
    {
        private DuProperty m_TargetMode;
        private DuProperty m_TargetObject;

        private DuProperty m_UpVectorMode;
        private DuProperty m_UpVectorObject;

        private DuProperty m_LockAxisX;
        private DuProperty m_LockAxisY;
        private DuProperty m_LockAxisZ;

        void OnEnable()
        {
            OnEnableFactoryMachine();

            m_TargetMode = FindProperty("m_TargetMode", "Target Mode");
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");

            m_UpVectorMode = FindProperty("m_UpVectorMode", "Up Vector Mode");
            m_UpVectorObject = FindProperty("m_UpVectorObject", "Up Vector Object");

            m_LockAxisX = FindProperty("m_LockAxisX", "Lock Axis X");
            m_LockAxisY = FindProperty("m_LockAxisY", "Lock Axis Y");
            m_LockAxisZ = FindProperty("m_LockAxisZ", "Lock Axis Z");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuLookAtFactoryMachine.Parameters"))
            {
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyField(m_TargetMode);
                PropertyFieldOrHide(m_TargetObject, (DuLookAtFactoryMachine.TargetMode) m_TargetMode.enumValueIndex
                                                    != DuLookAtFactoryMachine.TargetMode.ObjectTarget);
                Space();

                PropertyField(m_UpVectorMode);
                PropertyFieldOrHide(m_UpVectorObject, (DuLookAtFactoryMachine.UpVectorMode) m_UpVectorMode.enumValueIndex
                                                      != DuLookAtFactoryMachine.UpVectorMode.Object);
                Space();

                PropertyField(m_LockAxisX);
                PropertyField(m_LockAxisY);
                PropertyField(m_LockAxisZ);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Falloff();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
