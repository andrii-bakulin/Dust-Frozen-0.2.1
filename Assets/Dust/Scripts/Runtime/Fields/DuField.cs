using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuField : DuMonoBehaviour
    {
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

        private float m_TimeSinceStart;
        public float timeSinceStart => m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public static void AddFieldComponentByType(System.Type duFieldType)
        {
            Selection.activeGameObject = AddFieldComponentByType(Selection.activeGameObject, duFieldType);
        }

        public static GameObject AddFieldComponentByType(GameObject activeGameObject, System.Type duFieldType)
        {
            DuFieldsSpace selectedFieldsSpace = null;

            if (Dust.IsNotNull(activeGameObject))
            {
                selectedFieldsSpace = activeGameObject.GetComponent<DuFieldsSpace>();

                if (Dust.IsNull(selectedFieldsSpace) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFieldsSpace = activeGameObject.transform.parent.GetComponent<DuFieldsSpace>();
            }

            var gameObject = new GameObject();
            {
                DuField field = gameObject.AddComponent(duFieldType) as DuField;

                if (Dust.IsNotNull(selectedFieldsSpace))
                {
                    field.transform.parent = selectedFieldsSpace.transform;
                    selectedFieldsSpace.fieldsMap.AddField(field);
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

        public abstract string FieldName();

        public abstract float GetPowerForFieldPoint(DuField.Point fieldPoint);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public abstract bool IsAllowGetFieldColor();

        /// <returns>Color.alpha used as power of color</returns>
        public abstract Color GetFieldColor(DuField.Point fieldPoint, float powerByField);

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

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            m_TimeSinceStart += Time.deltaTime;
        }
    }
}
