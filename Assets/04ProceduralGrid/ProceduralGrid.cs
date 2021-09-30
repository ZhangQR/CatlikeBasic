using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʹ�ó��򴴽�һ������
/// https://catlikecoding.com/unity/tutorials/procedural-grid/
/// </summary>
// Ҫ��Ⱦ��һ�����壬�ر��������������һ���ṩģ�ͣ��ڶ����ṩ��Ⱦ��ʽ
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    [Tooltip("�������")]
    public int xSize, ySize;

    private Vector3[] vertices; // ��Ŷ����ȣ�Ҫ����������һ
    private Mesh mesh;
    private void Awake()
    {
        // Generate();
        mesh = GetComponent<MeshFilter>().mesh;
        // ����һ�����㣬һ��������������
        StartCoroutine(Generate());
    }

    /// <summary>
    /// �� Scene ���ڻ�������
    /// </summary>
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


    /// <summary>
    /// ��������������Э����Ϊ���ö����������һ�����ĳ���
    /// ������Ҫ�أ�
    /// 1���������飬����ÿ�����������
    /// 2�����������飬ÿ����Ϊһ�������Σ���������������±꣨��������������꣩
    ///    ������������Ҫע������Ԫ�ص�˳�����ַ���˳��ʱ��
    /// 3���������飬͵���Ļ������Զ����㣻��������
    /// 4��UV���������
    /// 5�����˵Ļ������������ö���ɫ��standard ��ɫ������û���õ�������������Զ�����ɫ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        //************************************************************
        //************************************************************
        //****      ��䶥������                                  ****
        //************************************************************
        //************************************************************
        // xSize��ySize������������������ÿ�еĶ�����Ҫ���������� 1 
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
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
                // yield return wait;
            }
        }
        mesh.vertices = vertices;

        //************************************************************
        //************************************************************
        //****     ������������飬ע�ⷽ��                       ****
        //************************************************************
        //************************************************************
        int[] triangles = new int[xSize * ySize * 6];
        for(int y = 0, ti = 0, vi = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6,vi++)
            {
                // ��һ������
                //triangles[0] = 0;
                //triangles[1] = triangles[4] = xSize + 1;
                //triangles[2] = triangles[3] = 1;
                //triangles[5] = xSize + 2;

                // ��֪��ѭ����ôд�Ļ����Ͷ�дһ��ѭ���ҹ���
                //triangles[6] = 1;
                //triangles[7] = triangles[10] = xSize + 1 + 1;
                //triangles[2+6] = triangles[3+6] = 1+1;

                // ��һ�о���
                //triangles[ti + 0] = x;
                //triangles[ti + 1] = triangles[ti + 4] = x + xSize + 1;
                //triangles[ti + 2] = triangles[ti + 3] = x + 1;
                //triangles[ti + 5] = x + xSize + 2;
                //mesh.triangles = triangles;
                //yield return wait;

                // ȫ�������������ϣ���������
                //int offset = y * (xSize + 1);
                //int offsetV = y * 6 * xSize;
                //triangles[offsetV + ti + 0] = x + offset;
                //triangles[offsetV + ti + 1] = triangles[offsetV + ti + 4] = x + xSize + 1 + offset;
                //triangles[offsetV + ti + 2] = triangles[offsetV + ti + 3] = x + 1 + offset;
                //triangles[offsetV + ti + 5] = x + xSize + 2 + offset;
                //mesh.triangles = triangles;
                //yield return wait;

                // ȫ�������������ϣ�����������
                triangles[ti + 0] = vi;
                triangles[ti + 1] = triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
        }

        //************************************************************
        //************************************************************
        //****     ��䷨��                                       ****
        //**** ������ÿ������һ����ͬһ�������εķ��߻���һ��   ****
        //**** ����Ĭ���ǣ�0,0,1��                                ****
        //**** Ҫ���� Normal Map ����Ҫ������ͼ                   ****
        //************************************************************
        //************************************************************
        // ����������ҵ���ÿ�����������������Σ�Ȼ������Щ�����η��ߵ�ƽ�������ҹ�һ��
        // ��������ķ��߸���ѧ�ϵĺܲ�һ������һ������һ����������һ��ƽ��һ��
        mesh.RecalculateNormals();
        Vector4[] tangents = new Vector4[vertices.Length];
        // ����������У�����ĵ���λͨ���� 0���� Unity �е����ߣ�����λ�� -1 ���� 1
        // ����ȷ�������ߵķ��򣬾��������� Shader ���㸺���ߵ�ʱ��� *tangent.w
        // ��ô����ԭ���������Ϊ���ڶԳƵ�ģ�ͣ����Ժܷ���Ŀ������߿ռ��ǰ��
        Vector4 tangent = new Vector4(1, 0, 0, -1);
        for (int y = 0, i = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                // �������ߣ����ԺͶ���һ�𣬵�Ϊ�����������ǵ���һ��ѭ��
                tangents[i] = tangent;
            }
        }
        mesh.tangents = tangents;

        //************************************************************
        //************************************************************
        //**** UV                                                 ****
        //**** UV �������                                        ****
        //**** UV �����ڶ����ʱ��һ�����                        ****
        //**** ��Ϊ����������������ֿ�����                       ****
        //************************************************************
        //************************************************************
        Vector2[] uv = new Vector2[vertices.Length];
        for(int y = 0,i = 0; y <= ySize; y++)
        {
            for(int x = 0; x <= xSize; x++, i++)
            {
                // ������Ǽ򵥵ؽ���ƽ��,Ҫǿתһ������,��Ȼֻ�� 0,1
                // ��Ҫע�� texture �� repeat mode
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }
        mesh.uv = uv;
    }
}
