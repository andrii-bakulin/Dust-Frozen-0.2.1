using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDeformMesh)), CanEditMultipleObjects]
    public class DuDeformMeshEditor : DuEditor
    {
        private DuProperty m_Deformers;

        private DuProperty m_RecalculateBounds;
        private DuProperty m_RecalculateNormals;
        private DuProperty m_RecalculateTangents;

        void OnEnable()
        {
            m_Deformers = FindProperty("m_Deformers", "Deformers");

            m_RecalculateBounds = FindProperty("m_RecalculateBounds", "Recalculate Bounds");
            m_RecalculateNormals = FindProperty("m_RecalculateNormals", "Recalculate Normals");
            m_RecalculateTangents = FindProperty("m_RecalculateTangents", "Recalculate Tangents");
        }

        public override void OnInspectorGUI()
        {
            var main = target as DuDeformMesh;

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Dust.IsNull(main.meshOriginal))
            {
                if (Dust.IsNotNull(main.meshFilter.sharedMesh) && !main.meshFilter.sharedMesh.isReadable)
                {
                    DustGUI.HelpBoxError("Mesh is not readable." + "\n" + "Enabled \"Read/Write Enabled\" flag for this mesh.");
                }
                else if (main.enabled)
                {
                    main.ReEnableMeshForDeformer();
                }
            }

            PropertyField(m_Deformers);

            Space();

            PropertyField(m_RecalculateBounds);
            PropertyField(m_RecalculateNormals);
            PropertyField(m_RecalculateTangents);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int verticesCount = main.GetMeshVerticesCount();

            if (verticesCount >= 0)
            {
                Space();
                DustGUI.HelpBoxInfo("Mesh has " + verticesCount + " vertices");
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
