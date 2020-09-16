using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuObjectField : DuField
    {
        protected static readonly Color k_GizmosColorRangeZero = new Color(0.33f, 0.0f, 0.0f);
        protected static readonly Color k_GizmosColorRangeOne = new Color(0.80f, 0.0f, 0.0f);

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GizmosVisibility m_GizmoVisibility = GizmosVisibility.DrawOnSelect;
        public GizmosVisibility gizmoVisibility
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

        public override bool IsAllowGetFieldColor()
        {
            return remapping.remapColorEnabled;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            return GetFieldColorFromRemapping(remapping, powerByField);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility != GizmosVisibility.AlwaysDraw)
                return;

            DrawFieldGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (gizmoVisibility == GizmosVisibility.AlwaysHide)
                return;

            DrawFieldGizmos();
        }

        protected abstract void DrawFieldGizmos();

        protected Color GetGizmoColorRange0()
        {
            return gizmoFieldColor ? remapping.color * 0.33f : k_GizmosColorRangeZero;
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
