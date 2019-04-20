using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshCombine : MonoBehaviour {
    private MeshFilter meshFilter;
	// Use this for initialization
	void Start () {
        meshFilter = GetComponent<MeshFilter>();
        MeshFilter[] childMeshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combines = new CombineInstance[childMeshFilters.Length];
        for (int i = 0; i < childMeshFilters.Length; i++)
        {
            combines[i].mesh = childMeshFilters[i].sharedMesh;
            combines[i].transform = childMeshFilters[i].transform.localToWorldMatrix;
            Debug.Log(childMeshFilters[i].name);
        }
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combines);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
