using UnityEngine;
using static UnityEngine.Mathf;
public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    static Function[] functions = { Wave, MutiWave , Ripple , Sphere, Tour };
    public enum FunctionName
    {
        Wave,
        MutiWave,
        Ripple,
        Sphere,
        Tour,
        Max
    }
    public static Vector3 Wave(float u,float v,float t)
    {
        Vector3 p = Vector3.zero;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }

    public static Vector3 MutiWave(float u,float v,float t)
    {
        Vector3 p = Vector3.zero;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f * t));
        p.y += 0.5f * Sin(2f * PI * (t + v));
        p.y += Sin(PI * (0.25f * t + v + v));
        p.y *= (1f / 2.5f);
        p.z = v;
        return p;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 p = Vector3.zero;
        p.x = u;
        float d = Sqrt(u * u + v * v);
        p.y = Sin(PI * (4 * d - t));
        p.y /= (1f + 10f * d);
        p.z = v;
        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 5 * v+ t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Tour(float u, float v, float t)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Morph(float u,float v,float t,Function from,Function to,float process)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f,1f,process));
    }

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
}
