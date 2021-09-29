using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʹ�ó��򴴽�һ������
/// https://catlikecoding.com/unity/tutorials/procedural-grid/
/// </summary>
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    [Tooltip("�������")]
    public int xSize, ySize;

    private Vector3[] vertices; // ��Ŷ����ȣ�Ҫ����������һ
    private void Awake()
    {
        // Generate();
        StartCoroutine(Generate());
    }

    private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }
        
        Gizmos.color = Color.black;
        foreach(var v in vertices)
        {
            // ����д�Ϳ�����������������ƶ���
            Gizmos.DrawSphere(transform.TransformPoint(v), 0.1f);
        }
    }


    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        // ÿ�еĶ�����Ҫ���������� 1 
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // �����ж��㸳ֵ
        // ��������
        //for(int i = 0; i < xSize + 1; i++)
        //{
        //    for(int j = 0; j < ySize + 1; j++)
        //    {
        //        vertices[i * (ySize + 1) + j][0] = i;
        //        vertices[i * (ySize + 1) + j][1] = j; 
        //        vertices[i * (ySize + 1) + j][2] = 0;
        //    }
        //}

        // ����������
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
    }
}
