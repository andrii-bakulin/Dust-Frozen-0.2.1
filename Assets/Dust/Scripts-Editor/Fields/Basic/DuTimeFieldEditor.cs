﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTimeField)), CanEditMultipleObjects]
    public class DuTimeFieldEditor : DuFieldEditor
    {
        protected DuProperty m_TimeMode;
        protected DuProperty m_TimeScale;
        protected DuProperty m_Offset;

        protected DuRemappingEditor m_RemappingEditor;

        void OnEnable()
        {
            OnEnableField();

            m_TimeMode = FindProperty("m_TimeMode", "Mode");
            m_TimeScale = FindProperty("m_TimeScale", "Time Scale");
            m_Offset = FindProperty("m_Offset", "Offset");

            m_RemappingEditor = new DuRemappingEditor((target as DuTimeField).remapping, serializedObject.FindProperty("m_Remapping"));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuTimeField.Parameters"))
            {
                PropertyField(m_TimeMode);
                PropertyExtendedSlider(m_TimeScale, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_Offset, 0f, 1f, 0.01f);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Generate preview

                var timeMode = (DuTimeField.TimeMode) m_TimeMode.enumValueIndex;
                var timeScale = m_TimeScale.valFloat;
                var offset = m_Offset.valFloat;

                var previewDynamic = true;
                var previewLength = DuSessionState.GetFloat("DuTimeField.Preview.Length", 3f);
                var previewTitle = "Preview (" + previewLength.ToString("F1") + " sec)";

                // Small trick for "None" timeMode: it's always show be "Linear"
                if (timeMode == DuTimeField.TimeMode.Linear)
                {
                    timeScale = 1f;
                    offset = 0f;

                    previewDynamic = false;
                    previewLength = 1f;
                    previewTitle = "Preview";
                }

                AnimationCurve curve = new AnimationCurve();

                for (int i = 0; i < 200; i++)
                {
                    float innerOffset = i / 200f;
                    float globalOffset = (innerOffset + offset) * timeScale * previewLength;
                    float weight = (target as DuTimeField).GetPowerByTimeMode(timeMode, globalOffset);

                    curve.AddKey(new Keyframe(innerOffset, weight) {weightedMode = WeightedMode.Both});
                }

                Space();

                DustGUI.Lock();
                DustGUI.Field(previewTitle, curve, 0, 30, Color.green, new Rect(0f, 0f, 1f, 1f));
                DustGUI.Unlock();

                if (previewDynamic)
                {
                    previewLength = DustGUI.SliderExt.Create(1f, 5f, 0.1f, 1f, 100f).Draw("Preview Length", previewLength);
                    DuSessionState.SetFloat("DuTimeField.Preview.Length", previewLength);
                }
            }
            DustGUI.FoldoutEnd();

            m_RemappingEditor.OnInspectorGUI(false);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
