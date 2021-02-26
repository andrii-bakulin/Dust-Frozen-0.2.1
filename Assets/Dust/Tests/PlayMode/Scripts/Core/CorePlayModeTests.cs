using NUnit.Framework;
using UnityEngine;

public abstract class CorePlayModeTests
{
    protected readonly float TIME_SCALE = 0.5f;

    protected readonly float VECTOR_DISTANCE_DELTA = 0.005f;
    protected readonly float QUATERNION_ANGLE_DELTA = 0.1f;
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
                   $"NOT Expected: Vector3({baseValue.ToString("F3")})\n" +
                   $"But was:      Vector3({testValue.ToString("F3")})\n" +
                   $"Distance:     {distance} is less than delta {VECTOR_DISTANCE_DELTA} so vectors are EQUAL";
        
        Assert.That(distance, Is.Not.LessThanOrEqualTo(VECTOR_DISTANCE_DELTA), message);
    }

    protected void Assert_Equal(Quaternion testValue, Quaternion baseValue, string message = "")
    {
        float angle = Quaternion.Angle(testValue, baseValue);

        message += (string.IsNullOrEmpty(message) ? "" : "\n" ) +
                   $"Expected: Quaternion.Euler({baseValue.eulerAngles.ToString("F3")})\n" +
                   $"But was:  Quaternion.Euler({testValue.eulerAngles.ToString("F3")})\n" +
                   $"Angle:    {angle} is greater than delta {QUATERNION_ANGLE_DELTA}";
        
        Assert.That(angle, Is.LessThanOrEqualTo(QUATERNION_ANGLE_DELTA), message);
    }

    protected void Assert_NotEqual(Quaternion testValue, Quaternion baseValue, string message = "")
    {
        float angle = Quaternion.Angle(testValue, baseValue);

        message += (string.IsNullOrEmpty(message) ? "" : "\n" ) +
                   $"NOT Expected: Quaternion.Euler({baseValue.eulerAngles.ToString("F3")})\n" +
                   $"But was:      Quaternion.Euler({testValue.eulerAngles.ToString("F3")})\n" +
                   $"Angle:        {angle} is less than delta {QUATERNION_ANGLE_DELTA} so Quaternions are EQUAL";
        
        Assert.That(angle, Is.Not.LessThanOrEqualTo(QUATERNION_ANGLE_DELTA), message);
    }
}
