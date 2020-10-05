using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuSpawner))]
    [CanEditMultipleObjects]
    public class DuSpawnerEditor : DuEditor
    {
        private DuProperty m_SpawnPointMode;
        private DuProperty m_SpawnPoints;
        private DuProperty m_SpawnPointsIterate;
        private DuProperty m_SpawnPointsSeed;

        private DuProperty m_SpawnObjects;
        private DuProperty m_SpawnObjectsIterate;
        private DuProperty m_SpawnObjectsSeed;

        private DuProperty m_IntervalMode;
        private DuProperty m_Interval;
        private DuProperty m_IntervalRange;

        private DuProperty m_ParentMode;
        private DuProperty m_Limit;
        private DuProperty m_SpawnOnAwake;
        private DuProperty m_AllowMultiSpawn;

        void OnEnable()
        {
            m_SpawnPointMode = FindProperty("m_SpawnPointMode", "Spawn Point Mode");
            m_SpawnPoints = FindProperty("m_SpawnPoints", "Spawn Points");
            m_SpawnPointsIterate = FindProperty("m_SpawnPointsIterate", "Spawn Points Iterate");
            m_SpawnPointsSeed = FindProperty("m_SpawnPointsSeed", "Seed");

            m_SpawnObjects = FindProperty("m_SpawnObjects", "Spawn Objects");
            m_SpawnObjectsIterate = FindProperty("m_SpawnObjectsIterate", "Objects Iterate");
            m_SpawnObjectsSeed = FindProperty("m_SpawnObjectsSeed", "Seed");

            m_IntervalMode = FindProperty("m_IntervalMode", "Spawn Interval");
            m_Interval = FindProperty("m_Interval", "Interval");
            m_IntervalRange = FindProperty("m_IntervalRange", "Interval Range");

            m_ParentMode = FindProperty("m_ParentMode", "Set Parent As");
            m_Limit = FindProperty("m_Limit", "Limit");
            m_SpawnOnAwake = FindProperty("m_SpawnOnAwake", "Spawn On Awake");
            m_AllowMultiSpawn = FindProperty("m_AllowMultiSpawn", "Allow Multi Spawn");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SpawnPointMode);

            switch ((DuSpawner.SpawnPointMode) m_SpawnPointMode.enumValueIndex)
            {
                case DuSpawner.SpawnPointMode.Self:
                    break;

                case DuSpawner.SpawnPointMode.Points:
                    PropertyField(m_SpawnPoints);

                    if (m_SpawnPoints.property.arraySize > 1)
                    {
                        PropertyField(m_SpawnPointsIterate);

                        if ((DuSpawner.IterateMode) m_SpawnPointsIterate.enumValueIndex == DuSpawner.IterateMode.Random)
                            PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    }
                    break;
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SpawnObjects);

            if (m_SpawnObjects.property.arraySize > 1)
            {
                PropertyField(m_SpawnObjectsIterate);

                if ((DuSpawner.IterateMode) m_SpawnObjectsIterate.enumValueIndex == DuSpawner.IterateMode.Random)
                    PropertySeedRandomOrFixed(m_SpawnObjectsSeed);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_IntervalMode);

            DuSpawner.IntervalMode intervalMode = (DuSpawner.IntervalMode) m_IntervalMode.enumValueIndex;

            switch (intervalMode)
            {
                case DuSpawner.IntervalMode.Fixed:
                    PropertyField(m_Interval);
                    break;

                case DuSpawner.IntervalMode.Range:
                    PropertyFieldRange(m_IntervalRange);
                    break;
            }

            Space();

            PropertyField(m_Limit);
            PropertyField(m_SpawnOnAwake);
            PropertyField(m_AllowMultiSpawn);

            Space();

            PropertyField(m_ParentMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Interval.isChanged)
                m_Interval.valFloat = DuSpawner.Normalizer.IntervalValue(m_Interval.valFloat);

            if (m_IntervalRange.isChanged)
                m_IntervalRange.FindProperty("min").floatValue = DuSpawner.Normalizer.IntervalValue(m_IntervalRange.FindProperty("min").floatValue);

            if (m_IntervalRange.isChanged)
                m_IntervalRange.FindProperty("max").floatValue = DuSpawner.Normalizer.IntervalValue(m_IntervalRange.FindProperty("max").floatValue);

            if (m_Limit.isChanged)
                m_Limit.valInt = DuSpawner.Normalizer.Limit(m_Limit.valInt);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
