using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class HexCell : MonoBehaviour
    {
        public OrientationType orientationType;
        public Color Color { get; set; }
        public int Index { get; set; }
        public Orientation Orientation { get; set; }
        public Hex Hex { get; set; }

        private HexMesh mesh;

        private void Awake()
        {
            mesh = GetComponent<HexMesh>();
        }

        public void Triangulate()
        {
            mesh.Triangulate(Color, HexMetrics.corners);
        }

        private Vector3[] Corners()
        {
            Vector3[] corners = new Vector3[7];
            for (int i = 0; i <= 6; i++)
            {
                //angle between 2 vertices is 2 pi / n. radius is the distance from center to a corner vertice.
                var angle = 2 * Mathf.PI * (Orientation.StartAngle + i) / 6;
                var x = HexMetrics.outerRadius * Mathf.Sin(angle);
                var z = HexMetrics.innerRadius * Mathf.Cos(angle);

                var corner = new Vector3(x, 0, z);
                corners[i] = corner;
            }
            return corners;
        }
    }
}