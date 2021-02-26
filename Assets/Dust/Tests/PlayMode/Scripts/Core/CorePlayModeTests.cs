using NUnit.Framework;
using UnityEngine;

public abstract class CorePlayModeTests
{
    protected readonly float TIME_SCALE = 0.25f;

    protected readonly float VECTOR_DISTANCE_DELTA = 0.001f;
    protected readonly string FLOAT_ACCURACY_MASK = "F3";

    protected float Sec(float sec)
    {
        return sec * TIME_SCALE;
    }

    protected void Assert_Equal(Vector3 testValue, Vector3 baseValue, string message = "")
    {
        float distance = Vector3.Distance(testValue, baseValue);

        message += (string.IsNullOrEmpty(message) ? "" : "\n" ) +
                   $"Expected: Vector3({baseValue.ToString("F3")})\n" +
                   $"But was:  Vector3({testValue.ToString("F3")})\n" +
                   $"Distance: {distance} is greater than delta {VECTOR_DISTANCE_DELTA}";
        
        Assert.That(distance, Is.LessThanOrEqualTo(VECTOR_DISTANCE_DELTA), message);
    }

    protected void Assert_NotEqual(Vector3 testValue, Vector3 baseValue, string message = "")
    {
        float distance = Vector3.Distance(testValue, baseValue);

        message += (string.IsNullOrEmpty(message) ? "" : "\n" ) +
                  $"Expected: Vector3({baseValue.ToString("F3")})\n" +
                  $"But was:  Vector3({testValue.ToString("F3")})\n" +
                  $"Distance: {distance} is less than delta {VECTOR_DISTANCE_DELTA} so vectors are EQUAL";
        
        Assert.That(distance, Is.Not.LessThanOrEqualTo(VECTOR_DISTANCE_DELTA), message);
    }
}
