using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Scale
{
    public class DuScaleByActionTests : DuScaleActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInWorld = Vector3.Scale(testObject.transform.lossyScale, scaleBy);
            
            var sut = testObject.AddComponent<DuScaleByAction>();
            sut.duration = Sec(duration);
            sut.space = DuScaleByAction.Space.World;
            sut.scaleBy = scaleBy;
            sut.Play();

            yield return ScaleInWorldSpaceTest(testObject, duration, endInWorld);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var scaleBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var endInLocal = Vector3.Scale(testObject.transform.localScale, scaleBy);
            
            var sut = testObject.AddComponent<DuScaleByAction>();
            sut.duration = Sec(duration);
            sut.space = DuScaleByAction.Space.Local;
            sut.scaleBy = scaleBy;
            sut.Play();

            yield return ScaleInLocalSpaceTest(testObject, duration, endInLocal);
        }
    }
}
