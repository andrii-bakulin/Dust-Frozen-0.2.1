using System;
using DustEngine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test
{
    public class DustTests
    {
        //--------------------------------------------------------------------------------------------------------------

        [Test]
        public void IsNull_ShouldTrue_WhenSystemObjectIsNull()
        {
            String sut = null;
            Assert.True(Dust.IsNull(sut));
        }

        [Test]
        public void IsNull_ShouldFalse_WhenSystemObjectIsText()
        {
            String sut = "Test";
            Assert.False(Dust.IsNull(sut));
        }

        [Test]
        public void IsNotNull_ShouldTrue_WhenSystemObjectIsText()
        {
            String sut = "Test";
            Assert.True(Dust.IsNotNull(sut));
        }

        [Test]
        public void IsNotNull_ShouldFalse_WhenSystemObjectIsNull()
        {
            String sut = null;
            Assert.False(Dust.IsNotNull(sut));
        }

        //--------------------------------------------------------------------------------------------------------------

        [Test]
        public void IsNull_ShouldTrue_WhenUnityEngineObjectIsNull()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Collider2D>(); // Collider2D definitely NOT EXISTS in empty GameObject

            Assert.True(Dust.IsNull(sut));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void IsNull_ShouldFalse_WhenUnityEngineObjectIsTransform()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Transform>(); // Transform definitely EXISTS in empty GameObject

            Assert.False(Dust.IsNull(sut));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void IsNotNull_ShouldTrue_WhenUnityEngineObjectIsTransform()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Transform>(); // Transform definitely EXISTS in empty GameObject

            Assert.True(Dust.IsNotNull(sut));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void IsNotNull_ShouldFalse_WhenUnityEngineObjectIsNull()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Collider2D>(); // Collider2D definitely NOT EXISTS in empty GameObject

            Assert.False(Dust.IsNotNull(sut));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        //--------------------------------------------------------------------------------------------------------------

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void IsNullOrEmpty_ShouldTrue_WhenStringIs(string sut)
        {
            Assert.True(Dust.IsNullOrEmpty(sut));
        }

        [Test]
        [TestCase(" ")]
        [TestCase("Test")]
        public void IsNullOrEmpty_ShouldFalse_WhenStringIs(string sut)
        {
            Assert.False(Dust.IsNullOrEmpty(sut));
        }
    }
}
