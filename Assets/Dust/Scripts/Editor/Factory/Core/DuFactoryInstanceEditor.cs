using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFactoryInstance))]
    // [CanEditMultipleObjects] -> Cannot!
    public class DuFactoryInstanceEditor : DuEditor
    {
        private DuProperty m_InstanceIndex;
        private DuProperty m_InstanceOffset;
        private DuProperty m_MaterialReferences;

        void OnEnable()
        {
            m_InstanceIndex = FindProperty("m_InstanceIndex", "Instance Index");
            m_InstanceOffset = FindProperty("m_InstanceOffset", "Instance Offset");
            m_MaterialReferences = FindProperty("m_MaterialReferences");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DuFactoryInstance mainScript = target as DuFactoryInstance;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            bool IsFreeInstance = Dust.IsNull(mainScript.parentFactory);

            if (!IsFreeInstance)
            {
                PropertyFieldOrLock(m_InstanceIndex, true);
                PropertyFieldOrLock(m_InstanceOffset, true);
                Space();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // @DUST.todo: make control more then one element

            int count = m_MaterialReferences.property.arraySize;

            if (count == 0)
            {
                if (DustGUI.Button("Add Material Reference"))
                {
                    m_MaterialReferences.property.InsertArrayElementAtIndex(0);
                    count++;

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                    // Set default values

                    var matRef = mainScript.GetDefaultMaterialReference();

                    var item = m_MaterialReferences.property.GetArrayElementAtIndex(0);

                    DuProperty m_MeshRenderer      = FindProperty(item, "m_MeshRenderer");
                    DuProperty m_ColorPropertyName = FindProperty(item, "m_ColorPropertyName");
                    DuProperty m_PowerPropertyName = FindProperty(item, "m_PowerPropertyName");
                    DuProperty m_UvwPropertyName   = FindProperty(item, "m_UvwPropertyName");

                    m_MeshRenderer.property.objectReferenceValue = matRef.meshRenderer;
                    m_ColorPropertyName.property.stringValue = matRef.colorPropertyName;
                    m_PowerPropertyName.property.stringValue = matRef.powerPropertyName;
                    m_UvwPropertyName.property.stringValue = matRef.uvwPropertyName;
                }
            }
            else
            {
                if (DustGUI.Button("Remove Material Reference"))
                {
                    m_MaterialReferences.property.DeleteArrayElementAtIndex(0);
                    count--;
                }
            }

            for (int i = 0; i < m_MaterialReferences.property.arraySize; i++)
            {
                var item = m_MaterialReferences.property.GetArrayElementAtIndex(i);

                DuProperty m_MeshRenderer      = FindProperty(item, "m_MeshRenderer", "Mesh Renderer");
                DuProperty m_ColorPropertyName = FindProperty(item, "m_ColorPropertyName", "Color");
                DuProperty m_PowerPropertyName = FindProperty(item, "m_PowerPropertyName", "Power");
                DuProperty m_UvwPropertyName   = FindProperty(item, "m_UvwPropertyName", "UVW");
                DuProperty m_OriginalMaterial  = FindProperty(item, "m_OriginalMaterial", "Original Material");

                DustGUI.Header("Reference #" + (i + 1));
                PropertyFieldOrLock(m_MeshRenderer, !IsFreeInstance);
                PropertyFieldOrLock(m_ColorPropertyName, !IsFreeInstance);
                PropertyFieldOrLock(m_PowerPropertyName, !IsFreeInstance);
                PropertyFieldOrLock(m_UvwPropertyName, !IsFreeInstance);

                if (!IsFreeInstance)
                {
                    Space();
                    PropertyFieldOrLock(m_OriginalMaterial, true);
                }

                Space();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (!IsFreeInstance && Dust.IsNotNull(mainScript.stateZero))
            {
                if (DustGUI.FoldoutBegin("Default State", "DuFactoryInstance.DefaultStates"))
                {
                    DustGUI.Lock();
                    DustGUI.Field("Position", mainScript.stateZero.position.ToRound(3));
                    DustGUI.Field("Rotation", mainScript.stateZero.rotation.ToRound(3));
                    DustGUI.Field("Scale", mainScript.stateZero.scale.ToRound(3));
                    Space();
                    DustGUI.Field("Power", mainScript.stateZero.power);
                    DustGUI.Field("Color", mainScript.stateZero.color);
                    DustGUI.Field("Color Values", mainScript.stateZero.color.ToVector3(2));
                    DustGUI.Field("Color RGB", (mainScript.stateZero.color * 255).ToVector3Int());
                    DustGUI.Field("UVW", mainScript.stateZero.uvw.ToRound(3));
                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            if (!IsFreeInstance && Dust.IsNotNull(mainScript.stateDynamic))
            {
                if (DustGUI.FoldoutBegin("Dynamic End State", "DuFactoryInstance.DynamicStates"))
                {
                    DustGUI.Lock();
                    DustGUI.Field("Position", mainScript.stateDynamic.position.ToRound(3));
                    DustGUI.Field("Rotation", mainScript.stateDynamic.rotation.ToRound(3));
                    DustGUI.Field("Scale", mainScript.stateDynamic.scale.ToRound(3));
                    Space();
                    DustGUI.Field("Power", mainScript.stateDynamic.power);
                    DustGUI.Field("Color", mainScript.stateDynamic.color);
                    DustGUI.Field("Color Values", mainScript.stateDynamic.color.ToVector3(2));
                    DustGUI.Field("Color RGB", (mainScript.stateDynamic.color * 255).ToVector3Int());
                    DustGUI.Field("UVW", mainScript.stateDynamic.uvw.ToRound(3));
                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
