﻿using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animation/LookAt")]
    [ExecuteInEditMode]
    public class DuLookAt : DuMonoBehaviour
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
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        [SerializeField]
        private bool m_UpdateInEditor = true;
        public bool updateInEditor
        {
            get => m_UpdateInEditor;
            set => m_UpdateInEditor = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
#if UNITY_EDITOR
            if (isEditorUpdatesEnabled && !updateInEditor) return;
#endif

            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
#if UNITY_EDITOR
            if (isEditorUpdatesEnabled && !updateInEditor) return;
#endif

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
