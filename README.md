## 介绍
笨人（本人）就要站在巨人的肩膀上！ https://catlikecoding.com/  这一部分是基础，语法，数学之类的

## 3D 图

无意中算出来的奇葩图们：  
``` cs
    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p = Vector3.zero;
        float r = Cos(PI * v);
        p.x = r*Sin(PI * u);
        p.y = v;
        p.z = r*Cos(PI * u);
        return p;
    }
```

<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph01.jpg" width="300px"/>  

```cs
    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p = Vector3.zero;
        float r = Cos(v * PI * 0.5f + t);
        p.x = r*Sin(PI * u);
        p.y = Sin(PI * 0.5f * v);
        p.z = r*Cos(PI * u);
        return p;
    }
```

<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph02.gif" width="300px"/>  

```cs
    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p = Vector3.zero;
        float r = Sqrt(u * u + v * v);
        p.x = r * Sin(PI * u);
        p.y = 0;
        p.z = r * Cos(PI * u);
        return p;
    }
```
<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph03.jpg" width="300px"/>  


```cs
    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p = Vector3.zero;
        float s = Sin(t);
        float r = Sin(PI * v);
        // 就是把三个轴都进行统一的缩放;
        p.x = r * Sin(PI * u) * s;
        p.y = Cos(PI * v) * s;
        p.z = r * Cos(PI * u) * s;
        return p;
    }
```
<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph04.gif" width="300px"/>  


```cs
    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p = Vector3.zero;
        float s = Sin(8 * PI * u) * 0.1f + 0.9f;
        float r = Sin(PI * v);
        p.x = r * Sin(PI * u) * s;
        p.y = Cos(PI * v) * s;
        p.z = r * Cos(PI * u) * s;
        return p;
    }
```
上述形式中的 8，对应有几个 “角角”，然后 `* 0.1f + 0.9f`，其实是把 [-1,1] 变成 [0,8,1.0]，当然你也可以写成 `* 0.5f + 0.5f` ，那样 “胡桃” 会更明显点。如下图所示（其实应该有 16 个角角，但是 0.5 让负值消失了）：
<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph05.jpg" width="300px"/>  
下面的是 `* 0.8f + 0.2f`，范围是 [-0.6,1.0]，短的那 8 个是 [-0.6,0],长的是 [0,1]。  
<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph06.jpg" width="300px"/>  

阶段成品：  
<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph07.gif" width="700px"/>  

使用 Computer Shader，下图分辨率为 500（实际上即使是 1000，也可以轻松应对），可以看到依然很流畅。试想一下，如果创建 1000 * 1000 个物体，那基本上秒秒钟就要死机。  
<img src="https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph08.gif" width="700px"/>  
