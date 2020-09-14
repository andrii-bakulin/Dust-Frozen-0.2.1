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
        private GizmosVisibility m_GizmosVisibility = GizmosVisibility.DrawOnSelect;
        public GizmosVisibility gizmosVisibility
        {
            get => m_GizmosVisibility;
            set => m_GizmosVisibility = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowGetFieldColor()
        {
            return remapping.colorRemap != DuRemapping.ColorRemap.NoRemap;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float weightByField)
        {
            return GetFieldColorByWeight(remapping.color, weightByField);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmosVisibility != GizmosVisibility.AlwaysDraw)
                return;

            DrawFieldGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (gizmosVisibility == GizmosVisibility.AlwaysHide)
                return;

            DrawFieldGizmos();
        }

        protected abstract void DrawFieldGizmos();
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
