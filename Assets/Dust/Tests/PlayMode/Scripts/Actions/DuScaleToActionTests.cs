using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions
{
    public class DuScaleToActionTests : DuScaleActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = scaleTo;

            var sut = testObject.AddComponent<DuScaleToAction>();
            sut.duration = Sec(duration);
            sut.space = DuScaleToAction.Space.World;
            sut.scaleTo = scaleTo;
            sut.Play();

            yield return ScaleInWorldSpaceTest(testObject, duration, endInWorld);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = scaleTo;

            var sut = testObject.AddComponent<DuScaleToAction>();
            sut.duration = Sec(duration);
            sut.space = DuScaleToAction.Space.Local;
            sut.scaleTo = scaleTo;
            sut.Play();

            yield return ScaleInLocalSpaceTest(testObject, duration, endInLocal);
        }
    }
}
