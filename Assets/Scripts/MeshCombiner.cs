using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public List<MeshFilter> sourceMeshFilters;
    public MeshFilter targetMeshFilter;

    [ContextMenu("Combine Meshes")]
    private void CombineMeshes()
    {
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for (var i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;
        AssetDatabase.CreateAsset(mesh, "Assets/NewAsset.asset");
    }
}