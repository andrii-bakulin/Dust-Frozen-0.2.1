using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuParallaxController)), CanEditMultipleObjects]
    public class DuParallaxControllerEditor : DuEditor
    {
        private DuProperty m_ParallaxControl;
        private DuProperty m_Offset;
        private DuProperty m_TimeScale;
        private DuProperty m_Freeze;

        private DuProperty m_UpdateMode;

        void OnEnable()
        {
            m_ParallaxControl = FindProperty("m_ParallaxControl", "Parallax Control");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_TimeScale = FindProperty("m_TimeScale", "Time Scale");
            m_Freeze = FindProperty("m_Freeze", "Freeze");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.FoldoutBegin("Control");
            {
                PropertyField(m_ParallaxControl);

                switch ((DuParallaxController.ParallaxControl) m_ParallaxControl.enumValueIndex)
                {
                    case DuParallaxController.ParallaxControl.Manual:
                        PropertyExtendedSlider(m_Offset, 0f, 10f, 0.01f);
                        break;

                    case DuParallaxController.ParallaxControl.Time:
                        PropertyExtendedSlider(m_Offset, 0f, 10f, 0.01f);
                        PropertyExtendedSlider(m_TimeScale, -10f, 10f, 0.01f);
                        PropertyField(m_Freeze);
                        break;
                }

                Space();
            }
            DustGUI.FoldoutEnd();

            DustGUI.FoldoutBegin("Others");
            {
                PropertyField(m_UpdateMode);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
