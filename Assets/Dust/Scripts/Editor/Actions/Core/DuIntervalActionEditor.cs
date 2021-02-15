using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuIntervalActionEditor : DuActionEditor
    {
        protected DuProperty m_Duration;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Duration = FindProperty("m_Duration", "Duration");
        }

        protected override void InspectorCommitUpdates()
        {
            if (m_Duration.isChanged)
                m_Duration.valFloat = DuIntervalAction.Normalizer.Duration(m_Duration.valFloat);

            base.InspectorCommitUpdates();
        }
    }
}
