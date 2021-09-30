using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用程序创建一个网格
/// https://catlikecoding.com/unity/tutorials/procedural-grid/
/// </summary>
// 要渲染出一个物体，必备的两个组件，第一个提供模型，第二个提供渲染方式
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    [Tooltip("网格个数")]
    public int xSize, ySize;

    private Vector3[] vertices; // 存放顶点萌，要比网格数多一
    private Mesh mesh;
    private void Awake()
    {
        // Generate();
        mesh = GetComponent<MeshFilter>().mesh;
        // 可以一个顶点，一个网格的逐次生成
        StartCoroutine(Generate());
    }

    /// <summary>
    /// 在 Scene 窗口画出顶点
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
            // 这样写就可以让球球跟着物体移动了
            Gizmos.DrawSphere(transform.TransformPoint(v), 0.1f);
        }
    }


    /// <summary>
    /// 生成网格，这里用协程是为了让顶点和三角形一个个的出现
    /// 网格四要素：
    /// 1、顶点数组，包含每个顶点的坐标
    /// 2、三角形数组，每三个为一个三角形，包含顶点数组的下标（不包含具体的坐标）
    ///    三角形数组需要注意三个元素的顺序，左手法则，顺逆时针
    /// 3、法线数组，偷懒的话可以自动计算；切线数组
    /// 4、UV，最多四组
    /// 5、高兴的话，还可以设置顶点色，standard 着色器里面没有用到它，但你可以自定义着色器
    /// </summary>
    /// <returns></returns>
    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        //************************************************************
        //************************************************************
        //****      填充顶点数组                                  ****
        //************************************************************
        //************************************************************
        // xSize，ySize都是网格数量，所以每行的顶点数要比网格数多 1 
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
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
                // yield return wait;
            }
        }
        mesh.vertices = vertices;

        //************************************************************
        //************************************************************
        //****     填充三角形数组，注意方向                       ****
        //************************************************************
        //************************************************************
        int[] triangles = new int[xSize * ySize * 6];
        for(int y = 0, ti = 0, vi = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6,vi++)
            {
                // 第一个矩形
                //triangles[0] = 0;
                //triangles[1] = triangles[4] = xSize + 1;
                //triangles[2] = triangles[3] = 1;
                //triangles[5] = xSize + 2;

                // 不知道循环怎么写的话，就多写一次循环找规律
                //triangles[6] = 1;
                //triangles[7] = triangles[10] = xSize + 1 + 1;
                //triangles[2+6] = triangles[3+6] = 1+1;

                // 第一行矩形
                //triangles[ti + 0] = x;
                //triangles[ti + 1] = triangles[ti + 4] = x + xSize + 1;
                //triangles[ti + 2] = triangles[ti + 3] = x + 1;
                //triangles[ti + 5] = x + xSize + 2;
                //mesh.triangles = triangles;
                //yield return wait;

                // 全部三角形填充完毕：笨笨方法
                //int offset = y * (xSize + 1);
                //int offsetV = y * 6 * xSize;
                //triangles[offsetV + ti + 0] = x + offset;
                //triangles[offsetV + ti + 1] = triangles[offsetV + ti + 4] = x + xSize + 1 + offset;
                //triangles[offsetV + ti + 2] = triangles[offsetV + ti + 3] = x + 1 + offset;
                //triangles[offsetV + ti + 5] = x + xSize + 2 + offset;
                //mesh.triangles = triangles;
                //yield return wait;

                // 全部三角形填充完毕：不笨笨方法
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
        //****     填充法线                                       ****
        //**** 法线是每个顶点一个，同一个三角形的法线或许不一样   ****
        //**** 法线默认是（0,0,1）                                ****
        //**** 要是用 Normal Map 还需要切线贴图                   ****
        //************************************************************
        //************************************************************
        // 这个函数会找到和每个顶点相连的三角形，然后算这些三角形法线的平均，并且归一化
        // 所以这里的法线跟数学上的很不一样，是一个顶点一个，并不是一个平面一个
        mesh.RecalculateNormals();
        Vector4[] tangents = new Vector4[vertices.Length];
        // 在齐次坐标中，方向的第四位通常是 0，但 Unity 中的切线，第四位是 -1 或者 1
        // 用于确定负切线的方向，就像我们在 Shader 计算负切线的时候会 *tangent.w
        // 这么做的原因或许是因为对于对称的模型，可以很方便的控制切线空间的前后
        Vector4 tangent = new Vector4(1, 0, 0, -1);
        for (int y = 0, i = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                // 设置切线，可以和顶点一起，但为了清晰，还是单独一个循环
                tangents[i] = tangent;
            }
        }
        mesh.tangents = tangents;

        //************************************************************
        //************************************************************
        //**** UV                                                 ****
        //**** UV 最多四组                                        ****
        //**** UV 可以在顶点的时候一起计算                        ****
        //**** 但为了清晰，所以这里分开来了                       ****
        //************************************************************
        //************************************************************
        Vector2[] uv = new Vector2[vertices.Length];
        for(int y = 0,i = 0; y <= ySize; y++)
        {
            for(int x = 0; x <= xSize; x++, i++)
            {
                // 这里就是简单地进行平分,要强转一下类型,不然只有 0,1
                // 还要注意 texture 的 repeat mode
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }
        mesh.uv = uv;
    }
}
