using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/RotateBy Action")]
    public class DuRotateByAction : DuIntervalWithRollbackAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_RotateBy = Vector3.zero;
        public Vector3 rotateBy
        {
            get => m_RotateBy;
            set => m_RotateBy = value;
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
        private bool m_ImproveAccuracy = false;
        public bool improveAccuracy
        {
            get => m_ImproveAccuracy;
            set => m_ImproveAccuracy = value;
        }

        [SerializeField]
        private float m_ImproveAccuracyThreshold = 0.5f;
        public float improveAccuracyThreshold
        {
            get => m_ImproveAccuracyThreshold;
            set => m_ImproveAccuracyThreshold = Normalizer.ImproveAccuracyThreshold(value);
        }

        [SerializeField]
        private int m_ImproveAccuracyMaxIterations = 16;
        public int improveAccuracyMaxIterations
        {
            get => m_ImproveAccuracyMaxIterations;
            set => m_ImproveAccuracyMaxIterations = Normalizer.ImproveAccuracyMaxIterations(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            float signRotate = playingPhase == PlayingPhase.Main ? +1f : -1f;

            Vector3 deltaRotate = rotateBy * (signRotate * (playbackStateInPhase - previousStateInPhase));

            if (deltaRotate.Equals(Vector3.zero))
                return;
            
            if (space == Space.Local && Dust.IsNotNull(m_TargetTransform.parent))
                deltaRotate = m_TargetTransform.parent.TransformDirection(deltaRotate);
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int iterationsCount = 1;

            if (improveAccuracy)
            {
                iterationsCount = Mathf.CeilToInt(deltaRotate.magnitude / improveAccuracyThreshold);
                iterationsCount = Mathf.Min(iterationsCount, improveAccuracyMaxIterations); 
                deltaRotate /= iterationsCount;
            }

            Quaternion quaternion = Quaternion.Euler(deltaRotate);

            for (int i = 0; i < iterationsCount; i++)
            {
                if (space == Space.World || space == Space.Local)
                {
                    // m_TargetTransform.Rotate(deltaRotate, UnityEngine.Space.World);
                    m_TargetTransform.rotation *= Quaternion.Inverse(m_TargetTransform.rotation) * quaternion * m_TargetTransform.rotation;
                }
                else if (space == Space.Self)
                {
                    // m_TargetTransform.Rotate(deltaRotate, UnityEngine.Space.Self);
                    m_TargetTransform.localRotation *= quaternion;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public new static class Normalizer
        {
            public static float ImproveAccuracyThreshold(float value)
            {
                return Mathf.Max(value, 0.01f);
            }

            public static int ImproveAccuracyMaxIterations(int value)
            {
                return Mathf.Clamp(value, 1, 1000);
            }
        }
    }
}
