﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(SpawnAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class SpawnActionEditor : InstantActionEditor
    {
        private DuProperty m_SpawnObjects;
        private DuProperty m_SpawnObjectsIterate;
        private DuProperty m_SpawnObjectsSeed;

        private DuProperty m_SpawnPointMode;
        private DuProperty m_SpawnPoints;
        private DuProperty m_SpawnPointsIterate;
        private DuProperty m_SpawnPointsSeed;

        private DuProperty m_MultipleSpawnEnabled;
        private DuProperty m_MultipleSpawnCount;
        private DuProperty m_MultipleSpawnSeed;

        private DuProperty m_ResetTransform;
        private DuProperty m_ParentMode;

        //--------------------------------------------------------------------------------------------------------------

        static SpawnActionEditor()
        {
            ActionsPopupButtons.AddActionTransform(typeof(SpawnAction), "Spawn");
        }

        [MenuItem("Dust/Actions/Spawn")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Spawn Action", typeof(SpawnAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_SpawnObjects = FindProperty("m_SpawnObjects", "Objects");
            m_SpawnObjectsIterate = FindProperty("m_SpawnObjectsIterate", "Objects Iterate");
            m_SpawnObjectsSeed = FindProperty("m_SpawnObjectsSeed", "Seed");

            m_SpawnPointMode = FindProperty("m_SpawnPointMode", "Spawn At");
            m_SpawnPoints = FindProperty("m_SpawnPoints", "Spawn Points");
            m_SpawnPointsIterate = FindProperty("m_SpawnPointsIterate", "Spawn Points Iterate");
            m_SpawnPointsSeed = FindProperty("m_SpawnPointsSeed", "Seed");

            m_MultipleSpawnEnabled = FindProperty("m_MultipleSpawnEnabled", "Enabled");
            m_MultipleSpawnCount = FindProperty("m_MultipleSpawnCount", "Spawn Count");
            m_MultipleSpawnSeed = FindProperty("m_MultipleSpawnSeed", "Seed");

            m_ResetTransform = FindProperty("m_ResetTransform", "Reset Transform");
            m_ParentMode = FindProperty("m_ParentMode", "Assign Parent As");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "SpawnAction.Parameters"))
            {
                DustGUI.Header("Objects To Spawn");

                PropertyField(m_SpawnObjects);

                if (m_SpawnObjects.property.arraySize > 1)
                {
                    PropertyField(m_SpawnObjectsIterate);

                    if ((SpawnAction.IterateMode) m_SpawnObjectsIterate.valInt == SpawnAction.IterateMode.Random)
                        PropertySeedRandomOrFixed(m_SpawnObjectsSeed);
                }

                Space();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.Header("Spawn At Points");

                PropertyField(m_SpawnPointMode);

                switch ((SpawnAction.SpawnPointMode) m_SpawnPointMode.valInt)
                {
                    case SpawnAction.SpawnPointMode.Self:
                        break;

                    case SpawnAction.SpawnPointMode.Points:
                        PropertyField(m_SpawnPoints);

                        if (m_SpawnPoints.property.arraySize > 1)
                        {
                            PropertyField(m_SpawnPointsIterate);

                            if ((SpawnAction.IterateMode) m_SpawnPointsIterate.valInt == SpawnAction.IterateMode.Random)
                                PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                        }
                        break;

                    default:
                        // Nothing to show
                        break;
                }

                Space();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.Header("Spawn Multiple Objects");

                PropertyField(m_MultipleSpawnEnabled);

                if (m_MultipleSpawnEnabled.IsTrue)
                {
                    PropertyFieldRange(m_MultipleSpawnCount, 0, 16, 1, 0, int.MaxValue);
                    PropertySeedRandomOrFixed(m_MultipleSpawnSeed);
                }

                Space();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                PropertyField(m_ResetTransform);
                PropertyField(m_ParentMode);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("SpawnAction");
            OnInspectorGUI_Extended("SpawnAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            // @notice: no need to NormalizeMultipleSpawnCount for m_MultipleSpawnCount.
            // It auto-normalized in PropertyFieldRange() method

            InspectorCommitUpdates();
        }
    }
}
