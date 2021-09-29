using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用程序创建一个网格
/// https://catlikecoding.com/unity/tutorials/procedural-grid/
/// </summary>
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    [Tooltip("网格个数")]
    public int xSize, ySize;

    private Vector3[] vertices; // 存放顶点萌，要比网格数多一
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
            // 这样写就可以让球球跟着物体移动了
            Gizmos.DrawSphere(transform.TransformPoint(v), 0.1f);
        }
    }


    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        // 每行的顶点数要比网格数多 1 
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // 给所有顶点赋值
        // 笨笨方法
        //for(int i = 0; i < xSize + 1; i++)
        //{
        //    for(int j = 0; j < ySize + 1; j++)
        //    {
        //        vertices[i * (ySize + 1) + j][0] = i;
        //        vertices[i * (ySize + 1) + j][1] = j; 
        //        vertices[i * (ySize + 1) + j][2] = 0;
        //    }
        //}

        // 不笨笨方法
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
