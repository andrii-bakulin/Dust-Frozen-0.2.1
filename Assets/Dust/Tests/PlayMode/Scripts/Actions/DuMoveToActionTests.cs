using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.PlayMode
{
    public class DuMoveToActionTests : DuMoveActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = moveTo;
            var endInLocal = ConvertWorldToLocal(testObject, endInWorld);

            var sut = testObject.AddComponent<DuMoveToAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveToAction.Space.World;
            sut.moveTo = moveTo;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var moveTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = moveTo;
            var endInWorld = ConvertLocalToWorld(testObject, endInLocal);

            var sut = testObject.AddComponent<DuMoveToAction>();
            sut.duration = Sec(duration);
            sut.space = DuMoveToAction.Space.Local;
            sut.moveTo = moveTo;
            sut.Play();

            yield return MoveTest(testObject, duration, endInWorld, endInLocal);
        }
    }
}
