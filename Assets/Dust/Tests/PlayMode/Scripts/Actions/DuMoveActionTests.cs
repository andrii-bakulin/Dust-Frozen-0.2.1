using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DustEngine.Test.PlayMode
{
    public abstract class DuMoveActionTests : DuActionTests
    {
        protected static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var objLevelId in new[] {ObjectTopLevel, ObjectSubLevel})
                foreach (var duration in new[] {1.0f, 0.0f})
                foreach (var x in new[] {0f, 1f,  -5.00f, -1234.56f})
                foreach (var y in new[] {0f, 1f,   7.50f,  3456.78f})
                foreach (var z in new[] {0f, 1f, -12.25f, -9999.99f})
                {
                    yield return new TestCaseData(objLevelId, duration, x, y, z).Returns(null);                    
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Helpers

        protected Vector3 ConvertLocalToWorld(GameObject baseObject, Vector3 pointInLocal)
        {
            if (baseObject.transform.parent == null)
                return pointInLocal;
            
            return baseObject.transform.parent.TransformPoint(pointInLocal);
        }

        protected Vector3 ConvertWorldToLocal(GameObject baseObject, Vector3 pointInWorld)
        {
            if (baseObject.transform.parent == null)
                return pointInWorld;
            
            return baseObject.transform.parent.InverseTransformPoint(pointInWorld);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator MoveTest(GameObject testObject, float duration, Vector3 endInWorld, Vector3 endInLocal)
        {
            Debug.Log($"Start At [WORLD]: {testObject.transform.position.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Start At [LOCAL]: {testObject.transform.localPosition.ToString(FLOAT_ACCURACY_MASK)}");

            Debug.Log($"Expect At [WORLD]: {endInWorld.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Expect At [LOCAL]: {endInLocal.ToString(FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var moveByCmp = testObject.GetComponent<DuMoveByAction>();
                if (moveByCmp != null && !moveByCmp.moveBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.position, endInWorld, "Check middle point in World space");
                    Assert_NotEqual(testObject.transform.localPosition, endInLocal, "Check middle point in Local space");
                }
            }

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            Debug.Log($"Result At [WORLD]: {testObject.transform.position.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Result At [LOCAL]: {testObject.transform.localPosition.ToString(FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.position, endInWorld, "Check end point in World space");
            Assert_Equal(testObject.transform.localPosition, endInLocal, "Check end point in Local space");
        }
    }
}
