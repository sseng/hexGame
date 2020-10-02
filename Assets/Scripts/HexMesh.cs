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
        List<Vector3> corners;
        

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            mesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
        }

        public void Triangulate(Color color, Vector3[] corners)
        {
            mesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();

            Vector3 center = transform.position;
            for (int i = 0; i < 6; i++)
            {
                AddTriangle(center, center + corners[i], center + corners[i + 1]);
                AddTriangleColor(color);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.colors = colors.ToArray();
            meshCollider.sharedMesh = mesh;
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

        public void AddTriangleColor(Color color)
        {
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
        }
    }
}
