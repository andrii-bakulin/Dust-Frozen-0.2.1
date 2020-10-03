using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuObjectField : DuField
    {
        public static readonly Color k_GizmosColorRangeZero = new Color(0.00f, 0.50f, 0.66f);
        public static readonly Color k_GizmosColorRangeOne = new Color(0.00f, 0.75f, 1.00f);

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GizmoVisibility m_GizmoVisibility = GizmoVisibility.DrawOnSelect;
        public GizmoVisibility gizmoVisibility
        {
            get => m_GizmoVisibility;
            set => m_GizmoVisibility = value;
        }

        [SerializeField]
        private bool m_GizmoFieldColor = true;
        public bool gizmoFieldColor
        {
            get => m_GizmoFieldColor;
            set => m_GizmoFieldColor = value;
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

        public override Gradient GetFieldColorPreview(out float intensity)
        {
            return GetFieldColorPreview(remapping, out intensity);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, transform);
            DuDynamicState.Append(ref dynamicState, ++seq, remapping.GetDynamicStateHashCode());

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility != GizmoVisibility.AlwaysDraw)
                return;

            DrawFieldGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (gizmoVisibility == GizmoVisibility.AlwaysHide)
                return;

            DrawFieldGizmos();
        }

        protected abstract void DrawFieldGizmos();

        protected Color GetGizmoColorRange0()
        {
            return gizmoFieldColor ? remapping.color * 0.66f : k_GizmosColorRangeZero;
        }

        protected Color GetGizmoColorRange1()
        {
            return gizmoFieldColor ? remapping.color : k_GizmosColorRangeOne;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            ResetDefaults();
        }

        protected void ResetDefaults()
        {
            remapping.color = DuColor.RandomColor();
        }
    }
}
