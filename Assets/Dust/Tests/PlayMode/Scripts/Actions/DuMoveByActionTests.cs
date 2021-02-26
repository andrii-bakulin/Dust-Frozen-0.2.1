using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Move
{
    public class DuMoveByActionTests : DuMoveActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.position + moveBy;
            var endInLocal = ConvertWorldToLocal(testObject, endInWorld);
            
            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveByAction.Space.World;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = testObject.transform.localPosition + moveBy;
            var endInWorld = ConvertLocalToWorld(testObject, endInLocal);
            
            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveByAction.Space.Local;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator SelfSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var subObject = new GameObject();
            subObject.transform.parent = testObject.transform;
            subObject.transform.localPosition = moveBy;

            var endInWorld = subObject.transform.position;
            var endInLocal = ConvertWorldToLocal(testObject, endInWorld);

            Object.DestroyImmediate(subObject);

            var sut = testObject.AddComponent<DuMoveByAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveByAction.Space.Self;
            sut.moveBy = moveBy;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
    }
}
