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
                // ObjectTopLevel, Duration = 1sec
                yield return new TestCaseData(ObjectTopLevel, 1f,     0.00f,    0.00f,     0.00f).Returns(null);
                yield return new TestCaseData(ObjectTopLevel, 1f,    -5.00f,    7.50f,   -12.25f).Returns(null);
                yield return new TestCaseData(ObjectTopLevel, 1f, -1234.56f, 3456.78f, -3219.87f).Returns(null);

                // ObjectSubLevel, Duration = 1sec
                yield return new TestCaseData(ObjectSubLevel, 1f,     0.00f,    0.00f,     0.00f).Returns(null);
                yield return new TestCaseData(ObjectSubLevel, 1f,    -5.00f,    7.50f,   -12.25f).Returns(null);
                yield return new TestCaseData(ObjectSubLevel, 1f, -1234.56f, 3456.78f, -3219.87f).Returns(null);

                // ObjectSubLevel, Duration = 0sec
                yield return new TestCaseData(ObjectSubLevel, 0f,     0.00f,    0.00f,     0.00f).Returns(null);
                yield return new TestCaseData(ObjectSubLevel, 0f,    -5.00f,    7.50f,   -12.25f).Returns(null);
                yield return new TestCaseData(ObjectSubLevel, 0f, -1234.56f, 3456.78f, -3219.87f).Returns(null);
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator MoveTest(GameObject testObject, float duration, Vector3 endWorldCheckValue, Vector3 endLocalCheckValue)
        {
            Debug.Log($"Start At [WORLD]: {testObject.transform.position.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Start At [LOCAL]: {testObject.transform.localPosition.ToString(FLOAT_ACCURACY_MASK)}");

            Debug.Log($"Expect At [WORLD]: {endWorldCheckValue.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Expect At [LOCAL]: {endLocalCheckValue.ToString(FLOAT_ACCURACY_MASK)}");

            yield return new WaitForSeconds(Sec(duration * 0.75f));

            if (duration > 0f)
            {
                var moveByCmp = testObject.GetComponent<DuMoveByAction>();
                if (moveByCmp != null && !moveByCmp.moveBy.Equals(Vector3.zero))
                {
                    Assert_NotEqual(testObject.transform.position, endWorldCheckValue, "Check middle point in World space");
                    Assert_NotEqual(testObject.transform.localPosition, endLocalCheckValue, "Check middle point in Local space");
                }
                else // is DuMoveToAction
                {
                    // @Notice: Maybe situation when object did not change position in start & move-to points is equals :)
                    //          So may assert failed!
                }
            
                yield return new WaitForSeconds(Sec(duration * 0.75f));
            }

            Debug.Log($"Result At [WORLD]: {testObject.transform.position.ToString(FLOAT_ACCURACY_MASK)}");
            Debug.Log($"Result At [LOCAL]: {testObject.transform.localPosition.ToString(FLOAT_ACCURACY_MASK)}");

            Assert_Equal(testObject.transform.position, endWorldCheckValue, "Check end point in World space");
            Assert_Equal(testObject.transform.localPosition, endLocalCheckValue, "Check end point in Local space");
        }
    }
}
