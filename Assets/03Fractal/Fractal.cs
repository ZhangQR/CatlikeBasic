using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    [SerializeField] float depth = default;
    public void Start()
    {
        if (depth <= 0)
        {
            return;
        }
        Fractal up = CreateChild(Vector3.up, Quaternion.identity);
        Fractal right = CreateChild(Vector3.right, Quaternion.Euler(0, 0, -90));
        Fractal left = CreateChild(Vector3.left, Quaternion.Euler(0, 0, 90));
        Fractal down = CreateChild(Vector3.down, Quaternion.Euler(0, 0, 180));
        Fractal forword = CreateChild(Vector3.forward, Quaternion.Euler(90, 0, 0));
        Fractal back = CreateChild(Vector3.back, Quaternion.Euler(-90, 0, 0));

        up.transform.SetParent(transform, false);
        right.transform.SetParent(transform, false);
        left.transform.SetParent(transform, false);
        down.transform.SetParent(transform, false);
        forword.transform.SetParent(transform, false);
        back.transform.SetParent(transform, false);
    }

    private Fractal CreateChild(Vector3 direction,Quaternion rotation)
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
        transform.Rotate(new Vector3(0, 20f * Time.deltaTime, 0));
    }
}
