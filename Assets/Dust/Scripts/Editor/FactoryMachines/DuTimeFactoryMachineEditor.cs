using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTimeFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTimeFactoryMachineEditor : DuPRSFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static DuTimeFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuTimeFactoryMachine), "Time");
        }

        [MenuItem("Dust/Factory/Machines/Time")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuTimeFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseParameters();
            OnInspectorGUI_TransformBlock();
            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
