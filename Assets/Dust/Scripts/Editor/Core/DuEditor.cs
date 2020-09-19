using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public class DuEditor : Editor
    {
        public class DuProperty
        {
            public string propertyPath;
            public string title;
            public string tooltip;
            public SerializedProperty property;
            public bool isChanged;

            public Editor parentEditor;

            //--------------------------------------------------------------------------------------------------------------
            // Helpers

            public SerializedProperty FindProperty(string relativePropertyPath)
            {
                return property.FindPropertyRelative(relativePropertyPath);
            }

            public bool IsTrue => property.propertyType == SerializedPropertyType.Boolean ? property.boolValue : false;

            public int enumValueIndex => property?.enumValueIndex ?? 0;

            public int valInt
            {
                get => property.intValue;
                set => property.intValue = value;
            }

            public float valFloat
            {
                get => property.floatValue;
                set => property.floatValue = value;
            }

            public Vector3 valVector3
            {
                get => property.vector3Value;
                set => property.vector3Value = value;
            }

            public Vector3Int valVector3Int
            {
                get => property.vector3IntValue;
                set => property.vector3IntValue = value;
            }

            public AnimationCurve valAnimationCurve
            {
                get => property.animationCurveValue;
                set => property.animationCurveValue = value;
            }

            public Color valColor
            {
                get => property.colorValue;
                set => property.colorValue = value;
            }

            public SerializedProperty valUnityEvent => property.FindPropertyRelative("m_PersistentCalls.m_Calls");

            public GameObject GameObjectReference => property.objectReferenceValue as GameObject;

            public bool ObjectReferenceExists => Dust.IsNotNull(property.objectReferenceValue);
        }

        //--------------------------------------------------------------------------------------------------------------

        public DuProperty FindProperty(string propertyPath, string title = "", string tooltip = "")
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = serializedObject.FindProperty(propertyPath),
                isChanged = false,
                parentEditor = this
            };
            return duProperty;
        }

        public static DuProperty FindProperty(SerializedProperty parentProperty, string propertyPath, string title = "", string tooltip = "")
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = parentProperty.FindPropertyRelative(propertyPath),
                isChanged = false,
                parentEditor = null
            };
            return duProperty;
        }

        //--------------------------------------------------------------------------------------------------------------

        public class SerializedEntity
        {
            public Object target;
            public SerializedObject serializedObject;
        }

        // If I change some parameters for list of targets then I need to create SerializedObject for each target.
        // BUT only for self-target I need to use self-serializedObject object!
        public SerializedEntity[] GetSerializedEntitiesByTargets()
        {
            return GetSerializedEntitiesByTargets(this);
        }

        public static SerializedEntity[] GetSerializedEntitiesByTargets(DuEditor targetEditor)
        {
            var serializedTargets = new SerializedEntity[targetEditor.targets.Length];

            for (int i = 0; i < targetEditor.targets.Length; i++)
            {
                serializedTargets[i] = new SerializedEntity
                {
                    target = targetEditor.targets[i],
                    serializedObject = targetEditor.targets[i] == targetEditor.target
                        ? targetEditor.serializedObject :
                        new SerializedObject(targetEditor.targets[i])
                };
            }

            return serializedTargets;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyField(DuProperty duProperty, string label, string tooltip = "")
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            DustGUI.Field(new GUIContent(label, tooltip), duProperty.property);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked, string label, string tooltip = "")
        {
            if (isLocked) DustGUI.Lock();
            PropertyField(duProperty, label, tooltip);
            if (isLocked) DustGUI.Unlock();
            return duProperty.isChanged;
        }

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden, string label, string tooltip = "")
        {
            if (isHidden)
                return false;

            return PropertyField(duProperty, label, tooltip);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyField(DuProperty duProperty)
        {
            return PropertyField(duProperty, duProperty.title, duProperty.tooltip);
        }

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked)
        {
            return PropertyFieldOrLock(duProperty, isLocked, duProperty.title, duProperty.tooltip);
        }

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden)
        {
            return PropertyFieldOrHide(duProperty, isHidden, duProperty.title, duProperty.tooltip);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyFieldRange(DuProperty duProperty)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            duProperty.isChanged |= PropertyField(duProperty.FindProperty("min"), "Range Min");
            duProperty.isChanged |= PropertyField(duProperty.FindProperty("max"), "Range Max");
            return duProperty.isChanged;
        }

        public static bool PropertySeedRandomOrFixed(DuProperty duProperty, int defValue = DuConstants.RANDOM_SEED_DEFAULT)
        {
            bool isChanged = false;

            int seed = duProperty.valInt;

            bool curUseSeed = seed > 0;
            bool newUseSeed = DustGUI.Field("Use Fixed Seed", curUseSeed);

            if (curUseSeed != newUseSeed)
            {
                if (newUseSeed)
                    duProperty.valInt = duProperty.valInt == 0 ? defValue : -duProperty.valInt;
                else
                    duProperty.valInt = -duProperty.valInt;

                isChanged = true;
            }

            if (newUseSeed)
                isChanged = PropertySeedFixed(duProperty);

            return isChanged;
        }

        public static bool PropertySeedFixed(DuProperty duProperty, int seedMin = DuConstants.RANDOM_SEED_MIN, int seedMax = DuConstants.RANDOM_SEED_MAX)
        {
            EditorGUI.BeginChangeCheck();
            DustGUI.ExtraIntSlider.Create(seedMin, seedMax, 1, 1, int.MaxValue).Draw("Seed", duProperty.property);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertySlider(DuProperty duProperty, float leftValue, float rightValue)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Slider(duProperty.property, leftValue, rightValue, new GUIContent(duProperty.title, duProperty.tooltip));
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue, int leftLimit = int.MinValue, int rightLimit = int.MaxValue)
        {
            var slider = new DustGUI.ExtraIntSlider(leftValue, rightValue, stepValue, leftLimit, rightLimit);
            slider.LinkEditor(duProperty.parentEditor);
            duProperty.isChanged = slider.Draw(new GUIContent(duProperty.title, duProperty.tooltip), duProperty.property);
            return duProperty.isChanged;
        }

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue, float leftLimit = float.MinValue, float rightLimit = float.MaxValue)
        {
            var slider = new DustGUI.ExtraSlider(leftValue, rightValue, stepValue, leftLimit, rightLimit);
            slider.LinkEditor(duProperty.parentEditor);
            duProperty.isChanged = slider.Draw(new GUIContent(duProperty.title, duProperty.tooltip), duProperty.property);
            return duProperty.isChanged;
        }

        public static bool PropertyExtendedSlider01(DuProperty duProperty)
        {
            return PropertyExtendedSlider(duProperty, 0f, 1f, 0.01f, 0f, 1f);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyFieldCurve(DuProperty duProperty, int height = 100)
        {
            return PropertyFieldCurve(duProperty, duProperty.title);
        }

        public static bool PropertyFieldCurve(DuProperty duProperty, string label, int height = 100)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            DustGUI.Field(label, duProperty.property, 0, 90);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyField(SerializedProperty property, string label, string tooltip = "")
        {
            EditorGUI.BeginChangeCheck();
            DustGUI.Field(new GUIContent(label, tooltip), property);
            return EditorGUI.EndChangeCheck();
        }

        public static bool PropertyFieldOrLock(SerializedProperty property, bool isLocked, string label, string tooltip = "")
        {
            if (isLocked) DustGUI.Lock();

            bool isChanged = PropertyField(property, label, tooltip);

            if (isLocked) DustGUI.Unlock();

            return isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static int Popup(string label, int selectedIndex, string[] displayedOptions)
        {
            return EditorGUILayout.Popup(label, selectedIndex, displayedOptions);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Space()
        {
            DustGUI.SpaceLine(0.5f);
        }
    }
}
#endif
