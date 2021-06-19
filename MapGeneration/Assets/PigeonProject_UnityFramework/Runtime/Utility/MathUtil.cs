using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PigeonProject
{
    public static class MathUtil
    {
        public static float GetPerlinNoise(float x, float y = 0f, float seedX = 0f, float seedY = 0f, float frequency = 1f, float min = 0f, float max = 1f)
        {
            float perlin = Mathf.PerlinNoise(seedX + x * frequency, seedY + y * frequency);
            return perlin * (max - min) + min;
        }

        /// <param name="angle">in radians</param>
        public static Vector2 RotateVector2(Vector2 v, float angle)
        {
            return new Vector2(
                v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle),
                v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle)
            );
        }

        public static int Mod(int dividend, int divisor)
        {
            int rest = dividend % divisor;
            return rest < 0 ? rest + divisor : rest;
        }

        public static Vector2 Vector3to2(Vector3 v, SnapAxis axis = SnapAxis.Z)
        {
            switch(axis)
            {
                case SnapAxis.X:
                    return new Vector2(v.z, v.y);
                case SnapAxis.Y:
                    return new Vector2(v.x, v.z);
                case SnapAxis.Z:
                default:
                    return new Vector2(v.x, v.y);
            }
        }
        public static Vector3 Vector2to3(Vector2 v, SnapAxis axis = SnapAxis.Z)
        {
            switch(axis)
            {
                case SnapAxis.X:
                    return new Vector3(0f, v.y, v.x);
                case SnapAxis.Y:
                     return new Vector3(v.x, 0f, v.y);
                case SnapAxis.Z:
                default:
                    return new Vector3(v.x, v.y, 0f);
            }
        }
        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
 
            randDirection += origin;
 
            NavMeshHit navHit;
 
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
 
            return navHit.position;
        }

        public static void Vector2ToPolar(Vector2 v, out float radius, out float angle)
        {
            radius = v.magnitude;
            angle = Mathf.Atan2(v.y, v.x);
        }
        public static Vector2 PolarToVector2(float radius, float angle)
        {
            return new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        }

        public static int Wrap(int number, int minInclusive, int maxExclusive)
        {
            if (number >= maxExclusive)
                return Wrap(minInclusive + (number - maxExclusive), minInclusive, maxExclusive);
            if (number < minInclusive)
                return Wrap(maxExclusive - (minInclusive - number), minInclusive, maxExclusive);
            return number;
        }

        public static Vector3 Vector3MultiplyMemberwise(Vector3 a, Vector3 b)
        {
            return new Vector3
            (
                a.x * b.x,
                a.y * b.y,
                a.z * b.z
            );
        }
    }
}
