using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    public partial class DuFactory
    {
        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            DrawFieldGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            DrawFieldGizmos();
        }

        private void DrawFieldGizmos()
        {
            if (inspectorDisplay == InspectorDisplay.None)
                return;

            GUIStyle style = new GUIStyle("Label");
            style.fontSize = Mathf.RoundToInt(style.fontSize * inspectorScale);

            int instancesCount = instances.Length;
            for (int i = 0; i < instancesCount; i++)
            {
                var instance = instances[i];

                if (Dust.IsNull(instance))
                {
                    Dust.Debug.StrangeState("DuFactoryGUI.OnSceneGUI", "Instance is null");
                    continue;
                }

                switch (inspectorDisplay)
                {
                    case InspectorDisplay.Power:
                        string value = DuMath.IsNotZero(instance.stateDynamic.power) ? instance.stateDynamic.power.ToString("F2") : "0";
                        Handles.Label(
                            GetPositionInWorldSpace(instance.stateDynamic.position),
                            value,
                            style);
                        break;

                    case InspectorDisplay.Color:
                        Handles.color = instance.stateDynamic.color;
                        Handles.DotHandleCap(0,
                            GetPositionInWorldSpace(instance.stateDynamic.position),
                            transform.rotation,
                            0.05f * inspectorScale,
                            EventType.Repaint);
                        break;

                    case InspectorDisplay.Index:
                        Handles.Label(
                            GetPositionInWorldSpace(instance.stateDynamic.position),
                            instance.instanceIndex.ToString(),
                            style);
                        break;

                    case InspectorDisplay.UV:
                        Handles.color = new Color(instance.stateDynamic.uvw.x, instance.stateDynamic.uvw.y, instance.stateDynamic.uvw.z);
                        Handles.DotHandleCap(0,
                            GetPositionInWorldSpace(instance.stateDynamic.position),
                            transform.rotation,
                            0.05f * inspectorScale,
                            EventType.Repaint);
                        break;
                }
            }
        }
    }
}
#endif
