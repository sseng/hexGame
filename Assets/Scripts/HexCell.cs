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
        public HexCell[] neighbors;

        private HexMesh mesh;
        private readonly List<Hex> hexDirections = new List<Hex>()
        {
            new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1)
        };


        private void Awake()
        {
            mesh = GetComponent<HexMesh>();
            neighbors = new HexCell[6];
        }

        public void Triangulate()
        {
            mesh.Triangulate(Color, HexMetrics.corners);
        }

        public Hex HexNeighbor(HexDirection direction)
        {
            var neighbor = Hex.Add(hexDirections[(int)direction]);
            return neighbor;
        }

        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighbors[(int)direction];
        }

        public void SetNeighbor(HexDirection direction, HexCell otherCell)
        {
            neighbors[(int)direction] = otherCell;
            var opposite = Opposite(direction);
            otherCell.neighbors[(int)opposite] = this;
        }

        public HexDirection Opposite(HexDirection direction)
        {
            return (int)direction < 3 ? (direction + 3) : (direction - 3);
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

    public enum HexDirection
    {
        E, SE, SW, W, NW, NE
    }
}