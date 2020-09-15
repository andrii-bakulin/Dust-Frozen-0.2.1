using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuClampField)), CanEditMultipleObjects]
    public class DuClampFieldEditor : DuFieldEditor
    {
        protected DuProperty m_PowerClampMode;
        protected DuProperty m_PowerClampMin;
        protected DuProperty m_PowerClampMax;

        protected DuProperty m_ColorClampMode;
        protected DuProperty m_ColorClampMin;
        protected DuProperty m_ColorClampMax;

        void OnEnable()
        {
            OnEnableField();

            m_PowerClampMode = FindProperty("m_PowerClampMode", "Clamp Mode");
            m_PowerClampMin = FindProperty("m_PowerClampMin", "Min");
            m_PowerClampMax = FindProperty("m_PowerClampMax", "Max");

            m_ColorClampMode = FindProperty("m_ColorClampMode", "Clamp Mode");
            m_ColorClampMin = FindProperty("m_ColorClampMin", "Min");
            m_ColorClampMax = FindProperty("m_ColorClampMax", "Max");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Power", "DuClampField.Power"))
            {
                PropertyField(m_PowerClampMode);

                var powerClampMode = (DuClampField.ClampMode) m_PowerClampMode.enumValueIndex;
                bool isMinEnabled = powerClampMode == DuClampField.ClampMode.MinAndMax || powerClampMode == DuClampField.ClampMode.MinOnly;
                bool isMaxEnabled = powerClampMode == DuClampField.ClampMode.MinAndMax || powerClampMode == DuClampField.ClampMode.MaxOnly;

                if (!isMinEnabled) DustGUI.Lock();
                PropertyExtendedSlider(m_PowerClampMin, 0f, 1f, 0.01f);
                if (!isMinEnabled) DustGUI.Unlock();

                if (!isMaxEnabled) DustGUI.Lock();
                PropertyExtendedSlider(m_PowerClampMax, 0f, 1f, 0.01f);
                if (!isMaxEnabled) DustGUI.Unlock();
            }
            DustGUI.FoldoutEnd();

            if (DustGUI.FoldoutBegin("Color", "DuClampField.Color"))
            {
                PropertyField(m_ColorClampMode);

                var colorClampMode = (DuClampField.ClampMode) m_ColorClampMode.enumValueIndex;
                bool isMinEnabled = colorClampMode == DuClampField.ClampMode.MinAndMax || colorClampMode == DuClampField.ClampMode.MinOnly;
                bool isMaxEnabled = colorClampMode == DuClampField.ClampMode.MinAndMax || colorClampMode == DuClampField.ClampMode.MaxOnly;

                if (!isMinEnabled) DustGUI.Lock();
                PropertyField(m_ColorClampMin);
                if (!isMinEnabled) DustGUI.Unlock();

                if (!isMaxEnabled) DustGUI.Lock();
                PropertyField(m_ColorClampMax);
                if (!isMaxEnabled) DustGUI.Unlock();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
