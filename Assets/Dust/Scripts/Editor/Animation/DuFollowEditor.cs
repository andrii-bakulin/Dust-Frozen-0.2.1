using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFollow)), CanEditMultipleObjects]
    public class DuFollowEditor : DuEditor
    {
        private DuProperty m_FollowObject;
        private DuProperty m_FollowDistance;

        private DuProperty m_UseSmoothDamp;
        private DuProperty m_SmoothTime;

        private DuProperty m_UpdateMode;

        void OnEnable()
        {
            m_FollowObject = FindProperty("m_FollowObject", "Follow Object");
            m_FollowDistance = FindProperty("m_FollowDistance", "Follow Distance");

            m_UseSmoothDamp = FindProperty("m_UseSmoothDamp", "Use Smooth Damp");
            m_SmoothTime = FindProperty("m_SmoothTime", "Smooth Time");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_FollowObject);
            PropertyField(m_FollowDistance);

            Space();

            PropertyField(m_UseSmoothDamp);
            PropertyFieldOrHide(m_SmoothTime, !m_UseSmoothDamp.IsTrue);

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_FollowObject.isChanged)
            {
                GameObject followObject = m_FollowObject.GameObjectReference;

                if (Dust.IsNotNull(followObject))
                {
                    foreach (var entity in GetSerializedEntitiesByTargets())
                    {
                        Vector3 followDistance = (entity.target as DuFollow).transform.position - followObject.transform.position;

                        entity.serializedObject.FindProperty("m_FollowDistance").vector3Value = DuVector3.Round(followDistance);
                        entity.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            if (m_SmoothTime.isChanged)
                m_SmoothTime.valVector3 = DuFollow.Normalizer.SmoothTime(m_SmoothTime.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
