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
        Vector3 newLocalPosition = Vector3.zero;
        float step = 2 / (float)solution; // 这个是固定的，没必要每次都在循环里面算

        float d = -1;
        for (int i = 0,a = 0; i < points.Length; i++,a++)
        {
            if (a == solution)
            {
                a = 0;
                d += step;
            }
            Transform p = points[i] = Instantiate(point).transform; // 最好不要每次都用 points[i]，拿一个变量存起来
            p.transform.SetParent(this.transform, false);
            newLocalPosition.x = step * (a + 0.5f) - 1f;
            newLocalPosition.z = d;
            p.transform.localPosition = newLocalPosition;
            p.transform.localScale = Vector3.one * step;
        }
    }

    private void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(functionName);
        Vector3 newLocalPosition;
        float time = Time.time;
        for (int i = 0; i < points.Length; i++)
        {
            Transform t = points[i].transform;
            newLocalPosition = t.localPosition;
            newLocalPosition.y = f(t.localPosition.x, t.localPosition.z, time); 
            t.localPosition = newLocalPosition;
        }
    }

}
