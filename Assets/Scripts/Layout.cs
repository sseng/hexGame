using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Layout : MonoBehaviour {

	    public int width = 5;
	    public int height = 5;
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.blue;
	    public HexCell cellPrefab;
	    public Text cellLabelPrefab;
        [Range(0, 5)]
        public int spacing = 1;

        public Orientation Orientation { get; set; }

        private Hashtable cells = new Hashtable();
	    private Canvas gridCanvas;
        private float hexSpacing = 1.1f;

	    void Awake ()
        {
            hexSpacing = 1f + spacing * 5 / 100f;
		    gridCanvas = GetComponentInChildren<Canvas>();
            Orientation = LayoutOrientation(OrientationType.pointy);
            DrawHexBoard();
	    }

        private void DrawHexBoard()
        {
            var mapRadius = Mathf.FloorToInt(width / 2);

            for (int q = -mapRadius; q <= mapRadius; q++)
            {
                int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
                int r2 = Mathf.Min(mapRadius, -q + mapRadius);
                for (int r = r1; r <= r2; r++)
                {
                    CreateCell(new Hex(q, r, -q - r));
                }
            }
        }

        //private void DrawSquareBoard(float spacing)
        //{
        //    for (int z = 0; z < height; z++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            Vector3 position = new Vector3
        //            {
        //                x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f) * spacing,
        //                y = 0f,
        //                z = z * (HexMetrics.outerRadius * 1.5f) * spacing
        //            };
        //            var coordinates = new Hex(x, z);
        //            CreateCell(position, coordinates);
        //        }
        //    }
        //}

        void CreateCell(Hex hex)
        {
            var position = HexToPoint(hex);

            HexCell cell = Instantiate(cellPrefab);
            cell.Index = cells.Count;
            cell.Color = defaultColor;
            cell.Hex = hex;
            cell.Triangulate();

            cell.transform.SetParent(transform, false);
            cell.transform.position = position;


            if(cell.Index > 0)
                SetNeighbors(cell);
            AddLabelToCell(cell, position);

            cells.Add(hex, cell);
            
        }

        public void SetNeighbors(HexCell cell)
        {
            var directions = Enum.GetValues(typeof(HexDirection));
            foreach (HexDirection direction in directions)
            {
                var otherCell = (HexCell)cells[cell.HexNeighbor(direction)];
                if (otherCell != null)
                {
                    cell.SetNeighbor(direction, otherCell);
                }
            }
        }

        void AddLabelToCell(HexCell cell, Vector3 position)
        {
            Text label = Instantiate(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
            label.text = cell.Hex.ToStringWithIndex(cell.Index);
        }

        public void ColorCell(Vector3 point, Color color)
        {
            //position = transform.InverseTransformPoint(position);
            var hex = PointToHex(point);
            var pos = HexToPoint(hex);
            var cell = (HexCell)cells[hex];

            if (cell != null)
            {
                cell.Color = color;
                cell.Triangulate();
                Debug.Log($"Cell index: {cell.Index}  coord: {cell.Hex}  clicked: {point} cell pos: {cell.transform.position} color: {cell.Color}");
            }
            else
            {
                Debug.Log($"cannot find cell at {hex.ToString()}");
            }
        }

        public Orientation LayoutOrientation(OrientationType orienatation)
        {
            float sqrt3 = Mathf.Sqrt(3.0f);

            switch (orienatation)
            {
                case OrientationType.flat:
                    return new Orientation(3.0f/2.0f, 0f, sqrt3/2.0f, sqrt3, 2.0f/3.0f, 0f, -1.0f/3.0f, sqrt3/3.0f, 0f);

                case OrientationType.pointy:
                default:
                    return new Orientation(sqrt3, sqrt3/2f, 0f, 3.0f/2.0f, sqrt3/3.0f, -1.0f/3.0f, 0f, 2.0f/3.0f, 0.5f);
            }
        }

        //pointy = sqrt3, sqrt3/2, 0, 3/2
        public Vector3 HexToPoint(Hex hex)
        {
            var o = Orientation;
            var x = (o.F0 * hex.Q + o.F1 * hex.R) * HexMetrics.size * hexSpacing;
            var z = (o.F2 * hex.Q + o.F3 * hex.R) * HexMetrics.size * hexSpacing;

            return new Vector3(x, 0f, z);
        }

        //pointy = sqrt3/3, -1/3, 0f, 2/3
        public Hex PointToHex(Vector3 point)
        {
            var o = Orientation;
            double x = point.x / (HexMetrics.size * hexSpacing);
            double z = point.z / (HexMetrics.size * hexSpacing);
            var q = (o.B0 * x + o.B1 * z);
            var r = (o.B2 * x + o.B3 * z);

            return HexRound(q, r, -q - r);
        }

        private Hex HexRound(double x, double z, double y)
        {
            var q = Convert.ToInt32(Math.Round(x));
            var r = Convert.ToInt32(Math.Round(z));
            var s = Convert.ToInt32(Math.Round(y));
            var qDiff = Math.Abs(q - x);
            var rDiff = Math.Abs(r - z);
            var sDiff = Math.Abs(s - y);

            if (qDiff > rDiff && qDiff > sDiff)
                q = -r - s;
            else
            {
                if (rDiff > sDiff)
                    r = -q - s;
                else
                    s = -q - r;
            } 

            return new Hex(q, r, s);
        }
    }
}