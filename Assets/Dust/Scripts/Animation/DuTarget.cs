using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animation/Target")]
    [ExecuteInEditMode]
    public class DuTarget : DuMonoBehaviour
    {
        [SerializeField]
        private GameObject m_TargetObject = null;
        public GameObject targetObject
        {
            get => m_TargetObject;
            set => m_TargetObject = value;
        }

        [SerializeField]
        private GameObject m_UpVectorObject = null;
        public GameObject upVectorObject
        {
            get => m_UpVectorObject;
            set => m_UpVectorObject = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.LateUpdate;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Animation/Target")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Target", typeof(DuTarget));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        void UpdateState(float deltaTime)
        {
            if (Dust.IsNull(targetObject) || targetObject == this.gameObject)
                return;

            if (Dust.IsNotNull(upVectorObject) && upVectorObject != this.gameObject)
                this.transform.LookAt(targetObject.transform, upVectorObject.transform.position - transform.position);
            else
                this.transform.LookAt(targetObject.transform);
        }
    }
}
