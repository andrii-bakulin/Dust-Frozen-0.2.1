using NUnit.Framework;
using UnityEngine;

public abstract class CorePlayModeTests
{
    protected readonly float TIME_SCALE = 0.25f;
    protected readonly string FLOAT_ACCURACY_MASK = "F2"; // @todo: F5

    protected float Sec(float sec)
    {
        return sec * TIME_SCALE;
    }

    protected void Assert_Equal(Vector3 vut, Vector3 baseValue, string message = "")
    {
        Assert.That(vut.ToString(FLOAT_ACCURACY_MASK), Is.EqualTo(baseValue.ToString(FLOAT_ACCURACY_MASK)), message);
    }

    protected void Assert_NotEqual(Vector3 vut, Vector3 baseValue, string message = "")
    {
        Assert.That(vut.ToString(FLOAT_ACCURACY_MASK), Is.Not.EqualTo(baseValue.ToString(FLOAT_ACCURACY_MASK)), message);
    }
}
