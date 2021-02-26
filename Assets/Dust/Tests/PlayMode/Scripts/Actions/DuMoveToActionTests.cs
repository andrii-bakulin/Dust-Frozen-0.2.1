using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.PlayMode
{
    public class DuMoveToActionTests : DuActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);

            var endWorldCheckValue = moveTo;
            var endLocalCheckValue = testObject.transform.parent.InverseTransformPoint(moveTo);

            yield return MoveTest(DuMoveToAction.Space.World, moveTo, endWorldCheckValue, endLocalCheckValue);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);

            var endWorldCheckValue = testObject.transform.parent.TransformPoint(moveTo);
            var endLocalCheckValue = moveTo;

            yield return MoveTest(DuMoveToAction.Space.Local, moveTo, endWorldCheckValue, endLocalCheckValue);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected IEnumerator MoveTest(DuMoveToAction.Space space, Vector3 moveTo, Vector3 endWorldCheckValue, Vector3 endLocalCheckValue)
        {
            var sut = testObject.AddComponent<DuMoveToAction>();
            sut.duration = Sec(1.0f);
            sut.space = space;
            sut.moveTo = moveTo;
            sut.Play();

            yield return new WaitForSeconds(Sec(0.75f));

            // @Notice: Maybe situation when object did not change position in start & move-to points is equals :)
            //          So may assert failed!

            Assert_NotEqual(sut.transform.position, endWorldCheckValue, "Check middle point in World space");
            Assert_NotEqual(sut.transform.localPosition, endLocalCheckValue, "Check middle point in Local space");
            
            yield return new WaitForSeconds(Sec(0.75f));

            Assert_Equal(sut.transform.position, endWorldCheckValue, "Check end point in World space");
            Assert_Equal(sut.transform.localPosition, endLocalCheckValue, "Check end point in Local space");
        }
    }
}
