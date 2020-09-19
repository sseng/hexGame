using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HexGrid : MonoBehaviour {

	    public int width = 5;
	    public int height = 5;
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.blue;

	    public HexCell cellPrefab;
	    public Text cellLabelPrefab;

	    private List<HexCell> cells;
	    private Canvas gridCanvas;

	    void Awake ()
        {
		    gridCanvas = GetComponentInChildren<Canvas>();
            cells = new List<HexCell>();

            DrawHexBoard(BoardOrientation.flat, 0.585f);
            //DrawSquareBoard();
	    }

        private void DrawHexBoard(BoardOrientation orientation, float spacing)
        {
            var radius = Convert.ToInt32(Mathf.Floor(width / 2));

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = Mathf.Max(-radius, -q - radius);
                int r2 = Mathf.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    var position = new Vector3(0,0,0);
                    switch (orientation)
                    {
                        case BoardOrientation.pointy:
                            position.x = (HexMetrics.innerRadius * 3.0f / 2.0f * q) * spacing;
                            position.z = (HexMetrics.innerRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f)) * spacing;
                            break;

                        case BoardOrientation.flat:
                            position.x = (HexMetrics.innerRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f)) * spacing;
                            position.z = (HexMetrics.innerRadius * 3.0f / 2.0f * r) * spacing;
                            break;
                    }

                    CreateCell(position);
                }
            }
        }

        private void DrawSquareBoard()
        {
            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 position = new Vector3
                    {
                        x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f),
                        y = 0f,
                        z = z * (HexMetrics.outerRadius * 1.5f)
                    };
                    CreateCell(position);
                }
            }
        }

        //private void Start()
        //{
        //    hexMesh.TriangulateAll(cells);
        //}

        void CreateCell(Vector3 position)
        {
            var cell = Instantiate(cellPrefab);
            cells.Add(cell);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(position);
            cell.color = defaultColor;

            Text label = Instantiate(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
            label.text = cell.coordinates.ToStringOnSeparateLines();
        }

        public void ColorCell(Vector3 position, Color color)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            HexCell cell = cells[index];
            cell.color = color;
            //hexMesh.Triangulate(cells);
            Debug.Log("touched at " + position);
        }
    }

    public enum BoardOrientation
    {
        flat,
        pointy
    }
}