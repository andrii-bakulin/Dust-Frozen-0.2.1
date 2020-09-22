using UnityEngine;

namespace DustEngine
{
    // @DUST.todo: (* ..123..) change for bit moves
    public static class DuDynamicState
    {
        public static int Normalize(int state)
        {
            return state != 0 ? state : 1;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, bool value)
        {
            dynamicState += sequenceIndex * 1234 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, int value)
        {
            dynamicState += sequenceIndex * 1234 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, float value)
        {
            dynamicState += sequenceIndex * 1234 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Vector3 value)
        {
            dynamicState += sequenceIndex * 1234 + value.GetHashCode();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, Transform transform)
        {
            dynamicState += sequenceIndex * 1231 + transform.position.GetHashCode();
            dynamicState += sequenceIndex * 1232 + transform.rotation.GetHashCode();
            dynamicState += sequenceIndex * 1233 + transform.lossyScale.GetHashCode();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, DuDeformer deformer)
        {
            dynamicState += sequenceIndex * 1234 + deformer.GetDynamicStateHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, DuFieldsMap fieldsMap)
        {
            dynamicState += sequenceIndex * 1234 + 0; // @DUST.todo:
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, Mesh mesh)
        {
            dynamicState += sequenceIndex * 1234 + mesh.GetHashCode();
        }
    }
}
