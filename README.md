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

<img src="https://https://github.com/ZhangQR/CatlikeBasic/raw/master/ReadmeImages/Graph01.gif" width="600px"/>  