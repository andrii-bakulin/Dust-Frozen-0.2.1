using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Transform Random Action")]
    public class DuTransformRandomAction : DuInstantAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        public enum TransformMode
        {
            Add = 0,
            Set = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_PositionEnabled = false;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        private Vector3 m_PositionRangeMin = DuVector3.New(-1f);
        public Vector3 positionRangeMin
        {
            get => m_PositionRangeMin;
            set => m_PositionRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_PositionRangeMax = DuVector3.New(+1f);
        public Vector3 positionRangeMax
        {
            get => m_PositionRangeMax;
            set => m_PositionRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RotationEnabled = false;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set => m_RotationEnabled = value;
        }

        [SerializeField]
        private Vector3 m_RotationRangeMin = Vector3.up * -180f;
        public Vector3 rotationRangeMin
        {
            get => m_RotationRangeMin;
            set => m_RotationRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_RotationRangeMax = Vector3.up * +180f;
        public Vector3 rotationRangeMax
        {
            get => m_RotationRangeMax;
            set => m_RotationRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ScaleEnabled = false;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set => m_ScaleEnabled = value;
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMin = DuVector3.New(-0.5f);
        public Vector3 scaleRangeMin
        {
            get => m_ScaleRangeMin;
            set => m_ScaleRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMax = DuVector3.New(+1.0f);
        public Vector3 scaleRangeMax
        {
            get => m_ScaleRangeMax;
            set => m_ScaleRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private TransformMode m_TransformMode = TransformMode.Set;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ?? (m_DuRandom = new DuRandom(seed));

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            if (positionEnabled)
            {
                Vector3 value = duRandom.Range(positionRangeMin, positionRangeMax);
                Vector3 position = Vector3.zero;

                if (space == Space.World)
                    position = tr.position;
                else if (space == Space.Local)
                    position = tr.localPosition;

                if (transformMode == TransformMode.Add)
                    position += value;
                else if (transformMode == TransformMode.Set)
                    position = value;

                if (space == Space.World)
                    tr.position = position;
                else if (space == Space.Local)
                    tr.localPosition = position;
            }

            if (rotationEnabled)
            {
                Vector3 value = duRandom.Range(rotationRangeMin, rotationRangeMax);
                Vector3 rotation = Vector3.zero;

                if (space == Space.World)
                    rotation = tr.eulerAngles;
                else if (space == Space.Local)
                    rotation = tr.localEulerAngles;

                if (transformMode == TransformMode.Add)
                    rotation += value;
                else if (transformMode == TransformMode.Set)
                    rotation = value;

                if (space == Space.World)
                    tr.eulerAngles = rotation;
                else if (space == Space.Local)
                    tr.localEulerAngles = rotation;
            }

            if (scaleEnabled)
            {
                Vector3 value = duRandom.Range(scaleRangeMin, scaleRangeMax);
                Vector3 scale = Vector3.one;

                if (space == Space.World)
                    scale = tr.lossyScale;
                else if (space == Space.Local)
                    scale = tr.localScale;

                if (transformMode == TransformMode.Add)
                    scale += value;
                else if (transformMode == TransformMode.Set)
                    scale = value;

                if (space == Space.World)
                    DuTransform.SetGlobalScale(tr, scale);
                else if (space == Space.Local)
                    tr.localScale = scale;
            }
        }
    }
}
