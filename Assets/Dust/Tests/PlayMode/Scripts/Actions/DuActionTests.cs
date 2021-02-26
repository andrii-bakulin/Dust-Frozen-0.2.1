using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.PlayMode
{
    public class DuActionTests : CorePlayModeTests
    {
        protected static IEnumerable<TestCaseData> TestCasesSimple
        {
            get
            {
                yield return new TestCaseData(0.0f, 0.0f, 0.0f).Returns(null);
                yield return new TestCaseData(5.0f, 7.0f, 12.0f).Returns(null);
            }
        }
        
        protected static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(0.0f, 0.0f, 0.0f).Returns(null);
                yield return new TestCaseData(1.0f, -2.0f, 3.0f).Returns(null);
                yield return new TestCaseData(2.3f, 5.7f, 12.5f).Returns(null);
                yield return new TestCaseData(5.0f, 7.0f, 12.0f).Returns(null);
                yield return new TestCaseData(-123.456f, 345.678f, -321.987f).Returns(null);
            }
        }
        
        protected GameObject holderLevel1;
        protected GameObject holderLevel2;
        protected GameObject testObject;
        
        //--------------------------------------------------------------------------------------------------------------

        [UnitySetUp]
        protected IEnumerator SetupPreset1()
        {
            holderLevel1 = new GameObject("Holder1");
            holderLevel1.transform.localPosition = new Vector3(1f, 2f, 3f);
            holderLevel1.transform.localRotation = Quaternion.Euler(45f, 0f, 0f);

            holderLevel2 = new GameObject("Holder2");
            holderLevel2.transform.parent = holderLevel1.transform;
            holderLevel2.transform.localPosition = new Vector3(-1.5f, -2.5f, -3.5f);
            holderLevel2.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);

            testObject = new GameObject("TestObject");
            testObject.transform.parent = holderLevel2.transform;
            testObject.transform.localPosition = new Vector3(1.25f, 2.35f, 3.45f);
            testObject.transform.localRotation = Quaternion.Euler(10f, 20f, 30f);
            
            yield break;
        }

        [UnityTearDown]
        protected IEnumerator ReleasePreset1()
        {
            Object.DestroyImmediate(testObject);   
            Object.DestroyImmediate(holderLevel2);   
            Object.DestroyImmediate(holderLevel1);

            yield break;
        }
    }
}
