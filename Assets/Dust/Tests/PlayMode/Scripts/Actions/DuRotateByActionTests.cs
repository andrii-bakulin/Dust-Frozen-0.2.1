using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.Actions.Rotate
{
    /*
    public class DuRotateByActionTests : DuRotateActionTests
    {
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator WorldSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var tmp = new GameObject();
            tmp.transform.parent = testObject.transform.parent;
            tmp.transform.localPosition = testObject.transform.localPosition;
            tmp.transform.localRotation = testObject.transform.localRotation;
            tmp.transform.localScale = testObject.transform.localScale;
            tmp.transform.Rotate(rotateBy, Space.World);

            // float timer = 0f;
            // float percentsCompletedLast = 0f;
            // float percentsCompletedNow = 0f;
            // while (timer < duration)
            // {
            //     timer += 1 / 120f; // 120fps :)
            //     percentsCompletedNow = timer / duration;
            //     percentsCompletedNow = Mathf.Min(percentsCompletedNow, 1f);
            //
            //     Vector3 deltaRotate = rotateBy * (percentsCompletedNow - percentsCompletedLast);
            //
            //     tmp.transform.Rotate(deltaRotate, UnityEngine.Space.World);
            //
            //     percentsCompletedLast = percentsCompletedNow;
            // }
            
            Quaternion endInWorld = tmp.transform.rotation;
            Quaternion endInLocal = tmp.transform.localRotation;
            
            var sut = testObject.AddComponent<DuRotateByAction>();
            sut.duration = Sec(duration);
            sut.space = DuRotateByAction.Space.World;
            sut.rotateBy = rotateBy;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }
        
        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator LocalSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var tmp = new GameObject();
            tmp.transform.parent = testObject.transform.parent;
            tmp.transform.localPosition = testObject.transform.localPosition;
            tmp.transform.localRotation = testObject.transform.localRotation;
            tmp.transform.localScale = testObject.transform.localScale;
            tmp.transform.Rotate(rotateBy, Space.Self);

            Quaternion endInWorld = tmp.transform.rotation;
            Quaternion endInLocal = tmp.transform.localRotation;
            
            var sut = testObject.AddComponent<DuRotateByAction>();
            sut.duration = Sec(duration);
            sut.space = DuRotateByAction.Space.Local;
            sut.rotateBy = rotateBy;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }

        [UnityTest, TestCaseSource(nameof(TestCases))]
        public IEnumerator SelfSpaceTest(string objLevelId, float duration, float x, float y, float z)
        {
            var rotateBy = new Vector3(x, y, z);
            var testObject = GetTestGameObject(objLevelId);

            var subObject = new GameObject();
            subObject.transform.parent = testObject.transform;
            subObject.transform.localRotation = Quaternion.Euler(rotateBy);

            var endInWorld = testObject.transform.rotation * Quaternion.Euler(rotateBy);
            var endInLocal = ConvertWorldToLocal(testObject, endInWorld);

            Object.DestroyImmediate(subObject);

            var sut = testObject.AddComponent<DuRotateByAction>();
            sut.duration = Sec(duration);
            sut.space = DuRotateByAction.Space.Self;
            sut.rotateBy = rotateBy;
            sut.Play();

            yield return RotateTest(testObject, duration, endInWorld, endInLocal);
        }
    }
    */
}
