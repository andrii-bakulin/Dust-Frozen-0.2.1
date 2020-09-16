using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animation/Follow")]
    [ExecuteInEditMode]
    public class DuFollow : DuMonoBehaviour
    {
        [SerializeField]
        private GameObject m_FollowObject = null;
        public GameObject followObject
        {
            get => m_FollowObject;
            set => m_FollowObject = value;
        }

        [SerializeField]
        private Vector3 m_FollowDistance = Vector3.zero;
        public Vector3 followDistance
        {
            get => m_FollowDistance;
            set => m_FollowDistance = value;
        }

        [SerializeField]
        private bool m_UseSmoothDamp = false;
        public bool useSmoothDamp
        {
            get => m_UseSmoothDamp;
            set => m_UseSmoothDamp = value;
        }

        [SerializeField]
        private Vector3 m_SmoothTime = Vector3.one;
        public Vector3 smoothTime
        {
            get => m_SmoothTime;
            set => m_SmoothTime = Normalizer.SmoothTime(value);
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.LateUpdate;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Vector3 m_SmoothVelocity;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Animation/Follow")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Follow", typeof(DuFollow));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
#endif
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.update -= EditorUpdate;
            }
#endif
        }

        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
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

#if UNITY_EDITOR
        void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            UpdateState(deltaTime);
        }
#endif

        void UpdateState(float deltaTime)
        {
            if (Dust.IsNull(followObject) || followObject == this.gameObject)
                return;

            Vector3 newPosition = followObject.transform.position + followDistance;

            if (useSmoothDamp)
                newPosition = DuVector3.SmoothDamp(transform.position, newPosition, ref m_SmoothVelocity, smoothTime, Mathf.Infinity, deltaTime);

            transform.position = newPosition;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static Vector3 SmoothTime(Vector3 value)
            {
                return Vector3.Max(DuVector3.New(0.01f), value);
            }
        }
    }
}
