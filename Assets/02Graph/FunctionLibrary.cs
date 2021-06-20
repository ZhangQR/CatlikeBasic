using UnityEngine;
using static UnityEngine.Mathf;
public static class FunctionLibrary
{
    public delegate float Function(float x, float z, float t);

    static Function[] functions = { Wave, MutiWave , Ripple };
    public enum FunctionName
    {
        Wave,
        MutiWave,
        Ripple
    }
    public static float Wave(float x,float z,float t)
    {
        return Sin(( x + z + t) * PI);
    }

    public static float MutiWave(float x,float z,float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (t + z));
        y += Sin(PI * (0.25f * t + z +x));
        return y * (1f / 2.5f);
    }

    public static float Ripple(float x, float z, float t)
    {
        float d = Sqrt(x * x + z * z);
        float ret = Sin(PI * (4 * d - t));
        return ret / (1f + 10f * d);
    }

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
}
