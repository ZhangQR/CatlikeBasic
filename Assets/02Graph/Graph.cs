using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] GameObject point; 
    [SerializeField,Range(3,100)] int solution; // ��������ٸ���
    Transform[] points;
    [SerializeField, Range(0.1f, 10)] float speed; 

    private void Awake()
    {
        points = new Transform[solution];
        Vector3 newLocalPosition = Vector3.zero;
        float step = 2 / (float)solution; // ����ǹ̶��ģ�û��Ҫÿ�ζ���ѭ��������

        for (int i = 0; i < solution; i++)
        {
            Transform p = points[i] = Instantiate(point).transform; // ��ò�Ҫÿ�ζ��� points[i]����һ������������
            p.transform.SetParent(this.transform, false);
            newLocalPosition.x = step * (i + 0.5f) - 1f;
            p.transform.localPosition = newLocalPosition;
            p.transform.localScale = Vector3.one * step;
        }
    }

    private void Update()
    {
        Vector3 newLocalPosition;
        float time = Time.time;
        for (int i = 0; i < solution; i++)
        {
            Transform t = points[i].transform;
            newLocalPosition = t.localPosition;
            newLocalPosition.y = Mathf.Sin((t.localPosition.x + time * speed) * Mathf.PI);
            t.localPosition = newLocalPosition;
        }
    }

}
