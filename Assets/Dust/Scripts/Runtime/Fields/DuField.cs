using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuField : DuMonoBehaviour, DuDynamicStateInterface
    {
        public static readonly Color k_GizmosColorRangeZero = new Color(0.00f, 0.50f, 0.66f);
        public static readonly Color k_GizmosColorRangeOne = new Color(0.00f, 0.75f, 1.00f);

        //--------------------------------------------------------------------------------------------------------------

        public class Point
        {
            // In
            public Vector3 inPosition; // point in world position
            public float inOffset; // offset for point in sequence of points [0..1]

            // Out
            public float outPower; // power calculated by fieldsMap
            public Color outColor; // color calculated by fieldsMap
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public static void AddFieldComponentByType(System.Type duFieldType)
        {
            Selection.activeGameObject = AddFieldComponentByType(Selection.activeGameObject, duFieldType);
        }

        public static GameObject AddFieldComponentByType(GameObject activeGameObject, System.Type duFieldType)
        {
            DuDeformer selectedDeformer = null;
            DuFieldsSpace selectedFieldsSpace = null;
            DuFactoryExtendedMachine selectedFactoryMachine = null;

            if (Dust.IsNotNull(activeGameObject))
            {
                selectedDeformer = activeGameObject.GetComponent<DuDeformer>();

                if (Dust.IsNull(selectedDeformer) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedDeformer = activeGameObject.transform.parent.GetComponent<DuDeformer>();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                selectedFieldsSpace = activeGameObject.GetComponent<DuFieldsSpace>();

                if (Dust.IsNull(selectedFieldsSpace) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFieldsSpace = activeGameObject.transform.parent.GetComponent<DuFieldsSpace>();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                selectedFactoryMachine = activeGameObject.GetComponent<DuFactoryExtendedMachine>();

                if (Dust.IsNull(selectedFactoryMachine) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFactoryMachine = activeGameObject.transform.parent.GetComponent<DuFactoryExtendedMachine>();
            }

            var gameObject = new GameObject();
            {
                DuField field = gameObject.AddComponent(duFieldType) as DuField;

                if (Dust.IsNotNull(selectedDeformer))
                {
                    field.transform.parent = selectedDeformer.transform;
                    selectedDeformer.fieldsMap.AddField(field);
                }
                else if (Dust.IsNotNull(selectedFieldsSpace))
                {
                    field.transform.parent = selectedFieldsSpace.transform;
                    selectedFieldsSpace.fieldsMap.AddField(field);
                }
                else if (Dust.IsNotNull(selectedFactoryMachine))
                {
                    field.transform.parent = selectedFactoryMachine.transform;
                    selectedFactoryMachine.fieldsMap.AddField(field);
                }

                gameObject.name = field.FieldName() + " Field";
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            // Require to show enabled-checkbox in editor for all fields
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public abstract string FieldName();

#if UNITY_EDITOR
        public abstract string FieldDynamicHint();
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Power

        public abstract float GetPowerForFieldPoint(DuField.Point fieldPoint);

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public abstract bool IsAllowCalculateFieldColor();

        /// <returns>Color.alpha used as power of color</returns>
        public abstract Color GetFieldColor(DuField.Point fieldPoint, float powerByField);

#if UNITY_EDITOR
        public abstract bool IsHasFieldColorPreview();
        public abstract Gradient GetFieldColorPreview(out float intensity);
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public abstract int GetDynamicStateHashCode();

        //--------------------------------------------------------------------------------------------------------------

        // How it works
        // 1) scale alpha by powerByField
        // 2) if alpha greater then 1f, then set alpha to 1f, but scale RGB for same value
        // 3) Clamp 0..1
        // Examples:       RGBA(0.1f, 0.2f, 0.4f, 0.50f);
        // Power 0.5 =>    RGBA(0.1f, 0.2f, 0.4f, 0.25f);   => downgrade alpha 0.5f to 0.25f
        // Power 1.0 =>    RGBA(0.1f, 0.2f, 0.4f, 0.50f);   => Nothing change
        // Power 2.0 =>    RGBA(0.1f, 0.2f, 0.4f, 1.00f);   => multiply alpha 2x
        // Power 4.0 =>    RGBA(0.1f, 0.2f, 0.4f, 2.00f);   => RGBA(0.2f, 0.4f, 0.8f, 1.00f);
        // Power 8.0 =>    RGBA(0.1f, 0.2f, 0.4f, 4.00f);   => RGBA(0.4f, 0.8f, 1.0f, 1.00f);
        protected Color GetFieldColorByPower(Color color, float powerByField)
        {
            color.a *= powerByField;

            if (color.a > 1f)
            {
                color.r *= color.a;
                color.g *= color.a;
                color.b *= color.a;
                color.a = 1f;
            }

            color.Clamp01();
            return color;
        }

        protected Color GetFieldColorFromRemapping(DuRemapping remapping, float powerByField)
        {
            switch (remapping.colorMode)
            {
                case DuRemapping.ColorMode.Color:
                    return GetFieldColorByPower(remapping.color, powerByField);

                case DuRemapping.ColorMode.Gradient:
                    return remapping.gradient.Evaluate(powerByField);

                default:
                    return Color.magenta;
            }
        }

        protected Gradient GetFieldColorPreview(DuRemapping remapping, out float intensity)
        {
            switch (remapping.colorMode)
            {
                case DuRemapping.ColorMode.Color:
                    intensity = remapping.color.a;
                    return remapping.color.ToGradient();

                case DuRemapping.ColorMode.Gradient:
                    intensity = 1f;
                    return remapping.gradient;

                default:
                    intensity = 1f;
                    return Color.magenta.ToGradient();
            }
        }
    }
}
