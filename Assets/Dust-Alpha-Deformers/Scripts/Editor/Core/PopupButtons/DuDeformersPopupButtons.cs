using UnityEngine;
using UnityEditor;

#if DUST_ALPHA_DEFORMERS
namespace DustEngine.DustEditor
{
    public class DuDeformersPopupButtons : DuPopupButtons
    {
        private DuDeformMeshEditor m_DeformMesh;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddDeformer(System.Type type, string title)
        {
            AddEntity("Deformers", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static DuPopupButtons Popup(DuDeformMeshEditor deformMesh)
        {
            var popup = new DuDeformersPopupButtons();
            popup.m_DeformMesh = deformMesh;

            GenerateColumn(popup, "Deformers", "Deformers");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            DuDeformerEditor.AddDeformerComponentByType((m_DeformMesh.target as DuMonoBehaviour).gameObject, cellRecord.type);
            return true;
        }
    }
}
#endif
