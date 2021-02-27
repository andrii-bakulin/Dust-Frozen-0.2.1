using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Rotate
{
    public class DuRotateToActionTests : DuRotateActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);
            
            Quaternion endInWorld = Quaternion.Euler(rotateTo);
            Quaternion endInLocal = ConvertWorldToLocal(testObject, endInWorld);

            var sut = testObject.AddComponent<DuRotateToAction>();
            sut.duration = Sec(duration);
            sut.space = DuRotateToAction.Space.World;
            sut.rotateTo = rotateTo;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            Quaternion endInLocal = Quaternion.Euler(rotateTo);
            Quaternion endInWorld = ConvertLocalToWorld(testObject, endInLocal);

            var sut = testObject.AddComponent<DuRotateToAction>();
            sut.duration = Sec(duration);
            sut.space = DuRotateToAction.Space.Local;
            sut.rotateTo = rotateTo;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }
    }
}
