using UnityEditor;
using UnityEngine;

namespace DustEngine
{
    public abstract class DuDeformer : DuMonoBehaviour
    {
        protected static readonly Color k_GizmosColorMain = new Color(0.60f, 0.60f, 1.00f);

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected DuFieldsMap m_FieldsMap = DuFieldsMap.Deformer();
        public DuFieldsMap fieldsMap => m_FieldsMap;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GizmosVisibility m_GizmosVisibility = GizmosVisibility.DrawOnSelect;
        public GizmosVisibility gizmosVisibility
        {
            get => m_GizmosVisibility;
            set => m_GizmosVisibility = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected static void AddDeformerComponentByType(System.Type duDeformerType)
        {
            DuDeformMesh selectedDeformMesh = null;

            if (Dust.IsNotNull(Selection.activeGameObject))
            {
                selectedDeformMesh = Selection.activeGameObject.GetComponent<DuDeformMesh>();

                if (Dust.IsNull(selectedDeformMesh))
                {
                    if (Selection.activeGameObject.GetComponent<MeshFilter>())
                        selectedDeformMesh = Selection.activeGameObject.AddComponent<DuDeformMesh>();
                }
            }

            var gameObject = new GameObject();
            {
                DuDeformer deformer = gameObject.AddComponent(duDeformerType) as DuDeformer;

                if (Dust.IsNotNull(selectedDeformMesh))
                {
                    deformer.transform.parent = selectedDeformMesh.transform;
                    selectedDeformMesh.AddDeformer(deformer);
                }

                gameObject.name = deformer.DeformerName() + " Deformer";
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public virtual void UpdateMeshPointsCloud(ref Vector3[] pointsCloud, Transform meshTransform, float strength)
        {
            int pointsCount = pointsCloud.Length;

            Vector3 vLocalPosition; // vertex in local space of deformer object
            Vector3 vWorldPosition; // vertex in world space

            Matrix4x4 matrixMeshLocalToMeshWorld = meshTransform.localToWorldMatrix;
            Matrix4x4 matrixMeshWorldToDefmLocal = transform.worldToLocalMatrix;

            Matrix4x4 matrixDefmLocalToMeshWorld = transform.localToWorldMatrix;
            Matrix4x4 matrixMeshWorldToMeshLocal = meshTransform.worldToLocalMatrix;

            for (int i = 0; i < pointsCount; i++)
            {
                float weight = 1f;

                // 1. Transform vertex position: local-in-mesh > world
                // 2. Transform vertex position: world > local-in-deformer
                vWorldPosition = matrixMeshLocalToMeshWorld.MultiplyPoint(pointsCloud[i]);
                vLocalPosition = matrixMeshWorldToDefmLocal.MultiplyPoint(vWorldPosition);

                if (fieldsMap.HasFields())
                {
                    bool result = fieldsMap.Calculate(vWorldPosition, (float) i / pointsCount, out weight);

                    if (result && DuMath.IsZero(weight))
                        continue;
                }

                if (DeformPoint(ref vLocalPosition, weight * strength) == false)
                    continue;

                // 1. Back-Transform vertex position: local-in-deformer > world
                // 2. Back-Transform vertex position: world > local-in-mesh
                vWorldPosition = matrixDefmLocalToMeshWorld.MultiplyPoint(vLocalPosition);
                pointsCloud[i] = matrixMeshWorldToMeshLocal.MultiplyPoint(vWorldPosition);
            }
        }

        public abstract string DeformerName();

        public abstract bool DeformPoint(ref Vector3 localPosition, float strength = 1f);

        //--------------------------------------------------------------------------------------------------------------
        // Helpers

        protected bool IsPointInsideDeformBox(Vector3 point, Vector3 size)
        {
            return -size.x / 2f <= point.x && point.x <= +size.x / 2f &&
                   -size.y / 2f <= point.y && point.y <= +size.y / 2f &&
                   -size.z / 2f <= point.z && point.z <= +size.z / 2f;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmosVisibility != GizmosVisibility.AlwaysDraw)
                return;

            DrawDeformerGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (gizmosVisibility == GizmosVisibility.AlwaysHide)
                return;

            DrawDeformerGizmos();
        }

        protected abstract void DrawDeformerGizmos();
#endif
    }
}
