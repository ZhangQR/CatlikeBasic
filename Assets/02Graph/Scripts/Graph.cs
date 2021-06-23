using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] GameObject point; 
    [SerializeField,Range(3,100)] int solution; // 会产生多少个点
    Transform[] points;
    [SerializeField] FunctionLibrary.FunctionName functionName;
    [SerializeField,Range(0f,5f)] float TranslateTime;
    [SerializeField] float FunctionLastTime;

    FunctionLibrary.FunctionName lastFrameFunctionName;
    bool isTranslating = false;
    float duration = 0f;
    private void Awake()
    {
        points = new Transform[solution * solution];
        float step = 2 / (float)solution;
        for (int i = 0; i < points.Length; i++)
        {
            Transform p = points[i] = Instantiate(point).transform;
            p.transform.SetParent(transform, false);
            p.transform.localScale = Vector3.one * step ;
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(functionName);
        if (isTranslating)
        {
            if(duration < TranslateTime)
            {
                TranslateGraph(FunctionLibrary.GetFunction(lastFrameFunctionName), f);
            }
            else
            {
                isTranslating = false;
                duration = 0f;
                lastFrameFunctionName = functionName;
            }
        }
        else
        {
            if (f != FunctionLibrary.GetFunction(lastFrameFunctionName))
            {
                isTranslating = true;
                duration = 0f;
            }
            else
            {
                if (duration < FunctionLastTime)
                {
                    UpdateGraph(f);
                    lastFrameFunctionName = functionName;
                }
                else
                {
                    functionName = functionName + 1 == FunctionLibrary.FunctionName.Max ? 0 : functionName + 1;
                }
            }
        }
    }

    private void UpdateGraph(FunctionLibrary.Function f)
    {
        float step = 2 / (float)solution;
        Vector3 newLocalPosition = Vector3.zero;
        newLocalPosition.z = 0.5f * step - 1f;
        float time = Time.time;
        for (int i = 0, a = 0, b = 0; i < points.Length; i++, a++)
        {
            if (a == solution)
            {
                a = 0;
                b++;
                newLocalPosition.z = (b + 0.5f) * step - 1f;
            }
            Transform t = points[i].transform;
            newLocalPosition.x = (a + 0.5f) * step - 1f;
            t.localPosition = f(newLocalPosition.x, newLocalPosition.z, time);
        }
    }

    private void TranslateGraph(FunctionLibrary.Function from, FunctionLibrary.Function to)
    {
        float step = 2 / (float)solution;
        Vector3 newLocalPosition = Vector3.zero;
        newLocalPosition.z = 0.5f * step - 1f;
        float time = Time.time;
        float process = duration / TranslateTime;
        for (int i = 0, a = 0, b = 0; i < points.Length; i++, a++)
        {
            if (a == solution)
            {
                a = 0;
                b++;
                newLocalPosition.z = (b + 0.5f) * step - 1f;
            }
            Transform t = points[i].transform;
            newLocalPosition.x = (a + 0.5f) * step - 1f;
            t.localPosition = FunctionLibrary.Morph(newLocalPosition.x, newLocalPosition.z, time, from, to, process);
        }
    }

}
