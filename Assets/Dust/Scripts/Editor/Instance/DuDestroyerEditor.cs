using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDestroyer)), CanEditMultipleObjects]
    public class DuDestroyerEditor : DuEditor
    {
        private DuProperty m_DestroyMode;

        private DuProperty m_Timeout;
        private DuProperty m_TimeoutRange;

        private DuProperty m_VolumeCenterMode;
        private DuProperty m_VolumeCenter;
        private DuProperty m_VolumeSize;

        void OnEnable()
        {
            m_DestroyMode = FindProperty("m_DestroyMode", "Destroy Mode");

            m_Timeout = FindProperty("m_Timeout", "Timeout");
            m_TimeoutRange = FindProperty("m_TimeoutRange", "Timeout Range");

            m_VolumeCenterMode = FindProperty("m_VolumeCenterMode", "Volume Center Mode");
            m_VolumeCenter = FindProperty("m_VolumeCenter", "Center");
            m_VolumeSize = FindProperty("m_VolumeSize", "Size");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_DestroyMode);

            switch ((DuDestroyer.DestroyMode) m_DestroyMode.enumValueIndex)
            {
                case DuDestroyer.DestroyMode.Manual:
                    DustGUI.HelpBoxInfo("To destroy object you need call DestroyNow() method");
                    break;

                case DuDestroyer.DestroyMode.Time:
                    PropertyField(m_Timeout);
                    break;

                case DuDestroyer.DestroyMode.TimeRange:
                    PropertyFieldRange(m_TimeoutRange);
                    break;

                case DuDestroyer.DestroyMode.AliveZone:
                case DuDestroyer.DestroyMode.DeadZone:
                    PropertyField(m_VolumeCenterMode);

                    switch ((DuDestroyer.VolumeCenterMode) m_VolumeCenterMode.enumValueIndex)
                    {
                        case DuDestroyer.VolumeCenterMode.StartPosition:
                            if (Application.isPlaying)
                                PropertyFieldOrLock(m_VolumeCenter, true);
                            else
                                DustGUI.StaticTextField("Center", "Will be set up from object position when it'll appear in scene");
                            PropertyField(m_VolumeSize, "Size");
                            break;

                        case DuDestroyer.VolumeCenterMode.World:
                            PropertyField(m_VolumeCenter);
                            PropertyField(m_VolumeSize);
                            break;
                    }

                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Post Update

            if (m_VolumeSize.isChanged)
                m_VolumeSize.valVector3 = DuDestroyer.Normalizer.VolumeSize(m_VolumeSize.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
