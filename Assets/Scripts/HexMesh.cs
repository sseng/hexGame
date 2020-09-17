using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {
        Mesh mesh;
        MeshCollider meshCollider;
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            mesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        internal void Triangulate(HexCell[] cells)
        {
            mesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();

            for (int i = 0; i < cells.Length; i++)
            {
                Triangulate(cells[i]);
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.colors = colors.ToArray();
            meshCollider.sharedMesh = mesh;
        }

        void Triangulate(HexCell cell)
        {
            Vector3 center = cell.transform.localPosition;
            for (int i = 0; i < 6; i++)
            {
                AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i+1]);
                AddTriangleColor(cell.color);
            }
        }

        private void AddTriangleColor(Color color)
        {
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }
    }
}
