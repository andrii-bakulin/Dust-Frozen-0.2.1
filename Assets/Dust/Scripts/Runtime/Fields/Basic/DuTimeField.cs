using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Time Field")]
    public class DuTimeField : DuField
    {
        public enum TimeMode
        {
            Linear = 0,
            Sin = 1,
            Cos = 2,
            WaveUp = 3,
            WaveDown = 4,
            PingPong = 5,
            Ping = 6,
            Pong = 7,
            Square = 8,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private TimeMode m_TimeMode = TimeMode.Linear;
        public TimeMode timeMode
        {
            get => m_TimeMode;
            set => m_TimeMode = value;
        }

        [SerializeField]
        private float m_TimeScale = 1f;
        public float timeScale
        {
            get => m_TimeScale;
            set => m_TimeScale = value;
        }

        [SerializeField]
        private float m_Offset = 0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Basic Fields/Time")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuTimeField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, timeMode);
            DuDynamicState.Append(ref dynamicState, ++seq, timeScale);
            DuDynamicState.Append(ref dynamicState, ++seq, offset);

            DuDynamicState.Append(ref dynamicState, ++seq, remapping.GetDynamicStateHashCode());
            DuDynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Time";
        }

#if UNITY_EDITOR
        public override string FieldDynamicHint()
        {
            if (Mathf.Approximately(timeScale, 1f))
                return "";

            return "Scale " + timeScale.ToString("F2");
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Power

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            float globalOffset = m_OffsetDynamic + offset * timeScale;

            float power = GetPowerByTimeMode(timeMode, globalOffset);

            return remapping.MapValue(power);
        }

        public float GetPowerByTimeMode(TimeMode mode, float timeOffset)
        {
            switch (mode)
            {
                default:
                case TimeMode.Linear:
                    return timeOffset;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                case TimeMode.Sin:
                    return DuMath.Fit(-1f, +1f, 0f, 1f, Mathf.Sin(DuConstants.PI2 * timeOffset));

                case TimeMode.Cos:
                    return DuMath.Fit(-1f, +1f, 0f, 1f, Mathf.Cos(DuConstants.PI2 * timeOffset));

                case TimeMode.WaveUp:
                    return Mathf.Abs(Mathf.Sin(DuConstants.PI * timeOffset));

                case TimeMode.WaveDown:
                    return 1f - Mathf.Abs(Mathf.Sin(DuConstants.PI * timeOffset));

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                case TimeMode.PingPong:
                    return Mathf.PingPong(timeOffset * 2f, 1f);

                case TimeMode.Ping:
                    return Mathf.Repeat(timeOffset, 1f);

                case TimeMode.Pong:
                    return 1f - Mathf.Repeat(timeOffset, 1f);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                case TimeMode.Square:
                    return Mathf.Repeat(timeOffset, 1f) < 0.5f ? 0f : 1f;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public override bool IsAllowCalculateFieldColor()
        {
            return remapping.remapColorEnabled;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            return GetFieldColorFromRemapping(remapping, powerByField);
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return remapping.remapColorEnabled;
        }

        public override Gradient GetFieldColorPreview()
        {
            return GetFieldColorPreview(remapping);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * timeScale;
        }

        private void Reset()
        {
            remapping.color = DuColor.RandomColor();
            remapping.innerOffset = 0f;
            remapping.clampMinEnabled = false;
            remapping.clampMaxEnabled = false;
        }
    }
}
