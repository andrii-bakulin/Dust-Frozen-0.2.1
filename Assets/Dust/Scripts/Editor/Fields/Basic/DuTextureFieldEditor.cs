﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTextureField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTextureFieldEditor : DuObjectFieldEditor
    {
        private DuProperty m_Texture;
        private DuProperty m_WrapMode;

        private DuProperty m_Width;
        private DuProperty m_Height;
        private DuProperty m_Direction;

        private DuProperty m_FlipX;
        private DuProperty m_FlipY;

        private DuProperty m_PowerSource;
        private DuProperty m_ApplyPowerToAlpha;

        //--------------------------------------------------------------------------------------------------------------

        static DuTextureFieldEditor()
        {
            DuPopupButtons.AddBasicField(typeof(DuTextureField), "Texture");
        }

        [MenuItem("Dust/Fields/Basic Fields/Texture")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuTextureField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Texture = FindProperty("m_Texture", "Texture");
            m_WrapMode = FindProperty("m_WrapMode", "Wrap Mode");

            m_Width = FindProperty("m_Width", "Width");
            m_Height = FindProperty("m_Height", "Height");
            m_Direction = FindProperty("m_Direction", "Direction");

            m_FlipX = FindProperty("m_FlipX", "Flip X");
            m_FlipY = FindProperty("m_FlipY", "Flip Y");

            m_PowerSource = FindProperty("m_PowerSource", "Power Source");

            m_ApplyPowerToAlpha = FindProperty("m_ApplyPowerToAlpha", "Apply Power To Alpha");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuTextureField.Params"))
            {
                PropertyField(m_Texture);
                PropertyField(m_WrapMode);
                PropertyExtendedSlider(m_Width, 0f, 50f, 0.01f);
                PropertyExtendedSlider(m_Height, 0f, 50f, 0.01f);
                PropertyField(m_Direction);
                Space();

                PropertyField(m_FlipX);
                PropertyField(m_FlipY);
                Space();

                DustGUI.Header("Power Impact");
                PropertyField(m_PowerSource);
                Space();

                DustGUI.Header("Color Impact");
                PropertyField(m_ApplyPowerToAlpha);
                Space();

                PropertyField(m_CustomHint);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_RemappingBlock();

            if ((DuTextureField.ColorComponent) m_PowerSource.enumValueIndex != DuTextureField.ColorComponent.Ignore)
            {
                m_RemappingEditor.OnInspectorGUI(false, false);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
