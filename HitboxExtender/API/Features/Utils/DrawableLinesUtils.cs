using DrawableLine;
using UnityEngine;

namespace HitboxExtender.API.Features.Utils;

public static class DrawableLinesUtils
{
    /// <summary>
    /// Useful System for generating Capsule with <see cref="DrawableLines"/>
    /// </summary>
    /// <param name="start">position where it starts</param>
    /// <param name="end">position where it ends</param>
    /// <param name="radius">how big the capsule is</param>
    /// <param name="color">color of the capsule</param>
    /// <param name="duration">how much time it will stay</param>
    /// <param name="segments">how many rings are created to compose it</param>
    public static void GenerateCapsule(Vector3 start, Vector3 end, float radius, Color? color = null, float? duration = null, int segments = 8)
    {
        if (!DrawableLines.IsDebugModeEnabled) return;
        
        DrawableLines.GenerateSphere(start, radius, duration, color, segments);
        DrawableLines.GenerateSphere(end, radius, duration, color, segments);
        
        Vector3 dir = (end - start).normalized;
        Vector3 right = Vector3.Cross(dir, Vector3.up);
        if (right == Vector3.zero) right = Vector3.Cross(dir, Vector3.forward);
        right = right.normalized;

        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            Quaternion rotA = Quaternion.AngleAxis(i * angleStep, dir);
            Quaternion rotB = Quaternion.AngleAxis((i + 1) % segments * angleStep, dir);

            Vector3 offsetA = rotA * right * radius;
            Vector3 offsetB = rotB * right * radius;

            Vector3 startA = start + offsetA;
            Vector3 startB = start + offsetB;
            Vector3 endA = end + offsetA;
            Vector3 endB = end + offsetB;

            // tube side
            DrawableLines.GenerateLine(duration, color, startA, endA);
            // sphere ring edges
            DrawableLines.GenerateLine(duration, color, startA, startB);
            DrawableLines.GenerateLine(duration, color, endA, endB);
        }
    }
}