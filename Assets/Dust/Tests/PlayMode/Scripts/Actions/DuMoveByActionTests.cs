using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.PlayMode
{
    public class DuMoveByActionTests : DuActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);

            var endWorldCheckValue = testObject.transform.position + moveBy;
            var endLocalCheckValue = testObject.transform.parent.InverseTransformPoint(endWorldCheckValue);
            
            yield return MoveTest(DuMoveByAction.Space.World, moveBy, endWorldCheckValue, endLocalCheckValue);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);

            var endLocalCheckValue = testObject.transform.localPosition + moveBy;
            var endWorldCheckValue = testObject.transform.parent.TransformPoint(endLocalCheckValue);
            
            yield return MoveTest(DuMoveByAction.Space.Local, moveBy, endWorldCheckValue, endLocalCheckValue);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator SelfSpaceTest(float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);

            var subObject = new GameObject();
            subObject.transform.parent = testObject.transform;
            subObject.transform.localPosition = moveBy;

            var endWorldCheckValue = subObject.transform.position;
            var endLocalCheckValue = testObject.transform.parent.InverseTransformPoint(endWorldCheckValue);

            Object.DestroyImmediate(subObject);

            yield return MoveTest(DuMoveByAction.Space.Self, moveBy, endWorldCheckValue, endLocalCheckValue);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator MoveTest(DuMoveByAction.Space space, Vector3 moveBy, Vector3 endWorldCheckValue, Vector3 endLocalCheckValue)
        {
            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(1.0f);
            sut.space = space;
            sut.moveBy = moveBy;
            sut.Play();

            yield return new WaitForSeconds(Sec(0.75f));

            if (!sut.moveBy.Equals(Vector3.zero))
            {
                Assert_NotEqual(sut.transform.position, endWorldCheckValue, "Check middle point in World space");
                Assert_NotEqual(sut.transform.localPosition, endLocalCheckValue, "Check middle point in Local space");
            }
            
            yield return new WaitForSeconds(Sec(0.75f));

            Assert_Equal(sut.transform.position, endWorldCheckValue, "Check end point in World space");
            Assert_Equal(sut.transform.localPosition, endLocalCheckValue, "Check end point in Local space");
        }
    }
}
