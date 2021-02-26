using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.PlayMode
{
    public class DuMoveByActionTests : DuMoveActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endWorldCheckValue = testObject.transform.position + moveBy;
            var endLocalCheckValue = testObject.transform.parent != null 
                ? testObject.transform.parent.InverseTransformPoint(endWorldCheckValue)
                : endWorldCheckValue;
            
            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveByAction.Space.World;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endWorldCheckValue, endLocalCheckValue);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endLocalCheckValue = testObject.transform.localPosition + moveBy;
            var endWorldCheckValue = testObject.transform.parent != null
                ? testObject.transform.parent.TransformPoint(endLocalCheckValue)
                : endLocalCheckValue;
            
            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveByAction.Space.Local;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endWorldCheckValue, endLocalCheckValue);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator SelfSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var subObject = new GameObject();
            subObject.transform.parent = testObject.transform;
            subObject.transform.localPosition = moveBy;

            var endWorldCheckValue = subObject.transform.position;
            var endLocalCheckValue = testObject.transform.parent != null
                ? testObject.transform.parent.InverseTransformPoint(endWorldCheckValue)
                : endWorldCheckValue;

            Object.DestroyImmediate(subObject);

            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveByAction.Space.Self;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endWorldCheckValue, endLocalCheckValue);
        }
    }
}
