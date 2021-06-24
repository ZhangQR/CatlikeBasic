using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField, Range(8, 1000)] int solution;
    [SerializeField] ComputeShader computerShader;
    ComputeBuffer buffer;
    [SerializeField] Mesh mesh;
    [SerializeField] Material pointMaterial;
    private void Awake()
    {
        buffer = new ComputeBuffer(solution * solution, 3 * 4);
    }

    private void Update()
    {
        computerShader.SetFloat ("Time", Time.time);
        computerShader.SetFloat("Solution", solution);
        computerShader.SetBuffer(0, "Result", buffer);
        computerShader.Dispatch(0, solution/8, solution/8, 1);
        pointMaterial.SetBuffer("_Buffer", buffer);
        pointMaterial.SetFloat("_Scale", 2/ (float)solution);
        var bounds = new Bounds(Vector3.zero, Vector3.one * 2f);
        Graphics.DrawMeshInstancedProcedural(mesh, 0, pointMaterial,bounds,solution * solution);
    }
}
