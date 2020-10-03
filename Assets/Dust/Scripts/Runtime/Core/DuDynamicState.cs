using UnityEngine;

namespace DustEngine
{
    // @DUST.todo: do something with consts
    public static class DuDynamicState
    {
        public static int Normalize(int state)
        {
            return state != 0 ? state : 1;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, bool value)
        {
            dynamicState ^= sequenceIndex * 854837 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, int value)
        {
            dynamicState ^= sequenceIndex * 330177 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, System.Enum value)
        {
            dynamicState ^= sequenceIndex * 366250 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, float value)
        {
            dynamicState ^= sequenceIndex * 974003 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Vector3 value)
        {
            dynamicState ^= sequenceIndex * 575673 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Color value)
        {
            dynamicState ^= sequenceIndex * 625751 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Gradient value)
        {
            dynamicState ^= sequenceIndex * 238715 + (Dust.IsNotNull(value) ? value.GetHashCode() : 123456);
        }

        public static void Append(ref int dynamicState, int sequenceIndex, AnimationCurve value)
        {
            dynamicState ^= sequenceIndex * 772937 + (Dust.IsNotNull(value) ? value.GetHashCode() : 123456);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, Transform transform)
        {
            if (Dust.IsNotNull(transform))
            {
                dynamicState ^= sequenceIndex * 784449 + transform.position.GetHashCode();
                dynamicState ^= sequenceIndex * 807525 + transform.rotation.GetHashCode();
                dynamicState ^= sequenceIndex * 371238 + transform.lossyScale.GetHashCode();
            }
            else
            {
                dynamicState ^= sequenceIndex * 784449 + 123456;
                dynamicState ^= sequenceIndex * 807525 + 123456;
                dynamicState ^= sequenceIndex * 371238 + 123456;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, DuDeformer deformer)
        {
            dynamicState ^= sequenceIndex * 291422 + (Dust.IsNotNull(deformer) ? deformer.GetDynamicStateHashCode() : 123456);
        }

        public static void Append(ref int dynamicState, int sequenceIndex, DuFieldsMap fieldsMap)
        {
            dynamicState ^= sequenceIndex * 955735 + (Dust.IsNotNull(fieldsMap) ? fieldsMap.GetDynamicStateHashCode() : 123456);
        }

        public static void Append(ref int dynamicState, int sequenceIndex, DuField field)
        {
            dynamicState ^= sequenceIndex * 512661 + (Dust.IsNotNull(field) ? field.GetDynamicStateHashCode() : 123456);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, Mesh mesh)
        {
            dynamicState ^= sequenceIndex * 848409 + (Dust.IsNotNull(mesh) ? mesh.GetHashCode() : 123456);
        }
    }
}
