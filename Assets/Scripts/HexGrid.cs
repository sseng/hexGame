using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HexGrid : MonoBehaviour {

	    public int width = 6;
	    public int height = 6;
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.blue;

	    public HexCell cellPrefab;
	    public Text cellLabelPrefab;

	    HexCell[] cells;
        HexMesh hexMesh;
	    Canvas gridCanvas;

	    void Awake ()
        {
		    gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

		    cells = new HexCell[height * width];

            DrawHexBoard();
            
	    }

        private void DrawHexBoard()
        {
            var radius = width/2;
            for (int q = -radius; q <= radius; q++)
            {
                int r1 = Mathf.Max(-radius, -q - radius);
                int r2 = Mathf.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    CreateCell(q, r, -q - r);
                }
            }
        }

        private void DrawSquareBoard()
        {
            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        }

        private void Start()
        {
            hexMesh.Triangulate(cells);
        }

        void CreateCell (int x, int z, int i)
        {
		    Vector3 position;
		    position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		    position.y = 0f;
		    position.z = z * (HexMetrics.outerRadius * 1.5f);

		    HexCell cell = cells[i] = Instantiate(cellPrefab);
		    cell.transform.SetParent(transform, false);
		    cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.color = defaultColor;

		    Text label = Instantiate(cellLabelPrefab);
		    label.rectTransform.SetParent(gridCanvas.transform, false);
		    label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
            label.text = cell.coordinates.ToStringOnSeparateLines();
	    }

        //void Update()
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        HandleInput();
        //    }
        //}

        //void HandleInput()
        //{
        //    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(inputRay, out hit))
        //    {
        //        ColorCell(hit.point, touchedColor);
        //    }
        //}

        public void ColorCell(Vector3 position, Color color)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            HexCell cell = cells[index];
            cell.color = color;
            hexMesh.Triangulate(cells);
            Debug.Log("touched at " + position);
        }
    }
}