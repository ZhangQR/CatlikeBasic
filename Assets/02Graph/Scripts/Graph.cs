using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] GameObject point; 
    [SerializeField,Range(3,100)] int solution; // 会产生多少个点
    Transform[] points;
    [SerializeField, Range(0.1f, 10)] float speed;
    [SerializeField] FunctionLibrary.FunctionName functionName;
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
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(functionName);
        float step = 2 / (float)solution;
        Vector3 newLocalPosition = Vector3.zero ;
        newLocalPosition.z = 0.5f * step - 1f;
        float time = Time.time;
        for (int i = 0,a = 0,b = 0; i < points.Length; i++, a++)
        {
            if(a == solution)
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

}
