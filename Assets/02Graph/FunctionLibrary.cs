using UnityEngine;
using static UnityEngine.Mathf;
public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    static Function[] functions = { Wave, MutiWave , Ripple , Sphere };
    public enum FunctionName
    {
        Wave,
        MutiWave,
        Ripple,
        Sphere
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
        Vector3 p = Vector3.zero;
        float r = Cos(v * PI * 0.5f);
        p.x = r*Sin(PI * u);
        p.y = Sin(PI * 0.5f * v);
        p.z = r*Cos(PI * u);
        return p;
    }

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
}
