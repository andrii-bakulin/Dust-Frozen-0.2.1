using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Scale
{
    public class DuScaleToActionTests : DuScaleActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator Scale_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
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
        public IEnumerator Scale_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
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
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator ScaleAndRollback_InWorldSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = testObject.transform.lossyScale;

            var sut = testObject.AddComponent<DuScaleToAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = DuScaleToAction.Space.World;
            sut.scaleTo = scaleTo;
            sut.Play();

            yield return ScaleInWorldSpaceTest(testObject, duration, endInWorld);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator ScaleAndRollback_InLocalSpace(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleTo = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = testObject.transform.localScale;

            var sut = testObject.AddComponent<DuScaleToAction>();
            sut.duration = Sec(duration * 0.5f);
            sut.playRollback = true;
            sut.rollbackDuration = Sec(duration * 0.5f);
            sut.space = DuScaleToAction.Space.Local;
            sut.scaleTo = scaleTo;
            sut.Play();

            yield return ScaleInLocalSpaceTest(testObject, duration, endInLocal);
        }
    }
}
