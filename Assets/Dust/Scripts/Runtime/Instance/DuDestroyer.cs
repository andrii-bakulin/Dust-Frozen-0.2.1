using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Instance/Destroyer")]
    public class DuDestroyer : DuMonoBehaviour
    {
        public enum DestroyMode
        {
            Manual = 0,
            Time = 1,
            TimeRange = 2,
            AliveZone = 3,
            DeadZone = 4,
        };

        public enum VolumeCenterMode
        {
            StartPosition = 0,
            World = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DestroyMode m_DestroyMode = DestroyMode.Time;
        public DestroyMode destroyMode
        {
            get => m_DestroyMode;
            set => m_DestroyMode = value;
        }

        [SerializeField]
        private float m_Timeout = 1f;
        public float timeout
        {
            get => m_Timeout;
            set => m_Timeout = value;
        }

        [SerializeField]
        private DuRange m_TimeoutRange = DuRange.zeroToOne;
        public DuRange timeoutRange
        {
            get => m_TimeoutRange;
            set => m_TimeoutRange = value;
        }

        [SerializeField]
        private VolumeCenterMode m_VolumeCenterMode = VolumeCenterMode.StartPosition;
        public VolumeCenterMode volumeCenterMode
        {
            get => m_VolumeCenterMode;
            set => m_VolumeCenterMode = value;
        }

        [SerializeField]
        private Vector3 m_VolumeCenter = Vector3.zero;
        public Vector3 volumeCenter
        {
            get => m_VolumeCenter;
            set => m_VolumeCenter = value;
        }

        [SerializeField]
        private Vector3 m_VolumeSize = Vector3.one;
        public Vector3 volumeSize
        {
            get => m_VolumeSize;
            set => m_VolumeSize = Normalizer.VolumeSize(value);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            switch (destroyMode)
            {
                default:
                case DestroyMode.Manual:
                case DestroyMode.Time:
                    // Nothing need to do...
                    break;

                case DestroyMode.TimeRange:
                    m_Timeout = Random.Range(timeoutRange.min, timeoutRange.max);
                    break;

                case DestroyMode.AliveZone:
                case DestroyMode.DeadZone:
                    if (volumeCenterMode == VolumeCenterMode.StartPosition)
                        volumeCenter = this.transform.position;
                    break;
            }
        }

        private void Update()
        {
            switch (destroyMode)
            {
                default:
                case DestroyMode.Manual:
                    // Nothing need to do...
                    break;

                case DestroyMode.Time:
                case DestroyMode.TimeRange:
                    m_Timeout -= Time.deltaTime;

                    if (m_Timeout <= 0f)
                        DestroyNow();
                    break;

                case DestroyMode.AliveZone:
                    if (!IsInsideVolume())
                        DestroyNow();
                    break;

                case DestroyMode.DeadZone:
                    if (IsInsideVolume())
                        DestroyNow();
                    break;
            }
        }

        protected bool IsInsideVolume()
        {
            Vector3 pos = transform.position;
            Vector3 halfSize = volumeSize / 2f;

            if (volumeCenter.x - halfSize.x > pos.x) return false;
            if (volumeCenter.x + halfSize.x < pos.x) return false;

            if (volumeCenter.y - halfSize.y > pos.y) return false;
            if (volumeCenter.y + halfSize.y < pos.y) return false;

            if (volumeCenter.z - halfSize.z > pos.z) return false;
            if (volumeCenter.z + halfSize.z < pos.z) return false;

            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            switch (destroyMode)
            {
                default:
                case DestroyMode.Manual:
                    break;

                case DestroyMode.Time:
                case DestroyMode.TimeRange:
                    return;

                case DestroyMode.AliveZone:
                    Gizmos.color = Color.green;
                    break;

                case DestroyMode.DeadZone:
                    Gizmos.color = Color.red;
                    break;
            }

            switch (volumeCenterMode)
            {
                case VolumeCenterMode.StartPosition:
                    Gizmos.DrawWireCube(Application.isPlaying ? volumeCenter : transform.position, volumeSize);
                    break;

                case VolumeCenterMode.World:
                    Gizmos.DrawWireCube(volumeCenter, volumeSize);
                    break;
            }
        }
#endif

        public void DestroyNow()
        {
            this.enabled = false;
            Destroy(this.gameObject);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static Vector3 VolumeSize(Vector3 value)
            {
                return DuVector3.Abs(value);
            }
        }
    }
}
