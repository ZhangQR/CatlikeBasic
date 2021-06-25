using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    [SerializeField] int depth = default;
    static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.left, Vector3.back, Vector3.forward };
    static Quaternion[] rotation = { Quaternion.identity, Quaternion.Euler(0,0,-90),
                                    Quaternion.Euler(0,0,90), Quaternion.Euler(-90,0,0), Quaternion.Euler(90,0,0)};
    Transform[][] fractals;
    private Mesh mesh;
    private Material material;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        material = GetComponent<MeshRenderer>().material;
        fractals = new Transform[depth][];
        fractals[0] = new Transform[1];
        fractals[0][0] = transform;
        float scale = transform.localScale.x;
        for (int i = 1; i < fractals.Length; i++)

        {
            scale *= 0.5f;
            fractals[i] = new Transform[(int)Mathf.Pow(5, i)];
            Transform[] partFractal = fractals[i];
            //for (int j = 0; j < partFractal.Length * 0.2f; j++)
            for (int j = 0; j < Mathf.Pow(5, i-1); j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    string name = "fractalC" + i.ToString() + "P" + j.ToString() + "I" + k.ToString();
                    GameObject go = new GameObject(name);
                    go.transform.localScale = Vector3.one * scale;
                    Transform parent = fractals[i - 1][j];
                    go.transform.localRotation = parent.rotation * rotation[k];
                    go.transform.localPosition = parent.position + 0.75f * scale * 2 * (parent.rotation * directions[k]); //parent.TransformDirection
                    go.AddComponent<MeshFilter>().mesh = mesh;
                    go.AddComponent<MeshRenderer>().material = material;
                    go.transform.SetParent(transform, false);
                    partFractal[5 * j + k] = go.transform;
                }
            }
        }
    }
    public void Start()
    {
    }

    private Fractal CreateChild(Vector3 direction, Quaternion rotation)
    {
        Fractal fractal = Instantiate(this);
        fractal.depth--;
        fractal.transform.localPosition = direction * 0.75f;
        fractal.transform.localRotation = rotation;
        fractal.transform.localScale = Vector3.one * 0.5f;
        return fractal;
    }

    private void Update()
    {
        //transform.Rotate(new Vector3(0, 20f * Time.deltaTime, 0));
    }
}
