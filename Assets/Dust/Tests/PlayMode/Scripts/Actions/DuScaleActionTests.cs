using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DustEngine.Test.Actions
{
    public abstract class DuScaleActionTests : DuActionTests
    {
        protected static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var objLevelId in new[] {ObjectTopLevel, ObjectSubLevel})
                foreach (var duration in new[] {1.0f, 0.0f})
                foreach (var x in new[] {0f, 0.5f, 1f, -1f, 0.75f})
                foreach (var y in new[] {0f, 0.5f, 1f, -1f, 1.25f})
                foreach (var z in new[] {0f, 0.5f, 1f, -1f, 3.75f})
                {
                    yield return new TestCaseData(objLevelId, duration, x, y, z).Returns(null);                    
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Helpers

        protected Vector3 ConvertLocalToWorld(GameObject baseObject, Vector3 scaleInLocal)
        {
            if (baseObject.transform.parent == null)
                return scaleInLocal;
            
            return baseObject.transform.parent.TransformPoint(scaleInLocal);
        }

        protected Vector3 ConvertWorldToLocal(GameObject baseObject, Vector3 scaleInWorld)
        {
            if (baseObject.transform.parent == null)
                return scaleInWorld;
            
            return baseObject.transform.parent.InverseTransformPoint(scaleInWorld);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator ScaleTest(GameObject testObject, float duration, Vector3 endInWorld, Vector3 endInLocal)
        {
            Debug.Log($"Start At [WORLD]: {testObject.transform.lossyScale.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Start At [LOCAL]: {testObject.transform.localScale.ToString(FLOAT_ACCURACY_MASK)}");

            Debug.Log($"Expect At [WORLD]: {endInWorld.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Expect At [LOCAL]: {endInLocal.ToString(FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var scaleByCmp = testObject.GetComponent<DuScaleByAction>();
                if (scaleByCmp != null && !scaleByCmp.scaleBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.lossyScale, endInWorld, "Check middle in World space");
                    Assert_NotEqual(testObject.transform.localScale, endInLocal, "Check middle in Local space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            Debug.Log($"Result At [WORLD]: {testObject.transform.lossyScale.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Result At [LOCAL]: {testObject.transform.localScale.ToString(FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.lossyScale, endInWorld, "Check end in World space");
            Assert_Equal(testObject.transform.localScale, endInLocal, "Check end in Local space");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator ScaleInWorldSpaceTest(GameObject testObject, float duration, Vector3 endInWorld)
        {
            Debug.Log($"Start At [WORLD]: {testObject.transform.lossyScale.ToString(FLOAT_ACCURACY_MASK)}");

            Debug.Log($"Expect At [WORLD]: {endInWorld.ToString(FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var scaleByCmp = testObject.GetComponent<DuScaleByAction>();
                if (scaleByCmp != null && !scaleByCmp.scaleBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.lossyScale, endInWorld, "Check middle in World space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            Debug.Log($"Result At [WORLD]: {testObject.transform.lossyScale.ToString(FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.lossyScale, endInWorld, "Check end in World space");
        }
        
        protected IEnumerator ScaleInLocalSpaceTest(GameObject testObject, float duration, Vector3 endInLocal)
        {
            Debug.Log($"Start At [LOCAL]: {testObject.transform.localScale.ToString(FLOAT_ACCURACY_MASK)}");

            Debug.Log($"Expect At [LOCAL]: {endInLocal.ToString(FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var scaleByCmp = testObject.GetComponent<DuScaleByAction>();
                if (scaleByCmp != null && !scaleByCmp.scaleBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.localScale, endInLocal, "Check middle in Local space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            Debug.Log($"Result At [LOCAL]: {testObject.transform.localScale.ToString(FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.localScale, endInLocal, "Check end in Local space");
        }
    }
}
