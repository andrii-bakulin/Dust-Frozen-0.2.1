using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuRemappingEditor
    {
        private DuRemapping m_Remapping;
        private Material m_DrawerMaterial;

        protected DuEditor.DuProperty m_RemapForceEnabled;
        protected DuEditor.DuProperty m_Strength;
        protected DuEditor.DuProperty m_InnerOffset;
        protected DuEditor.DuProperty m_Invert;

        protected DuEditor.DuProperty m_Min;
        protected DuEditor.DuProperty m_Max;
        protected DuEditor.DuProperty m_ClampMinEnabled;
        protected DuEditor.DuProperty m_ClampMaxEnabled;

        protected DuEditor.DuProperty m_ContourMultiplier;
        protected DuEditor.DuProperty m_ContourMode;
        protected DuEditor.DuProperty m_ContourSteps;
        protected DuEditor.DuProperty m_ContourSpline;
        protected DuEditor.DuProperty m_ContourSplineAnimationSpeed;
        protected DuEditor.DuProperty m_ContourSplineOffset;

        protected DuEditor.DuProperty m_RemapColorEnabled;
        protected DuEditor.DuProperty m_ColorMode;
        protected DuEditor.DuProperty m_Color;
        protected DuEditor.DuProperty m_Gradient;

        public DuRemappingEditor(DuRemapping duRemapping, SerializedProperty remappingProperty)
        {
            m_Remapping = duRemapping;
            m_DrawerMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));

            m_RemapForceEnabled = DuEditor.FindProperty(remappingProperty, "m_RemapForceEnabled", "Enabled");
            m_Strength = DuEditor.FindProperty(remappingProperty, "m_Strength", "Strength");
            m_InnerOffset = DuEditor.FindProperty(remappingProperty, "m_InnerOffset", "Inner Offset");
            m_Invert = DuEditor.FindProperty(remappingProperty, "m_Invert", "Invert");

            m_Min = DuEditor.FindProperty(remappingProperty, "m_Min", "Min");
            m_Max = DuEditor.FindProperty(remappingProperty, "m_Max", "Max");
            m_ClampMinEnabled = DuEditor.FindProperty(remappingProperty, "m_ClampMinEnabled", "Clamp Min");
            m_ClampMaxEnabled = DuEditor.FindProperty(remappingProperty, "m_ClampMaxEnabled", "Clamp Max");

            m_ContourMultiplier = DuEditor.FindProperty(remappingProperty, "m_ContourMultiplier", "Contour Multiplier");
            m_ContourMode = DuEditor.FindProperty(remappingProperty, "m_ContourMode", "Contour Mode");
            m_ContourSteps = DuEditor.FindProperty(remappingProperty, "m_ContourSteps", "Steps");

            m_ContourSpline = DuEditor.FindProperty(remappingProperty, "m_ContourSpline", "Spline");
            m_ContourSplineAnimationSpeed = DuEditor.FindProperty(remappingProperty, "m_ContourSplineAnimationSpeed", "Spline Animation Speed");
            m_ContourSplineOffset = DuEditor.FindProperty(remappingProperty, "m_ContourSplineOffset", "Spline Offset");

            m_RemapColorEnabled = DuEditor.FindProperty(remappingProperty, "m_RemapColorEnabled", "Enabled");
            m_ColorMode = DuEditor.FindProperty(remappingProperty, "m_ColorMode", "Mode");
            m_Color = DuEditor.FindProperty(remappingProperty, "m_Color", "Color");
            m_Gradient = DuEditor.FindProperty(remappingProperty, "m_Gradient", "Gradient");
        }

        public void OnInspectorGUI(bool showGraphMirrored)
        {
            if (DustGUI.FoldoutBegin("Force", "DuRemapping.Force"))
            {
                DuEditor.PropertyField(m_RemapForceEnabled);

                PropertyMappingGraph(m_Remapping, m_Color.valColor, showGraphMirrored);

                if (m_RemapForceEnabled.IsTrue)
                {
                    DuEditor.PropertyExtendedSlider(m_Strength, 0f, 1f, 0.01f);
                    DuEditor.PropertyExtendedSlider01(m_InnerOffset);
                    DuEditor.PropertyField(m_Invert);
                    DuEditor.Space();

                    DuEditor.PropertyExtendedSlider(m_Min, 0f, 1f, 0.01f);
                    DuEditor.PropertyExtendedSlider(m_Max, 0f, 1f, 0.01f);
                    DuEditor.Space();

                    DustGUI.Header("Clamping");
                    DuEditor.PropertyField(m_ClampMinEnabled);
                    DuEditor.PropertyField(m_ClampMaxEnabled);
                    DuEditor.Space();

                    DustGUI.Header("Contour");
                    DuEditor.PropertyExtendedSlider(m_ContourMultiplier, 0f, 1f, 0.01f);
                    DuEditor.PropertyField(m_ContourMode);

                    switch ((DuRemapping.ContourMode) m_ContourMode.enumValueIndex)
                    {
                        case DuRemapping.ContourMode.None:
                            break;

                        case DuRemapping.ContourMode.Curve:
                            DuEditor.PropertyFieldCurve(m_ContourSpline);
                            DuEditor.PropertyExtendedSlider(m_ContourSplineAnimationSpeed, 0f, 1f, 0.01f);
                            DuEditor.PropertyExtendedSlider(m_ContourSplineOffset, 0f, 1f, 0.01f);
                            break;

                        case DuRemapping.ContourMode.Step:
                            DuEditor.PropertyExtendedIntSlider(m_ContourSteps, 1, 25, 1, 1);
                            break;
                    }
                }

                DuEditor.Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Color", "DuRemapping.Color"))
            {
                DuEditor.PropertyField(m_RemapColorEnabled);

                if (m_RemapColorEnabled.IsTrue)
                {
                    DuEditor.PropertyField(m_ColorMode);

                    switch ((DuRemapping.ColorMode) m_ColorMode.enumValueIndex)
                    {
                        case DuRemapping.ColorMode.Color:
                            DuEditor.PropertyField(m_Color);
                            break;

                        case DuRemapping.ColorMode.Gradient:
                            DuEditor.PropertyField(m_Gradient);
                            break;
                    }
                }

                DuEditor.Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_InnerOffset.isChanged)
                m_InnerOffset.valFloat = DuRemapping.ObjectNormalizer.InnerOffset(m_InnerOffset.valFloat);

            if (m_ContourSteps.isChanged)
                m_ContourSteps.valInt = DuRemapping.ObjectNormalizer.ContourSteps(m_ContourSteps.valInt);

            if (m_ContourSpline.isChanged)
                m_ContourSpline.valAnimationCurve = DuRemapping.ObjectNormalizer.ContourSpline(m_ContourSpline.valAnimationCurve);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void PropertyMappingGraph(DuRemapping duRemapping, Color color, bool showGraphMirrored)
        {
            // Begin to draw a horizontal layout, using the helpBox EditorStyle
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            Color bgColor = new Color(0.15f, 0.15f, 0.15f);
            float kHeight = 100;

            // Reserve GUI space with a width from 10 to 10000, and a fixed height of 200, and
            // cache it as a rectangle.
            Rect layoutRectangle = GUILayoutUtility.GetRect(10, 10000, kHeight, kHeight);

            float kWidth = layoutRectangle.width;

            if (Event.current.type == EventType.Repaint)
            {
                // If we are currently in the Repaint event, begin to draw a clip of the size of
                // previously reserved rectangle, and push the current matrix for drawing.
                GUI.BeginClip(layoutRectangle);
                GL.PushMatrix();

                // Clear the current render buffer, setting a new background colour, and set our
                // material for rendering.
                GL.Clear(true, false, bgColor);
                m_DrawerMaterial.SetPass(0);

                // Start drawing in OpenGL Quads, to draw the background canvas. Set the
                // colour black as the current OpenGL drawing colour, and draw a quad covering
                // the dimensions of the layoutRectangle.
                GL.Begin(GL.QUADS);
                GL.Color(bgColor);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(layoutRectangle.width, 0, 0);
                GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
                GL.Vertex3(0, layoutRectangle.height, 0);
                GL.End();

                // Start drawing in OpenGL Lines, to draw the lines of the grid.
                GL.Begin(GL.LINES);
                GL.Color(color);

                float rangeMin = 0f;
                float rangeMax = 1f;

                for (int i = 0; i <= kWidth; i++)
                {
                    float offset;

                    if (showGraphMirrored)
                    {
                        offset = Mathf.LerpUnclamped(rangeMin, rangeMax, i / kWidth * 2f);
                        offset = Mathf.PingPong(offset, 1f);
                    }
                    else
                    {
                        offset = Mathf.LerpUnclamped(rangeMin, rangeMax, i / kWidth);
                    }

                    float value = duRemapping.MapValue(offset);

                    if (value < 0f)
                        continue;

                    if (value > 1f)
                        value = 1f;

                    float yFrom = kHeight * (1 - value);
                    float yTo = kHeight;

                    GL.Vertex3(i, yFrom, 0);
                    GL.Vertex3(i, yTo, 0);

                    if (EditorGUIUtility.pixelsPerPoint > 1.0f)
                    {
                        // This require for retina display
                        GL.Vertex3(i + 0.5f, yFrom, 0);
                        GL.Vertex3(i + 0.5f, yTo, 0);
                    }
                }

                // End lines drawing.
                GL.End();

                // Pop the current matrix for rendering, and end the drawing clip.
                GL.PopMatrix();
                GUI.EndClip();
            }

            // End our horizontal
            GUILayout.EndHorizontal();
        }
    }
}
